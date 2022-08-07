using UnityEngine;

[System.Serializable]
public class DepletableStat 
{
    public string statName;

    public AudioClip recoverHealthSound;

    private int modifier = 0;
    private float multiplier = 1f;
    [SerializeField]
    private int currentBase;
    [SerializeField]
    private int maxBase;
    [SerializeField]
    private int totalCurrent;
    public int TotalCurrent { get {
            totalCurrent = Mathf.FloorToInt(currentBase * multiplier + modifier);
            return totalCurrent;
        } set { } }
    private int totalMax;
    public int TotalMax { get {
            totalMax = Mathf.FloorToInt(maxBase * multiplier + modifier);
            return totalMax;
        }set { } }

    public DepletableStat()
    {

    }

    public DepletableStat(string newName) { 
        statName = newName;

        modifier = 0;
        multiplier = 1f;
    }

    public void CalculateMax(int sourceAmount)
    {
        maxBase = Mathf.CeilToInt(Random.Range(1.95f, 2.05f) * sourceAmount * 2f);
        currentBase = maxBase;
        Reconcile();
    }

    public void Increase(int amount)
    {
        currentBase += amount;
        GameManager.instance.audioManager.PlaySoundEffect(recoverHealthSound);
        Reconcile();
    }

    public void Decrease(int amount)
    {
        currentBase -= amount;
        Reconcile();
    }

    private void Reconcile()
    {
        currentBase = Mathf.Clamp(currentBase, 0, maxBase);
        int dummy = TotalCurrent;
        dummy = TotalMax;
    }

    public bool IsDepleted()
    {
        return totalCurrent <= 0;
    }
}