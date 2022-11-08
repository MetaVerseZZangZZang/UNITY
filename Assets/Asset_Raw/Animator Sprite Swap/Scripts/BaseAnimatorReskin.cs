using UnityEngine;

namespace AnimatorSpriteSwapSystem
{
    public abstract class BaseAnimatorReskin : ScriptableObject
    {
        public string animatorName;
        public RuntimeAnimatorController animatorController;
    }
}