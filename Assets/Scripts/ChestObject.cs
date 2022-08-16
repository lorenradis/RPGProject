using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObject : Interactable
{

    [SerializeField]
    private int uniqueCode;
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    private Sprite openSprite;

    [SerializeField]
    ChestInfo chestInfo;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //instantiate the chest info so that we don't modify the copy living in the editor
        chestInfo = GameManager.instance.chestManager.GetChestInfoByCode(uniqueCode);
        spriteRenderer.sprite = chestInfo.isOpen ? openSprite : closedSprite;
    }

    public override void OnInteract(Transform player)
    {
        if (chestInfo.isOpen)
        {
            DialogManager.instance.ShowSimpleDialog("It's empty...");
        }
        else
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        DialogManager.instance.ShowSimpleDialog("You opened the chest...");
        if (chestInfo.item != null)
        {
            DialogManager.instance.ShowSimpleDialog("It contains a " + chestInfo.item.itemName + "!");
            if (GameManager.instance.inventory.HasRoom(chestInfo.item))
            {
                chestInfo.isOpen = true;
                DialogManager.instance.ShowSimpleDialog("You pocket the " + chestInfo.item.itemName + ", score!");
                GameManager.instance.inventory.AddItemToList(Instantiate(chestInfo.item));
                spriteRenderer.sprite = openSprite;
            }
            else
            {
                DialogManager.instance.ShowSimpleDialog("But you don't have enough room for it, so you leave it behind.");
            }
        }
        if(chestInfo.gold > 0)
        {
            DialogManager.instance.ShowSimpleDialog("What luck, you found " + chestInfo.gold + " gold coins!");
            GameManager.instance.playerInfo.GainGold(chestInfo.gold);
        }
    }
}
