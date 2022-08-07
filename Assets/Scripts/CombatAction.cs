using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class CombatAction : ScriptableObject
{

    public string actionName;

    public int attackPower = 0;
    public int healthRecoveryPower = 0;
    public int magicRecoveryPower = 0;
    public int healthRecoveryAmount = 0;
    public int magicRecoveryAmount = 0;
    public int accuracy = 100;

    public int statusChance = 0;

    public int critChance = 10;

    public int mpCost = 0;

    public enum AttackType { PHYSICAL, SPECIAL }
    public AttackType attackType;

    public enum StatusEffect { NONE, POISON, BURN, FREEZE, PETRIFY, SLEEP, CONFUSE, PARALYZE, ALL }
    public StatusEffect statusToApply;
    public StatusEffect statusToRecover;

    public string description;
}