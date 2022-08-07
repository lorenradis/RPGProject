using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;

    public bool isConsumable;

    public CombatAction combatAction = null;

    public int quantity;
    public int maxStackSize = 1;

    public int purchasePrice;
    public int sellPrice;

    public Sprite icon;

    public bool useInBattle;
    public bool useOutOfBattle;

    public string itemDescription;

    public void OnUseItem()
    {

        UnitInfo playerInfo = GameManager.instance.playerInfo;

        if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
        {
            if (!useInBattle)
            {
                return;
            }

        }
        else
        {
            if (!useOutOfBattle)
            {
                return ;
            }

        }

        int recoveryAmount = 0;

        if (combatAction.healthRecoveryAmount > 0)
        {
            if(playerInfo.HP.TotalCurrent < playerInfo.HP.TotalMax)
            {
                recoveryAmount = Mathf.Clamp(combatAction.healthRecoveryAmount, 0, (playerInfo.HP.TotalMax - playerInfo.HP.TotalCurrent));
                playerInfo.HP.Increase(combatAction.healthRecoveryAmount);
                if(GameManager.instance.gameState == GameManager.GameState.BATTLE)
                {

                }
                else
                {
                    DialogManager.instance.ShowSimpleDialog(playerInfo.unitName + " recovered " + recoveryAmount + " health!");
                }
            }
        }

        if(combatAction.magicRecoveryAmount > 0)
        {
            if (playerInfo.MP.TotalCurrent < playerInfo.MP.TotalMax)
            {
                recoveryAmount = Mathf.Clamp(combatAction.magicRecoveryAmount, 0, (playerInfo.MP.TotalMax - playerInfo.MP.TotalCurrent));
                playerInfo.MP.Increase(combatAction.magicRecoveryAmount);
                if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
                {

                }
                else
                {
                    DialogManager.instance.ShowSimpleDialog(playerInfo.unitName + " recovered " + recoveryAmount + " magic points!");
                }
            }
        }

        if (combatAction.statusToRecover != CombatAction.StatusEffect.NONE)
        {
            if(playerInfo.currentStatus == combatAction.statusToRecover)
            {
                playerInfo.currentStatus = CombatAction.StatusEffect.NONE;
            }
        }

        if (isConsumable)
        {
            quantity--;
            if (quantity < 1)
            {
                GameManager.instance.inventory.RemoveItemFromList(this);
            }
            if(GameManager.instance.gameState != GameManager.GameState.BATTLE)
                GameManager.instance.ShowInventory();
        }
    }

    public bool IsUsable()
    {
        UnitInfo player = GameManager.instance.playerInfo;

        if (!useInBattle && GameManager.instance.gameState == GameManager.GameState.BATTLE)
            return false;
        if (!useOutOfBattle && GameManager.instance.gameState != GameManager.GameState.BATTLE)
            return false;

        if (combatAction.healthRecoveryAmount > 0 && player.HP.TotalCurrent < player.HP.TotalMax)
            return true;
        if (combatAction.magicRecoveryAmount > 0 && player.MP.TotalCurrent < player.MP.TotalMax)
            return true;

        return false;
    }
}
