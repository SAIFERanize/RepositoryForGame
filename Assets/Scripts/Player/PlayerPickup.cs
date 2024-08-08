using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private List<Item> itemsInRange = new List<Item>();
    private bool canPickUp = true;

    private void OnEnable()
    {
        InventoryManager.Instance.OnInventoryChanged += UpdateCanPickUp;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnInventoryChanged -= UpdateCanPickUp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item != null)
        {
            itemsInRange.Add(item);
            UpdateCanPickUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item != null)
        {
            itemsInRange.Remove(item);
            UpdateCanPickUp();
        }
    }

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E) && itemsInRange.Count > 0)
        {
            var item = itemsInRange[0];
            if (InventoryManager.Instance.CanPickUpItem(item.itemScriptableObject, item.amount))
            {
                InventoryManager.Instance.AddItem(item.itemScriptableObject, item.amount);
                itemsInRange.RemoveAt(0);
                Destroy(item.gameObject); // Удаляет предмет из сцены только если он был добавлен в инвентарь
            }
            UpdateCanPickUp(); // Обновляем статус возможности подбирать предметы
        }
    }

    void UpdateCanPickUp()
    {
        canPickUp = false;
        foreach (var item in itemsInRange)
        {
            if (InventoryManager.Instance.CanPickUpItem(item.itemScriptableObject, item.amount))
            {
                canPickUp = true;
                break;
            }
        }
        Debug.Log($"Могу подбирать предметы: {canPickUp}");
    }
}
