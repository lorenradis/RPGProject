using UnityEngine;

[System.Serializable]
public class Stat
{
    public string statName;
    [SerializeField]
    protected int baseValue;
    protected int value;
    protected int exp;
    protected int level;
    protected int modifier = 0;
    protected int permModifier = 0;
    protected float multiplier = 1f;

    public int Value
    {
        get
        {
            //Debug.Log(statName + " is " +  Mathf.FloorToInt(.01f * (2 * baseValue + exp) * level) + 5);
            value = Mathf.FloorToInt((baseValue * (level - 1)) / 10 + 2);
            return value;
        }
        set { }
    }

    public Stat()
    {
        statName = "";
        baseValue = 10;
        exp = 0;
        level = 1;
        modifier = 0;
        multiplier = 1f;
    }

    public Stat(string newName, int newBase, int newLevel)
    {
        statName = newName;
        baseValue = newBase;
        level = newLevel;
        exp = 0;
        modifier = 0;
        multiplier = 1f;
        int value = Value;
    }

    public void SetLevel(int _level)
    {
        level = _level;
    }

    public void SetModifier(int amount)
    {
        modifier = amount;
    }

    public void AddPermanentModifier(int amount)
    {
        permModifier += amount;
    }

    public void SetMultiplier(float newMult)
    {
        multiplier = newMult;
    }

}

/*
 * 
 *base stats should be d&d like, 12 to 18 ish with exceptions

formula for current value is :  (baseValue * (level - 1))/10 + 2

stats are

str
def
int
wis
con
speed
luck

hp - 195 to 205 % of con
mp - 195 to 205% of int

accuracy - 100 for most everything  
 */