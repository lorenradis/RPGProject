using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperInteraction : Interactable
{
    public Dialog promptDialog;
    public Dialog buySellDialog;
    public Dialog whatToSellDialog;
    public Dialog whatToBuyDialog;
    public Dialog confirmSellDialog;
    public Dialog successDialog;
    public Dialog confirmBuyDialog;
    public Dialog noMoneyDialog;
    public Dialog continueDialog;
    public Dialog endInteractionDialog;
    /*
    public override void OnInteract(Transform player)
    {
        ShowShopPrompt();
        base.OnInteract(player);
    }

    private void ShowShopPrompt()
    {
        DialogManager.instance.ShowDialog(promptDialog, () =>
        {
            AskBuyOrSell();
        }, () =>
        {
            DialogManager.instance.ShowDialog(endInteractionDialog);
        });
    }

    private void AskBuyOrSell()
    {
        DialogManager.instance.ShowDialog(buySellDialog, () =>
        {
            ShowShopInventory();
        }, () =>
        {
            ShowSellableInventory();
        });
    }

    private void ShowSellableInventory()
    {
        GameManager.instance.uiManager.DisplayInventory();
        GameManager.instance.inventory.EnterSellMode();
        DialogManager.instance.ShowDialog(whatToSellDialog, () =>
        {
            ConfirmSale();
        }, () =>
        {
            GameManager.instance.uiManager.HideInventory();
            GameManager.instance.inventory.EnterNormalMode();
        });
    }

    private void ShowShopInventory()
    {
        Debug.Log("Showing the shop");
    }

    private void ConfirmSale()
    {
        if(GameManager.instance.inventory.selectedItems.Count > 0)
        {
            DialogManager.instance.ShowDialog(confirmSellDialog, () =>
            {
                GameManager.instance.inventory.SellSelectedItems();
                DialogManager.instance.ShowDialog(successDialog);
                GameManager.instance.inventory.EnterNormalMode();
                GameManager.instance.uiManager.HideInventory();
                DialogManager.instance.ShowDialog(continueDialog, () =>
                {
                    AskBuyOrSell();
                }, () =>
                {
                    DialogManager.instance.ShowDialog(endInteractionDialog);
                });
            }, () =>
            {

            });
        }
        else
        {
            ShowSellableInventory();
        }
    }

    private void ConfirmPurchase()
    {

    }
    */
}