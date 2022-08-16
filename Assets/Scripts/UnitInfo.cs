using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu]
public class UnitInfo : ScriptableObject
{

    public DepletableStat HP = new DepletableStat("HP");
    public DepletableStat MP = new DepletableStat("MP");
    public Stat Strength = new Stat("Strength", 10, 1);
    public Stat Defense = new Stat("Defense", 10, 1);
    public Stat Intelligence = new Stat("Intelligence", 10, 1);
    public Stat Wisdom = new Stat("Wisdom", 10, 1);
    public Stat Speed = new Stat("Speed", 10, 1);
    public Stat Heart = new Stat("Heart", 10, 1);
    public Stat Constitution = new Stat("Constitution", 10, 1);
    private List<Stat> stats = new List<Stat>();
    [SerializeField]
    private int accuracy = 100;
    public int Accuracy { get { return accuracy; }set { } }

    public CombatAction.StatusEffect currentStatus = CombatAction.StatusEffect.NONE;

    public string unitName;

    public Sprite icon;
    public Sprite portrait;
    public RuntimeAnimatorController overworldAnimator;
    public RuntimeAnimatorController battleAnimator;

    public int numberKilled = 0;

    public Weapon weapon;

    [SerializeField]
    private List<AbilityUnlock> abilityUnlocks = new List<AbilityUnlock>();

    public List<CombatAction> availableSpells = new List<CombatAction>();

    public string description;

    public int experience;
    public int gold;
    public int level = 1;

    public int moveMod;

    public bool isBoss;

    public int nextLevel
    {
        get
        {
            return (int)(level * level * level);
        }
        set
        {

        }
    }

    public void Setup()
    {
        stats.Add(Strength);
        stats.Add(Defense);
        stats.Add(Intelligence);
        stats.Add(Wisdom);
        stats.Add(Constitution);
        stats.Add(Speed);
        stats.Add(Heart);

        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].SetLevel(level);
        }

        HP.CalculateMax(Constitution.Value);
        MP.CalculateMax(Intelligence.Value);

    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        ResetStatsForLevel();
        ResetAbilitiesForLevel();
    }

    public void ResetStatsForLevel()
    {
        foreach(Stat stat in stats)
        {
            stat.SetLevel(level);
            int value = stat.Value;
        }
        HP.CalculateMax(Constitution.Value);
        MP.CalculateMax(Intelligence.Value);
    }

    private void ResetAbilitiesForLevel()
    {
        availableSpells.Clear();
        for (int i = 0; i < abilityUnlocks.Count; i++)
        {
            if(level >= abilityUnlocks[i].levelToLearn)
            {
                availableSpells.Add(abilityUnlocks[i].abilityToLearn);
            }
        }
    }

    private void LevelUp()
    {
        level++;
        SetLevel(level);
        if(experience > nextLevel)
        {
            LevelUp();
        }
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if(experience > nextLevel)
        {
            LevelUp();
        }
    }

    public bool LearnedAbility(int _level)
    {
        for (int i = 0; i < abilityUnlocks.Count; i++)
        {
            if (abilityUnlocks[i].levelToLearn == _level)
                return true;
        }
        return false;
    }

    public CombatAction GetAbilityAtLevel(int _level)
    {
        for (int i = 0; i < abilityUnlocks.Count; i++)
        {
            if (abilityUnlocks[i].levelToLearn == _level)
                return abilityUnlocks[i].abilityToLearn;
        }
        return null;
    }

    public void GainGold(int amount)
    {
        gold += amount;
    }

    public void SpendGold(int amount)
    {
        gold -= amount;
    }

    public void TakeDamage(int amount)
    {
        HP.Decrease(amount);
    }

    public void RecoverHealth(int amount)
    {
        HP.Increase(amount);
    }

    public void UseMP(int amount)
    {
        MP.Decrease(amount);
    }

    public void RecoverMP(int amount)
    {
        MP.Increase(amount);
    }
}

[System.Serializable]
public struct AbilityUnlock
{
    public int levelToLearn;
    public CombatAction abilityToLearn;
}