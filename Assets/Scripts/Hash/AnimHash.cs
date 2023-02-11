using UnityEngine;
namespace ProjectA.Hash
{
    public static class AnimHash
    {
        public static readonly int Running = Animator.StringToHash("Running");
        public static readonly int Jump1 = Animator.StringToHash("Jump");
        public static readonly int Land = Animator.StringToHash("Land");
        public static readonly int Floating = Animator.StringToHash("Float");
        public static readonly int Slash1 = Animator.StringToHash("Slash1");
        public static readonly int RunningSword = Animator.StringToHash("RunningSword");
    }
}