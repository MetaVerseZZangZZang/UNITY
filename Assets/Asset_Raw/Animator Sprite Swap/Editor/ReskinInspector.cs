using UnityEngine;
using UnityEditor;

namespace AnimatorSpriteSwapSystem
{
    public class BaseReskinInspector : Editor
    {
        private MessageType messageType;
        private string messageString;

        private void OnEnable()
        {
            SetMessage("Click on Update to regenerate the animation frame data. \n\nWarning: Manual changes to the Scriptable Object asset frame data will be lost.", MessageType.Info);
        }

        public override void OnInspectorGUI()
        {
            //Adds an update button in the reskin scriptable object inspector
            if (GUILayout.Button("Update"))
            {
                AnimatorReskinWindow window = new AnimatorReskinWindow();
                BaseAnimatorReskin editedObject = this.serializedObject.targetObject as BaseAnimatorReskin;
                if (!window.CheckIfObjectCanBeUpdated(editedObject))
                {
                    SetMessage("Failed to update automatically. Fill in the missing data in the Reskin Window.", MessageType.Warning);
                    window.Show();
                }
                else
                {
                    SetMessage("Successfully updated. No issues encountered.", MessageType.Info);
                }
            }

            EditorGUILayout.HelpBox(messageString, messageType, true);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            base.OnInspectorGUI();
        }
        
        private void SetMessage(string message, MessageType type)
        {
            messageString = message;
            messageType = type;
        }
    }

    [CustomEditor(typeof(AnimatorReskin))]
    public class AnimatorReskinInspector : BaseReskinInspector
    {

    }

    [CustomEditor(typeof(AnimatorReskinDynamic))]
    public class AnimatorReskinDynamicInspector : BaseReskinInspector
    {

    }
}