using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Attack Data")]
public class NightAttackData : ScriptableObject
{
   public enum AttackType { Attack1,Attack2,Attack3 }
    public AttackType attackType;
    public float cooldown;
}
