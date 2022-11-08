using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Sprites;

namespace AnimatorSpriteSwapSystem
{
    public class AnimatorReskinWindow : EditorWindow
    {
        public AnimatorController animatorControllerSource;
        public BaseAnimatorReskin updateReskinSource;

        private Vector2 windowScrollPosition;

        private enum WindowState
        {
            MainWindow,
            UpdateScriptableObject,
            SheetSelection,
        }
        private enum ReskinType
        {
            Regular,
            Dynamic
        }

        private WindowState windowState = WindowState.MainWindow;
        private ReskinType reskinType = ReskinType.Regular;
        private DynamicSpriteSource dynamicSpriteSource = DynamicSpriteSource.SameTexture;
        private MessageType messageType;
        private string messageString;
        private int spriteAlternateCount = 1;

        HashSet<string> generationErrors;
        List<AnimationStateData> animationStates;
        List<ReplacementTexture> textures;

        [MenuItem("Tools/Animation/Create Reskin Assets")]
        static void Init()
        {
            AnimatorReskinWindow window = GetWindow<AnimatorReskinWindow>(title:"Create Reskin Assets");
            window.ChangeWindowState(WindowState.MainWindow);
            window.Show();
        }

        [MenuItem("Tools/Animation/Update Reskin Assets")]
        static void Initi()
        {
            AnimatorReskinWindow window = GetWindow<AnimatorReskinWindow>(title: "Update Reskin Assets");
            window.ChangeWindowState(WindowState.UpdateScriptableObject);
            window.Show();
        }

        private void OnEnable()
        {
            ChangeWindowState(windowState == WindowState.UpdateScriptableObject ? WindowState.UpdateScriptableObject : WindowState.MainWindow);
        }

        private void OnGUI()
        {
            windowScrollPosition = EditorGUILayout.BeginScrollView(windowScrollPosition);
            switch (windowState)
            {
                case WindowState.MainWindow:
                    {
                        reskinType = (ReskinType)GUILayout.Toolbar((int)reskinType, new string[] { "Regular", "Dynamic" });

                        EditorGUILayout.HelpBox("Select Animation Controller asset you want to create a Reskin Asset for.", MessageType.Info, true);
                        GUILayout.Label("Source Animator Controller", EditorStyles.label);
                        animatorControllerSource = EditorGUILayout.ObjectField(new GUIContent("Animator Controller",
                            "The reference Animator Controller asset to be used for the generated Reskin Asset."), animatorControllerSource, typeof(AnimatorController), false) as AnimatorController;

                        if (reskinType == ReskinType.Dynamic)
                        {
                            GUILayout.Label("Alternate sprite source:", EditorStyles.label);
                            bool toggleSame = dynamicSpriteSource == DynamicSpriteSource.SameTexture;
                            toggleSame = EditorGUILayout.Toggle(new GUIContent("Same Texture",
                                "Same Texture is used when alternate sprites for each animation are stored in the same file. For example, Idle animation and all its alternative sprites are the same spritesheet and each set of alternate animation sprites are ordered one after another (Idle_Left uses sprites 1-4, Idle_Bottom uses sprites 5-8, Idle_Right uses sprites 9-12, Idle_Top uses sprites 13-16)."), toggleSame, "Radio");
                            if (toggleSame) dynamicSpriteSource = DynamicSpriteSource.SameTexture;
                            bool toggleDifferent = dynamicSpriteSource == DynamicSpriteSource.SeparateTextures;
                            toggleDifferent = EditorGUILayout.Toggle(new GUIContent("Separate Textures",
                                "Separate Textures is used when alternate sprites for each animation are stored in separate files following the same sprite order. For example, Idle_Left and Walking_Left are stored in spritesheet Character_Left.png, Idle_Bottom and Walking_Bottom are stored in spritesheet Character_Bottom.png, etc."), toggleDifferent, "Radio");
                            if (toggleDifferent) dynamicSpriteSource = DynamicSpriteSource.SeparateTextures;


                            spriteAlternateCount = EditorGUILayout.IntField(new GUIContent("Sprite Alternate Count",
                                "Alternate animation count (e.g. for Top/Down/Left/Right direction animation setup, Sprite Alternate Count should be 4)"), spriteAlternateCount);
                        }
                        if (GUILayout.Button("Generate"))
                        {
                            if (animatorControllerSource != null)
                            {
                                if (reskinType == ReskinType.Dynamic && spriteAlternateCount <= 0)
                                {
                                    SetMessage("Sprite Count cannot be less than 1. Please set a different value.", MessageType.Warning);
                                }
                                else
                                {
                                    CollectAnimatorData(animatorControllerSource);
                                    ChangeWindowState(WindowState.SheetSelection);
                                }
                            }
                            else
                            {
                                SetMessage("Please select an Animator Controller asset.", MessageType.Warning);
                            }
                        }
                        break;
                    }
                case WindowState.UpdateScriptableObject:
                    {
                        EditorGUILayout.HelpBox("Select the Reskin Asset you want to update and its source Animator Controller asset." +
                            "\nThe Reskin Asset will be updated using the data from the selected Animator Controller asset. " +
                            "\nNew animation sprites will be automatically replaced with sprites from the replacement spritesheets already referenced in the asset.", MessageType.Info, true);

                        GUILayout.Label("Source Reskin Asset", EditorStyles.label);
                        updateReskinSource = EditorGUILayout.ObjectField(new GUIContent("Reskin Asset",
                            "The reference Animator Controller asset to be used for the generated Reskin Asset."), updateReskinSource, typeof(BaseAnimatorReskin), false) as BaseAnimatorReskin;

                        GUILayout.Label("Source Animator Controller", EditorStyles.label);
                        animatorControllerSource = EditorGUILayout.ObjectField(new GUIContent("Animator Controller",
                            "The reference Animator Controller asset to be used for the generated Reskin Asset."), animatorControllerSource, typeof(AnimatorController), false) as AnimatorController;

                        if (GUILayout.Button("Update"))
                        {
                            if (updateReskinSource != null && animatorControllerSource != null)
                            {
                                CheckIfObjectCanBeUpdated(updateReskinSource, animatorControllerSource);
                            }
                            else
                            {
                                SetMessage("Unassigned Reskin Asset or Animator Controller.", MessageType.Warning);
                            }
                        }
                        break;
                    }
                case WindowState.SheetSelection:
                    {
                        EditorGUILayout.HelpBox("Select spritesheet replacements.", MessageType.Info, true);
                        for (int i = 0; i < textures.Count; i++)
                        {
                            GUILayout.Label(textures[i].original.name, EditorStyles.label);
                            GUILayout.BeginHorizontal();
                            GUILayout.Box(AssetPreview.GetAssetPreview(textures[i].original), GUILayout.Width(125), GUILayout.Height(125));
                            GUILayout.BeginVertical();
                            for (int j = 0; j < textures[i].replacement.Length; j++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Box(AssetPreview.GetAssetPreview(textures[i].replacement[j]), GUILayout.Width(125), GUILayout.Height(125));
                                GUILayout.BeginVertical();
                                EditorGUILayout.HelpBox("Select replacement texture " + (dynamicSpriteSource == DynamicSpriteSource.SeparateTextures ? "#" + (j + 1) + " " : "") + "for " + textures[i].original.name, MessageType.Info, true);
                                Texture placeholder = textures[i].replacement[j];
                                placeholder = EditorGUILayout.ObjectField(placeholder, typeof(Texture), false) as Texture;
                                textures[i].replacement[j] = placeholder;
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                        }

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Cancel"))
                        {
                            ChangeWindowState(WindowState.MainWindow);
                        }

                        if (updateReskinSource == null)
                        {
                            if (GUILayout.Button("Generate"))
                            {
                                CheckReplacementTexturesAssigned();
                                SaveTemplate();
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("Update"))
                            {
                                CheckReplacementTexturesAssigned();
                                SaveTemplate();
                            }
                        }

                        GUILayout.EndHorizontal();
                        break;
                    }
            }

            EditorGUILayout.HelpBox(messageString, messageType, true);
            EditorGUILayout.EndScrollView();
        }

        public bool CheckIfObjectCanBeUpdated(BaseAnimatorReskin baseAnimatorReskin, AnimatorController animatorController = null)
        {
            updateReskinSource = baseAnimatorReskin;
            animatorControllerSource = animatorController != null ? animatorController : updateReskinSource.animatorController as AnimatorController;

            if (animatorControllerSource)
            {
                if (baseAnimatorReskin is AnimatorReskinDynamic)
                    reskinType = ReskinType.Dynamic;
                else
                    reskinType = ReskinType.Regular;
                
                switch (reskinType)
                {
                    case ReskinType.Regular:
                        {
                            AnimatorReskin animatorReskin = baseAnimatorReskin as AnimatorReskin;
                            CollectAnimatorData(animatorControllerSource);

                            for (int i = 0; i < animatorReskin.states.Count; i++)
                            {
                                for (int j = 0; j < animationStates.Count; j++)
                                {
                                    if (animationStates[j].hash == animatorReskin.states[i].hash)
                                    {
                                        for (int k = 0; k < animatorReskin.states[i].keyframes.Length; k++)
                                        {
                                            for (int l = 0; l < animationStates[j].data.frames.Count; l++)
                                            {
                                                for (int m = 0; m < textures.Count; m++)
                                                {
                                                    if (textures[m].original == SpriteUtility.GetSpriteTexture(animationStates[j].data.frames[l].sprite, false))
                                                    {
                                                        textures[m] = new ReplacementTexture(textures[m].original, new Texture[] { SpriteUtility.GetSpriteTexture(animatorReskin.states[i].keyframes[k].sprite, false) });
                                                        break;
                                                    }
                                                }

                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            int replacementCount = 0;
                            for (int i = 0; i < textures.Count; i++)
                                if (textures[i].replacement != null)
                                    if (textures[i].replacement[0])
                                        replacementCount++;
                            if (replacementCount == textures.Count)
                            {
                                SaveTemplate();
                                return true;
                            }
                        }
                        break;
                    case ReskinType.Dynamic:
                        {
                            AnimatorReskinDynamic animatorReskinDynamic = baseAnimatorReskin as AnimatorReskinDynamic;
                            dynamicSpriteSource = animatorReskinDynamic.dynamicSpriteSource;
                            spriteAlternateCount = animatorReskinDynamic.alternateCount;
                            CollectAnimatorData(animatorControllerSource);

                            for (int i = 0; i < animatorReskinDynamic.states.Count; i++)
                            {
                                for (int j = 0; j < animationStates.Count; j++)
                                {
                                    if (animationStates[j].hash == animatorReskinDynamic.states[i].hash)
                                    {
                                        for (int k = 0; k < animatorReskinDynamic.states[i].keyframes.Length; k++)
                                        {
                                            for (int l = 0; l < animationStates[j].data.frames.Count; l++)
                                            {
                                                for (int m = 0; m < textures.Count; m++)
                                                {
                                                    if (textures[m].original == SpriteUtility.GetSpriteTexture(animationStates[j].data.frames[l].sprite, false))
                                                    {
                                                        Texture[] replacementTextures = new Texture[dynamicSpriteSource == DynamicSpriteSource.SeparateTextures ? spriteAlternateCount : 1];
                                                        for (int n = 0; n < replacementTextures.Length; n++)
                                                        {
                                                            if(n < animatorReskinDynamic.states[i].keyframes[k].sprites.Length)
                                                                replacementTextures[n] = SpriteUtility.GetSpriteTexture(animatorReskinDynamic.states[i].keyframes[k].sprites[n], false);
                                                        }

                                                        textures[m] = new ReplacementTexture(textures[m].original, replacementTextures);
                                                        break;
                                                    }
                                                }

                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            int replacementCount = 0;
                            for (int i = 0; i < textures.Count; i++)
                            {
                                if (textures[i].replacement != null)
                                {
                                    for (int j = 0; j < textures[i].replacement.Length; j++)
                                    {
                                        if(textures[i].replacement[j])
                                            replacementCount++;
                                    }
                                }
                            }
                            
                            if ((dynamicSpriteSource == DynamicSpriteSource.SameTexture && replacementCount == textures.Count) || (dynamicSpriteSource == DynamicSpriteSource.SeparateTextures && replacementCount == textures.Count * spriteAlternateCount))
                            {
                                SaveTemplate();
                                return true;
                            }

                        }
                        break;
                }
                
                ChangeWindowState(WindowState.SheetSelection);
                return false;
            }
            else
            {
                ChangeWindowState(WindowState.UpdateScriptableObject);
                return false;
            }
        }

        private void CollectAnimatorData(AnimatorController controller)
        {
            generationErrors = new HashSet<string>();
            animationStates = new List<AnimationStateData>();

            //Find all animation states in the animation controller
            foreach (AnimatorControllerLayer layer in controller.layers)
                animationStates.AddRange(GetStates_Layer(layer, layer.name));

            if (animationStates.Count == 0)
            {
                SetMessage("No states were found in the selected Animator Controller. \nPlease select a valid Animator Controller asset.", MessageType.Error);
                return;
            }

            //Get all textures from sprites used by the animation states
            textures = GetTextures();
        }

        /// <summary>
        /// Returns all animation state data found in the animation layer.
        /// </summary>
        private List<AnimationStateData> GetStates_Layer(AnimatorControllerLayer layer, string directory)
        {
            List<AnimationStateData> states = new List<AnimationStateData>();
            foreach (ChildAnimatorState animatorState in layer.stateMachine.states)
                GetState(ref states, directory, animatorState);
            foreach (ChildAnimatorStateMachine stateMachine in layer.stateMachine.stateMachines)
                states.AddRange(GetStates_StateMachine(stateMachine, directory + "." + stateMachine.stateMachine.name));
            return states;
        }

        /// <summary>
        /// Returns all animation state data found in the animation state machine.
        /// </summary>
        private List<AnimationStateData> GetStates_StateMachine(ChildAnimatorStateMachine stateMachine, string directory)
        {
            List<AnimationStateData> states = new List<AnimationStateData>();
            foreach (ChildAnimatorState animatorState in stateMachine.stateMachine.states)
                GetState(ref states, directory, animatorState);
            foreach (ChildAnimatorStateMachine _stateMachine in stateMachine.stateMachine.stateMachines)
                states.AddRange(GetStates_StateMachine(_stateMachine, directory + "." + _stateMachine.stateMachine.name));
            return states;
        }

        /// <summary>
        /// Adds animation state data to the AnimationStateData list.
        /// </summary>
        private void GetState(ref List<AnimationStateData> states, string directory, ChildAnimatorState animatorState)
        {
            string stateDir = directory + "." + animatorState.state.name;

            if (animatorState.state.motion)
            {
                AnimationClipData animationClipData = GetClipData(animatorState.state.motion as AnimationClip);
                if (animationClipData != null)
                    states.Add(new AnimationStateData(stateDir, Animator.StringToHash(stateDir), GetClipData(animatorState.state.motion as AnimationClip)));
                else
                    Debug.LogWarning("State \"" + stateDir + "\" animation clip does not have sprite data. State will be ignored.");
            }
            else
                Debug.LogWarning("State \"" + stateDir + "\" does not have an animation clip assigned. State will be ignored.");
        }

        /// <summary>
        /// Returns all sprite keyframe data stored in the clip.
        /// </summary>
        private AnimationClipData GetClipData(AnimationClip clip)
        {
            if (clip == null)
                return null;
            EditorCurveBinding[] curveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            foreach (EditorCurveBinding binding in curveBindings)
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                if (keyframes != null)
                {
                    //Gets all Sprite keyframes (can be modified to get different types)
                    if (binding.path == "" && binding.propertyName == "m_Sprite" && keyframes.Length > 0)
                    {
                        List<Keyframe> frames = new List<Keyframe>();
                        for (int i = 0; i < keyframes.Length; i++)
                        {
                            Sprite sprite = keyframes[i].value as Sprite;
                            frames.Add(new Keyframe(sprite, Mathf.RoundToInt(keyframes[i].time * clip.frameRate), (keyframes[i].time / (keyframes[keyframes.Length - 1].time + 1 / clip.frameRate))));
                        }
                        return new AnimationClipData(clip, frames);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all references to textures from sprites used by the animation states.
        /// </summary>
        private List<ReplacementTexture> GetTextures()
        {
            List<ReplacementTexture> allTextures = new List<ReplacementTexture>();
            for (int i = 0; i < animationStates.Count; i++)
            {
                for (int j = 0; j < animationStates[i].data.frames.Count; j++)
                {
                    if (animationStates[i].data.frames[j].sprite)
                    {
                        bool contains = false;
                        for (int k = 0; k < allTextures.Count; k++)
                        {
                            if (allTextures[k].original == SpriteUtility.GetSpriteTexture(animationStates[i].data.frames[j].sprite, false))
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                            allTextures.Add(new ReplacementTexture(SpriteUtility.GetSpriteTexture(animationStates[i].data.frames[j].sprite, false), new Texture[reskinType == ReskinType.Regular || dynamicSpriteSource != DynamicSpriteSource.SeparateTextures ? 1 : spriteAlternateCount]));
                    }
                }
            }
            return allTextures;
        }

        /// <summary>
        /// Check if replacement textures are assigned, logs warnings if any related issues are found.
        /// </summary>
        private void CheckReplacementTexturesAssigned()
        {
            foreach (ReplacementTexture texture in textures)
            {
                if (texture.replacement != null)
                {
                    for (int i = 0; i < texture.replacement.Length; i++)
                    {
                        if (!texture.replacement[i])
                        {
                            Debug.LogWarning("No replacement texture for " + texture.original + " was selected. Original texture sprites will be used instead.");
                        }
                        else
                        {
                            if (reskinType == ReskinType.Regular || (reskinType == ReskinType.Dynamic && dynamicSpriteSource == DynamicSpriteSource.SeparateTextures))
                            {
                                int originalTextureSpriteCount = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture.original)).Length;
                                int replacementTextureSpriteCount = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture.replacement[i])).Length;
                                if (replacementTextureSpriteCount != originalTextureSpriteCount)
                                    Debug.LogWarning("Sprite count missmatch for original texture \"" + texture.original.name + "\" (sprite count: " + (originalTextureSpriteCount - 1) + ") and selected replacement texture \"" + texture.replacement[i].name + "\" (sprite count: " + (replacementTextureSpriteCount - 1) + ")! \nGenerated asset may not work as expected. Original texture sprites will be used instead for missing sprites.");
                            }
                            else if (reskinType == ReskinType.Dynamic && dynamicSpriteSource == DynamicSpriteSource.SameTexture)
                            {

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the equivalent sprite for the original sprite from the replacement texture.
        /// </summary>
        private Sprite FindReplacementSprite(Sprite original, int replacementTextureIndex = 0, int spriteIndexOffset = 0)
        {
            Sprite replacementSprite = original;
            if (original != null)
            {
                Texture2D originalTexture = SpriteUtility.GetSpriteTexture(original, false);
                Object[] originalSprites = FilterAndSort(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(originalTexture)));
                for (int k = 0; k < originalSprites.Length; k++)
                {
                    if (originalSprites[k] as Sprite != null)
                    {
                        if (original == originalSprites[k] as Sprite)
                        {
                            Texture replacementTexture = textures[textures.FindIndex(x => x.original == originalTexture)].replacement[replacementTextureIndex];
                            if (replacementTexture != null)
                            {
                                Object[] replacementSprites = FilterAndSort(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(replacementTexture)));

                                if (k + spriteIndexOffset < replacementSprites.Length)
                                    replacementSprite = replacementSprites[k + spriteIndexOffset] as Sprite;
                                else
                                    generationErrors.Add(
                                        "IndexOutOfRangeException. Please make sure the right textures were used and Sprite Alternate Count value is correct. " +
                                        "Generated asset may not work as expected. Original texture sprites will be used instead for missing sprites. "
                                        );
                            }
                            break;
                        }
                    }
                }
            }
            return replacementSprite;
        }

        private Object[] FilterAndSort(Object[] objects)
        {
            //Sort sprites by name to prevent cases where AssetDatabase.LoadAllAssetsAtPath messes the sprite order up (may cause some issues if you manually renamed sprites in the sprite editor)
            System.Array.Sort(objects, delegate (Object x, Object y) { return PadNumbers(x.name).CompareTo(PadNumbers(y.name)); });
            return System.Array.FindAll(objects, o => o.GetType() == typeof(Sprite));
        }

        private string PadNumbers(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }


        /// <summary>
        /// Generates the finalized AnimatorState list for the AnimatorReskin asset
        /// </summary>
        private List<AnimatorState> MapKeyframes()
        {
            List<AnimatorState> animatorStates = new List<AnimatorState>();
            for (int i = 0; i < animationStates.Count; i++)
            {
                Keyframe[] keyframes = new Keyframe[animationStates[i].data.frames.Count];
                for (int j = 0; j < animationStates[i].data.frames.Count; j++)
                {
                    Sprite replacementSprite = FindReplacementSprite(animationStates[i].data.frames[j].sprite);
                    keyframes[j] = new Keyframe(replacementSprite, animationStates[i].data.frames[j].frameNumber, animationStates[i].data.frames[j].normalizedTime);
                }
                animatorStates.Add(new AnimatorState(animationStates[i].name, animationStates[i].hash, keyframes, animationStates[i].data.clip.isLooping));
            }
            return animatorStates;
        }

        /// <summary>
        /// Generates the finalized AnimatorStateDynamic list for the AnimatorReskinDynamic asset.
        /// </summary>
        private List<AnimatorStateDynamic> MapKeyframesDynamic()
        {
            List<AnimatorStateDynamic> animatorStates = new List<AnimatorStateDynamic>();
            for (int i = 0; i < animationStates.Count; i++)
            {
                KeyframeDynamic[] keyframes = new KeyframeDynamic[animationStates[i].data.frames.Count];
                for (int j = 0; j < animationStates[i].data.frames.Count; j++)
                {
                    Sprite[] replacementSprites = new Sprite[spriteAlternateCount];
                    for (int k = 0; k < replacementSprites.Length; k++)
                    {
                        switch (dynamicSpriteSource)
                        {
                            case DynamicSpriteSource.SameTexture:
                                replacementSprites[k] = FindReplacementSprite(animationStates[i].data.frames[j].sprite, 0, k * animationStates[i].data.frames.Count);
                                break;
                            case DynamicSpriteSource.SeparateTextures:
                                replacementSprites[k] = FindReplacementSprite(animationStates[i].data.frames[j].sprite, k, 0);
                                break;
                        }
                    }
                    keyframes[j] = new KeyframeDynamic(replacementSprites, animationStates[i].data.frames[j].frameNumber, animationStates[i].data.frames[j].normalizedTime);
                }
                animatorStates.Add(new AnimatorStateDynamic(animationStates[i].name, animationStates[i].hash, keyframes, animationStates[i].data.clip.isLooping));
            }
            return animatorStates;
        }

        /// <summary>
        /// Generates and saves reskin assets.
        /// </summary>
        private void SaveTemplate()
        {
            switch (reskinType)
            {
                case ReskinType.Regular:
                    {
                        List<AnimatorState> states = MapKeyframes();
                        foreach (string error in generationErrors)
                            Debug.LogError(error);
                        AnimatorReskin reskin = CreateInstance(typeof(AnimatorReskin)) as AnimatorReskin;
                        reskin.Initialize(animatorControllerSource, states);

                        if (updateReskinSource)
                            OverrideAsset(reskin);
                        else
                            CreateAsset(reskin);
                    }
                    break;
                case ReskinType.Dynamic:
                    {
                        List<AnimatorStateDynamic> states = MapKeyframesDynamic();
                        foreach (string error in generationErrors)
                            Debug.LogError(error);
                        AnimatorReskinDynamic reskin = CreateInstance(typeof(AnimatorReskinDynamic)) as AnimatorReskinDynamic;
                        reskin.Initialize(animatorControllerSource, states, spriteAlternateCount, dynamicSpriteSource);

                        if (updateReskinSource)
                            OverrideAsset(reskin);
                        else
                            CreateAsset(reskin);
                    }
                    break;
            }
        }

        /// <summary>
        /// Names and creates the scriptable object asset.
        /// </summary>
        private void CreateAsset(Object reskin)
        {
            string assetDirectory = "Assets/" + animatorControllerSource.name + "_Template";
            int index = 0;
            while (File.Exists(assetDirectory + (index != 0 ? " (" + index + ")" : "") + ".asset"))
                index++;
            string fullAssetDirectory = assetDirectory + (index != 0 ? " (" + index + ")" : "") + ".asset";
            AssetDatabase.CreateAsset(reskin, fullAssetDirectory);
            EditorGUIUtility.PingObject(reskin);
            ChangeWindowState(WindowState.MainWindow);
            SetMessage("Successfully created asset at " + fullAssetDirectory + ".", MessageType.Info);
        }

        /// <summary>
        /// Updates the scriptable object asset.
        /// </summary>
        private void OverrideAsset(Object reskin)
        {
            ChangeWindowState(WindowState.UpdateScriptableObject);

            switch (reskinType)
            {
                case ReskinType.Regular:
                    AnimatorReskin reskinRegular = reskin as AnimatorReskin;
                    (updateReskinSource as AnimatorReskin).Initialize(reskinRegular.animatorController as RuntimeAnimatorController, reskinRegular.states);
                    break;
                case ReskinType.Dynamic:
                    AnimatorReskinDynamic reskinDynamic = reskin as AnimatorReskinDynamic;
                    (updateReskinSource as AnimatorReskinDynamic).Initialize(reskinDynamic.animatorController as RuntimeAnimatorController, reskinDynamic.states, reskinDynamic.alternateCount, reskinDynamic.dynamicSpriteSource);
                    break;
            }

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(updateReskinSource);
            EditorGUIUtility.PingObject(updateReskinSource);
            SetMessage("Successfully updated " + updateReskinSource.name + ".", MessageType.Info);
            Reset();
        }


        private void ChangeWindowState(WindowState newState)
        {
            ResetMessage();

            switch (newState)
            {
                case WindowState.MainWindow:
                    Reset();
                    break;
                case WindowState.SheetSelection:
                    break;
            }

            windowState = newState;
        }

        private void Reset()
        {
            generationErrors = null;
            animationStates = null;
            textures = null;
        }

        private void SetMessage(string message, MessageType type)
        {
            messageString = message;
            messageType = type;
        }

        private void ResetMessage()
        {
            messageString = "";
            messageType = MessageType.None;
        }

        private struct ReplacementTexture
        {
            public readonly Texture original;
            public Texture[] replacement;

            public ReplacementTexture(Texture original, Texture[] replacement)
            {
                this.original = original;
                this.replacement = replacement;
            }
        }

        private struct AnimationStateData
        {
            public readonly string name;
            public readonly int hash;
            public AnimationClipData data;

            public AnimationStateData(string name, int hash, AnimationClipData data)
            {
                this.name = name;
                this.hash = hash;
                this.data = data;
            }
        }

        private class AnimationClipData
        {
            public readonly AnimationClip clip;
            public readonly List<Keyframe> frames;

            public AnimationClipData(AnimationClip clip, List<Keyframe> frames)
            {
                this.clip = clip;
                this.frames = frames;
            }
        }
    }
}