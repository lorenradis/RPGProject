using UnityEngine;

public class OverworldItemPickup : MonoBehaviour
{
    [SerializeField]
    private Item itemPrefab;
    public Item item;

    private void Start()
    {
        item = Instantiate(itemPrefab);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        if (GameManager.instance.inventory.AddItemToList(item))
        {
            Destroy(gameObject);
        }
        else
        {

        }
    }
}