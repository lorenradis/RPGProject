using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillsSlot : MonoBehaviour
{
    public UnitInfo unitInfo;

    public Image icon;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI numberText;

    public void SetUnitInfo(UnitInfo newUnitInfo)
    {
        unitInfo = newUnitInfo;
        icon.sprite = unitInfo.icon;
        nameText.text = unitInfo.unitName;
        numberText.text = "Slain: " + unitInfo.numberKilled;
    }

    public void ShowUnitInfo()
    {
        if(unitInfo == null)
        {
            Debug.Log("No unit set");
            return;
        }
        UIManager.instance.ShowEnemyInfoMenu(unitInfo);
    }
}
