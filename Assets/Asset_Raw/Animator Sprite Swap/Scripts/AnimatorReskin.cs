using System.Collections.Generic;
using UnityEngine;

namespace AnimatorSpriteSwapSystem
{
    public class AnimatorReskin : BaseAnimatorReskin
    {
        public List<AnimatorState> states;

        public void Initialize(RuntimeAnimatorController animatorController, List<AnimatorState> states)
        {
            this.animatorName = animatorController.name;
            this.animatorController = animatorController;
            this.states = states;
        }
    }

    [System.Serializable]
    public struct AnimatorState
    {
        public string name;
        public int hash;
        public Keyframe[] keyframes;
        public bool isLooping;

        public AnimatorState(string name, int hash, Keyframe[] keyframes, bool isLooping)
        {
            this.name = name;
            this.hash = hash;
            this.keyframes = keyframes;
            this.isLooping = isLooping;
        }
    }

    [System.Serializable]
    public struct Keyframe
    {
        public Sprite sprite;
        public int frameNumber;
        public float normalizedTime;

        public Keyframe(Sprite sprite, int frameNumber, float normalizedTime)
        {
            this.sprite = sprite;
            this.frameNumber = frameNumber;
            this.normalizedTime = normalizedTime;
        }
    }
}