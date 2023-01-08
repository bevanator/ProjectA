using System;
using UnityEngine;

namespace TGF.Utilities
{
    public static class AnimatorExtensions
    {
        public static void SetParameter(this Animator animator, int parameterHash, object value, AnimatorParamType valueType)
        {
            //Debug.Log("Anim: " + parameterHash);
            switch (valueType)
            {
                case AnimatorParamType.Int:
                    animator.SetInteger(parameterHash, (int)value);
                    break;
                case AnimatorParamType.Float:
                    animator.SetFloat(parameterHash, (float)value);
                    break;
                case AnimatorParamType.Bool:
                    animator.SetBool(parameterHash, (bool)value);
                    break;
                case AnimatorParamType.Trigger:
                    animator.SetTrigger(parameterHash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }
    }
    
    public enum AnimatorParamType
    {
        Int, Float, Bool, Trigger
    }
}
