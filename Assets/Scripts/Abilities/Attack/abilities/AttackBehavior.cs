using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior: Ability
{
    public float cooldownTimer = 0f;
    public float attackTimer = 0f;
    public float pogoTimer = 0f;
    public float selfKnockbackStrengthTimer = 0f;
    public AttackConfig config;
    public HashSet<int> attackablesHitThisAttack = new HashSet<int>();
}