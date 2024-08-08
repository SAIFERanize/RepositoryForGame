using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject InventoryPanel;
    public Transform inventory;
    public Transform quickslotParent;
    public List<InventorySlot> Slots = new List<InventorySlot>();
    public List<InventorySlot> QuickSlots = new List<InventorySlot>();
    public bool IsOpen;
    private Camera mainCamera;
    public Button dropButton;
    private InventorySlot selectedSlot;
    public Image selectedItemIcon;
    public Sprite selectedSlotSprite;  // Спрайт для выбранного слота
    public Sprite unselectedSlotSprite;  // Спрайт для невыбранного слота

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InventoryPanel.SetActive(true);
    }

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventory.childCount; i++)
        {
            if (inventory.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                Slots.Add(inventory.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (quickslotParent.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                QuickSlots.Add(quickslotParent.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        InventoryPanel.SetActive(false);
        dropButton.onClick.AddListener(DropSelectedItem);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.I))
        {
            IsOpen = !IsOpen;
            InventoryPanel.SetActive(IsOpen);
            if (IsOpen)
            {
                InventoryPanel.transform.SetAsLastSibling();
            }
        }
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        if (!CanPickUpItem(_item, _amount))
        {
            Debug.Log("Инвентарь полон или нет подходящего слота, предмет не может быть подобран");
            return;
        }

        _amount = AddItemToSlots(_item, _amount, QuickSlots);
        if (_amount > 0)
        {
            _amount = AddItemToSlots(_item, _amount, Slots);
        }

        if (_amount > 0)
        {
            Debug.LogWarning($"Не все предметы были добавлены. Остаток: {_amount}");
        }

        OnInventoryChanged?.Invoke();
    }

    private int AddItemToSlots(ItemScriptableObject _item, int _amount, List<InventorySlot> slots)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item && slot.amount < _item.maximumAmount)
            {
                int spaceLeft = _item.maximumAmount - slot.amount;
                int amountToAdd = Mathf.Min(spaceLeft, _amount);
                slot.amount += amountToAdd;
                _amount -= amountToAdd;
                slot.UpdateSlotUI();
                if (_amount <= 0)
                {
                    return 0;
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                int amountToAdd = Mathf.Min(_item.maximumAmount, _amount);
                slot.item = _item;
                slot.amount = amountToAdd;
                _amount -= amountToAdd;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.UpdateSlotUI();
                if (_amount <= 0)
                {
                    return 0;
                }
            }
        }

        return _amount;
    }

    public bool IsInventoryFull()
    {
        foreach (InventorySlot slot in Slots)
        {
            if (slot.isEmpty || (slot.item != null && slot.amount < slot.item.maximumAmount))
            {
                return false;
            }
        }
        foreach (InventorySlot slot in QuickSlots)
        {
            if (slot.isEmpty || (slot.item != null && slot.amount < slot.item.maximumAmount))
            {
                return false;
            }
        }
        return true;
    }

    public bool CanPickUpItem(ItemScriptableObject item, int amount)
    {
        int remainingAmount = amount;
        foreach (InventorySlot slot in QuickSlots)
        {
            if (slot.isEmpty)
            {
                remainingAmount -= item.maximumAmount;
            }
            else if (slot.item == item && slot.amount < item.maximumAmount)
            {
                remainingAmount -= (item.maximumAmount - slot.amount);
            }

            if (remainingAmount <= 0)
            {
                return true;
            }
        }

        foreach (InventorySlot slot in Slots)
        {
            if (slot.isEmpty)
            {
                remainingAmount -= item.maximumAmount;
            }
            else if (slot.item == item && slot.amount < item.maximumAmount)
            {
                remainingAmount -= (item.maximumAmount - slot.amount);
            }

            if (remainingAmount <= 0)
            {
                return true;
            }
        }

        return false;
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (selectedSlot == slot)
        {
            // Если слот уже выбран, отменяем выбор
            DeselectSlot();
            return;
        }

        // Деселектим предыдущий выбранный слот, если есть
        if (selectedSlot != null)
        {
            selectedSlot.SetSlotSprite(unselectedSlotSprite);
        }

        selectedSlot = slot;
        selectedSlot.SetSlotSprite(selectedSlotSprite);
        Debug.Log($"Выбран слот с предметом");
    }

    public void DeselectSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetSlotSprite(unselectedSlotSprite);
            selectedSlot = null;
            Debug.Log("Слот деселекционирован");
        }
    }

    private void DropSelectedItem()
    {
        if (selectedSlot != null && !selectedSlot.isEmpty)
        {
            // Создаем предмет в мире
            Vector3 dropPosition = mainCamera.transform.position + mainCamera.transform.forward * 2;
            Instantiate(selectedSlot.item.itemPrefab, dropPosition, Quaternion.identity);
            // Убираем предмет из инвентаря
            selectedSlot.ClearSlot();
            DeselectSlot();
        }
    }
}
