using UnityEngine;

public static class PlayerAnimatorData 
{
    public static class Params
    {
        public static readonly int AttackParamHash = Animator.StringToHash("Attack");
        public static readonly int JumpParamHash = Animator.StringToHash("Jump");
        public static readonly int RunParamHash = Animator.StringToHash("isRunning");
        public static readonly int MoveParamHash = Animator.StringToHash("isMoving");
        public static readonly int OnGroundParamHash = Animator.StringToHash("OnGround");
        public static readonly int HurtParamHash = Animator.StringToHash("IsAttacked");
        public static readonly int DieParamHash = Animator.StringToHash("Died");
    }
}