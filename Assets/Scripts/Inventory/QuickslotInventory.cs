using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    public Transform quickslotParent;
    public InventoryManager inventoryManager;
    public int currentQuickslotID = -1; // Начальное значение изменено на -1
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public player playerScript; // ссылка на скрипт игрока

    void Start()
    {
        // Инициализация слотов, установка спрайта для текущего слота, если он уже выбран
        if (currentQuickslotID >= 0 && currentQuickslotID < quickslotParent.childCount)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        }
    }

    void Update()
    {
        HandleNumberInput();
        HandleMouseClick();
    }

    private void HandleNumberInput()
    {
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                ToggleSelectSlot(i);
                break; // Прерываем цикл после обработки нажатия клавиши
            }
        }
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= quickslotParent.childCount) return;

        if (currentQuickslotID != -1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
        }
        currentQuickslotID = index;
        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
    }

    private void HandleMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentQuickslotID != -1)
        {
            InventorySlot selectedSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
            if (selectedSlot.item != null && selectedSlot.item.isConsumeable)
            {
                ConsumeItem(selectedSlot);
            }
        }
    }

    private void ConsumeItem(InventorySlot slot)
    {
        if (playerScript.Health >= 100)
        {
            Debug.Log("НЕ ХОЧУ.");
            return;
        }
        // Применяем изменения к здоровью
        playerScript.Heal(slot.item.changeHealht);

        // Уменьшаем количество предметов в слоте
        if (slot.amount <= 1)
        {
            slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData();
        }
        else
        {
            slot.amount--;
            slot.itemAmountText.text = slot.amount.ToString();
        }
    }

    private void ToggleSelectSlot(int index)
    {
        if (index < 0 || index >= quickslotParent.childCount) return;

        if (currentQuickslotID == index)
        {
            DeselectSlot();
        }
        else
        {
            SelectSlot(index);
        }
    }

    private void DeselectSlot()
    {
        if (currentQuickslotID == -1) return;

        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
        currentQuickslotID = -1;
    }
}
