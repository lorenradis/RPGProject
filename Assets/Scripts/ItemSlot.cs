using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Item item;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private TextMeshProUGUI itemText;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI quantityText;

    private ShopkeeperInteraction shopKeeper;

    public void SetItem(Item _item)
    {
        item = _item;
        icon = item.icon;
        itemText.text = item.itemName;
        if (image != null)
        {
            image.sprite = icon;
        }
        if (priceText != null)
        {
            priceText.text = "$" + item.purchasePrice;
        }
        if (quantityText != null)
        {
            if(item.quantity > 1)
            {
                quantityText.text = "x" + item.quantity;
            }
            else
            {
                quantityText.text = "";
            }
        }
    }

    public void SetShopKeeper(ShopkeeperInteraction newShopKeeper)
    {
        shopKeeper = newShopKeeper;
    }

    public void EnableItemSlot()
    {

    }

    public void DisableItemSlot()
    {

    }

    public void SelectThisItem()
    {

    }

    public void BuyThisItem()
    {
        shopKeeper.AttemptPurchase(item);
    }

    public void SellThisItem()
    {

    }

    public void UseThisItem()
    {
        if (item.IsUsable())
        {
            if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
            {
                BattleManager.instance.UseItem(item);
            }
            else
            {
                item.OnUseItem();
            }
        }
        else
        {
            if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
            {
                BattleManager.instance.DisplayDialog("Can't use that right now!");
            }
            else
            {
                DialogManager.instance.ShowSimpleDialog("Can't use that right now");
            }
        }
    }
}