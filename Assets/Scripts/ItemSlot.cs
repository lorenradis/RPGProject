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

    public void SetItem(Item _item)
    {
        item = _item;
        icon = item.icon;
        image.sprite = icon;
        itemText.text = item.itemName + " " + item.quantity;
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