using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance = null;

    [SerializeField]
    private ChestInfo[] sourceChestInfos;

    private List<ChestInfo> chestInfos = new List<ChestInfo>();

    private void Start()
    {
        Debug.Log("Cloning the chests");
        for (int i = 0; i < sourceChestInfos.Length; i++)
        {
            chestInfos.Add(Instantiate(sourceChestInfos[i]));
        }
    }

    public ChestInfo GetChestInfoByCode(int uniqueID)
    {
        Debug.Log("finding a chest with the code " + uniqueID);
        for (int i = 0; i < chestInfos.Count; i++)
        {
            if(chestInfos[i].uniqueCode == uniqueID)
            {
                return chestInfos[i];
            }
        }
        Debug.Log("Chest not found");
        return null;
    }
}
