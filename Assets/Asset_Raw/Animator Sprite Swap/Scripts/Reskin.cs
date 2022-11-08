using UnityEngine;

namespace AnimatorSpriteSwapSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Reskin : MonoBehaviour
    {
        //Reference to a AnimatorReskin asset
        public AnimatorReskin reskinAsset;
        //Component reference to the SpriteRenderer component
        private SpriteRenderer spriteRenderer;
        //Component reference to the Animator component
        private Animator animator;
        //Is reskinning active
        private bool isActive = false;
        //Normalized time of the current animation state
        private float normalizedTime;
        //Current keyframe index of the current animation state
        private int keyframeIndex;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            isActive = reskinAsset != null;
        }

        private void LateUpdate()
        {
            if (isActive)
            {
                AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                //Get the current normalized time of the current animation 
                normalizedTime = Mathf.Repeat(animatorStateInfo.normalizedTime, 1.0f);

                //Get the current animation state hash 
                int hash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                for (int i = 0; i < reskinAsset.states.Count; i++)
                {
                    //Find the corresponding hash in the reskin asset state list 
                    if (reskinAsset.states[i].hash == hash)
                    {
                        //Check if animation state is looping
                        if (!reskinAsset.states[i].isLooping && animatorStateInfo.normalizedTime >= 1.0f)
                        {
                            keyframeIndex = reskinAsset.states[i].keyframes.Length - 1;
                        }
                        else
                            //Choose the correct animation keyframe based on the normalized time
                            for (int j = keyframeIndex; j < reskinAsset.states[i].keyframes.Length + keyframeIndex; j++)
                            {
                                float rangeStart = reskinAsset.states[i].keyframes[(j + keyframeIndex) % reskinAsset.states[i].keyframes.Length].normalizedTime;
                                float rangeEnd = reskinAsset.states[i].keyframes[(j + keyframeIndex + 1) % reskinAsset.states[i].keyframes.Length].normalizedTime + (((j + keyframeIndex + 1) % reskinAsset.states[i].keyframes.Length == 0) ? 1.0f : 0.0f);
                                if (normalizedTime >= rangeStart && normalizedTime < rangeEnd)
                                {
                                    keyframeIndex = (j + keyframeIndex) % reskinAsset.states[i].keyframes.Length;
                                    break;
                                }
                            }
                        //Set the sprite value based on the animation keyframe index
                        spriteRenderer.sprite = reskinAsset.states[i].keyframes[keyframeIndex].sprite;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Replaces the active reskin asset
        /// </summary>
        public void SetReskin(AnimatorReskin newReskin)
        {
            isActive = newReskin != null;
            reskinAsset = newReskin;
        }
    }
}