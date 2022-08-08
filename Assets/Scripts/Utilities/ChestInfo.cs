using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChestInfo : ScriptableObject
{
    public Item item;
    public int gold;
    public bool isOpen;
    public int uniqueCode; //0 + region code + chestNumber i.e. first chest in the town is 00201, twelfth chest in the forest if 00412
}