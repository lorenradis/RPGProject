using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Inventory
{
    public List<Item> items = new List<Item>();

    private int maxItems = 48;

    public bool HasRoom(Item item)
    {
        if (items.Count < maxItems)
        {
            return true;
        }
        if (item.maxStackSize > 1)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == item.itemName && items[i].quantity < item.maxStackSize + item.quantity)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddItemToList(Item item)
    {
        if (item.maxStackSize > 1)
        {
            bool contains = false;
            Item itemToAddTo = null;
            for (int i = 0; i < items.Count; i++)
            {
                if (item.itemName == items[i].itemName)
                {
                    contains = true;
                    itemToAddTo = items[i];
                    break;
                }
            }
            if (contains)
            {
                if (itemToAddTo.quantity < itemToAddTo.maxStackSize)
                {
                    int roomLeft = itemToAddTo.maxStackSize - itemToAddTo.quantity;
                    if (item.quantity <= roomLeft)
                    {
                        itemToAddTo.quantity += item.quantity;
                        return true;
                    }
                    else
                    {
                        item.quantity -= roomLeft;
                        itemToAddTo.quantity += roomLeft;
                        return false;
                    }
                }
            }
        }

        if (items.Count < maxItems)
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItemFromList(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }

    }
}