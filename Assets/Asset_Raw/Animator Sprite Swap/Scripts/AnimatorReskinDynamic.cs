using System.Collections.Generic;
using UnityEngine;

namespace AnimatorSpriteSwapSystem
{
    public class AnimatorReskinDynamic : BaseAnimatorReskin
    {
        public List<AnimatorStateDynamic> states;
        public int alternateCount;
        public DynamicSpriteSource dynamicSpriteSource;

        public void Initialize(RuntimeAnimatorController animatorController, List<AnimatorStateDynamic> states, int alternateCount, DynamicSpriteSource dynamicSpriteSource)
        {
            this.animatorName = animatorController.name;
            this.animatorController = animatorController;
            this.states = states;
            this.alternateCount = alternateCount;
            this.dynamicSpriteSource = dynamicSpriteSource;
        }
    }

    [System.Serializable]
    public struct AnimatorStateDynamic
    {
        public string name;
        public int hash;
        public KeyframeDynamic[] keyframes;
        public bool isLooping;

        public AnimatorStateDynamic(string name, int hash, KeyframeDynamic[] keyframes, bool isLooping)
        {
            this.name = name;
            this.hash = hash;
            this.keyframes = keyframes;
            this.isLooping = isLooping;
        }
    }

    [System.Serializable]
    public struct KeyframeDynamic
    {
        public Sprite[] sprites;
        public int frameNumber;
        public float normalizedTime;

        public KeyframeDynamic(Sprite[] sprites, int frameNumber, float normalizedTime)
        {
            this.sprites = sprites;
            this.frameNumber = frameNumber;
            this.normalizedTime = normalizedTime;
        }
    }

    public enum DynamicSpriteSource
    {
        SameTexture,
        SeparateTextures
    }
}