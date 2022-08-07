using UnityEngine;
[System.Serializable]
public class CombatTurn
{

    public UnitInfo actor;
    public UnitInfo target;

    public CombatAction combatAction;

    public bool didHit = false;
    public bool statusHit = false;
    public bool didCrit = false;

    public int damageAmount;
    public int healthRecoveryAmount;
    public int magicRecoveryAmount;
    public int hitRoll;
    public int critRoll;
    public int statusRoll;

    public CombatTurn(UnitInfo _actor, UnitInfo _target, CombatAction _combatAction)
    {
        actor = _actor;
        target = _target;
        combatAction = _combatAction;
    }

    public void CalculateTurn()
    {
        //this method ONLY sets values for all potential effects of the action used

        hitRoll = Random.Range(1, 100);
        if (hitRoll < (combatAction.accuracy * actor.Accuracy) / 100)
        {
            didHit = true;
        }
        else
        {
            didHit = false;
        }
        critRoll = Random.Range(1, 255);
        didCrit = critRoll < actor.Heart.Value;
        if (didHit)
        {
            if (combatAction.attackPower > 0) //if this is an attack
            {
                damageAmount = CalculateDamage();
            }

            if (combatAction.healthRecoveryPower > 0) //if the player recovers health
            {
                healthRecoveryAmount = CalculateRecovery();
            }

            healthRecoveryAmount += combatAction.healthRecoveryAmount;

            if(combatAction.magicRecoveryPower >0)
            {
                magicRecoveryAmount = CalculateRecovery();
            }

            magicRecoveryAmount += combatAction.magicRecoveryAmount;

            if (combatAction.statusToApply != CombatAction.StatusEffect.NONE)
            {
                statusRoll = Random.Range(1, 100);
                if (statusRoll < combatAction.statusChance)
                {
                    statusHit = true;
                }
            }
        }
    }

    private int CalculateDamage()
    {
        int offense = 1;
        int defense = 1;
        if (combatAction.attackType == CombatAction.AttackType.PHYSICAL)
        {
            offense = actor.Strength.Value + combatAction.attackPower;
            defense = target.Defense.Value;
        }
        else
        {
            offense = actor.Intelligence.Value + combatAction.attackPower;
            defense = target.Wisdom.Value;
        }
        float baseDamage;
        if(didCrit)
        {
            baseDamage = Mathf.Clamp(offense / 2f, 1f, 9999f);
        }
        else
        {
            baseDamage = Mathf.Clamp((offense / 2f - defense / 4f), 1f, 9999f);
        }

        return Mathf.FloorToInt(baseDamage + DamageRange(baseDamage) + RandomHit());
    }

    private int CalculateRecovery()
    {
        return Mathf.FloorToInt(damageAmount * .5f + DamageRange(damageAmount * .5f) + RandomHit());
    }

    private float DamageRange(float baseDamage)
    {
        return baseDamage * Random.Range(-.0625f, .0625f);
    }

    private int RandomHit()
    {
        return Random.Range(0, 2);
    }
}