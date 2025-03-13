using UnityEngine;

public static class EnemyAnimatorData
{
    public static class Params
    {
        public static readonly int MoveParamHash = Animator.StringToHash("isMoving");
        public static readonly int DieParamHash = Animator.StringToHash("Die");
        public static readonly int AttackParamHash = Animator.StringToHash("Attack");
        public static readonly int HurtParamHash = Animator.StringToHash("IsAttacked");
    }
}