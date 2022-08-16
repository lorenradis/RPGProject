using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperInteraction : Interactable
{
    public string shopkeeperName;
    public Sprite shopkeeperPortrait;

    public Dialog greetingDialog; //hi, how can i help you? (buy, sell)
    public Dialog whatToBuyDialog; //what would you like to buy? (confirm, cancel)
    public Dialog whatToSellDialog; //what would you like to sell? (confirm, cancel)
    public Dialog confirmSellDialog; //i'll buy it for x is that ok? (yes, no)
    public Dialog confirmPurchaseDialog; //you can have it for x is that ok? (yes, no)
    public Dialog successSellDialog; //thank you!
    public Dialog successPurchaseDialog; //thank you!
    public Dialog cancelDialog; //understood
    public Dialog anythingElseDialog; //anything else? (buy, sell)
    public Dialog notEnoughMoneyDialog;
    public Dialog farewellDialog;

    public List<Item> items = new List<Item>();

    public override void OnInteract(Transform player)
    {
        base.OnInteract(player);
        ShowGreeting();
    }

    private void ShowGreeting()
    {
        DialogManager.instance.ShowDialog(greetingDialog, () => {
            ShowSellerInventory();
        }, () => {
            ShowPlayerInventory();
        }, () => {
            ShowFarewellDialog();
        });
    }

    private void ShowSellerInventory()
    {
        GameManager.instance.ShowShopMenu(this);
    }

    private void ShowPlayerInventory()
    {
        UIManager.instance.ShowInventory();
    }

    public void AttemptPurchase(Item item)
    {
        DialogManager.instance.ShowDialog(confirmPurchaseDialog, () => {
            if(GameManager.instance.playerInfo.gold >= item.purchasePrice)
            {
                GameManager.instance.playerInfo.SpendGold(item.purchasePrice);
                GameManager.instance.inventory.AddItemToList(Instantiate(item));
                DialogManager.instance.ShowDialog(successPurchaseDialog);
                GameManager.instance.HideShopMenu();
                ShowAnythingElse();
            }
            else
            {
                DialogManager.instance.ShowDialog(notEnoughMoneyDialog);
                GameManager.instance.HideShopMenu();
                ShowAnythingElse();
            }
        }, () => {
            GameManager.instance.HideShopMenu();
            ShowAnythingElse();
        });
    }

    private void ShowAnythingElse()
    {
        DialogManager.instance.ShowDialog(anythingElseDialog, () => {
            ShowSellerInventory();
        }, () => {
            ShowPlayerInventory();
        }, () => {
            ShowFarewellDialog();
        });
    }

    public void AttemptSell(Item item)
    {

    }

    private void ShowFarewellDialog()
    {
        DialogManager.instance.ShowDialog(farewellDialog);
    }
}