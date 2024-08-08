using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemAmountText;

    private ItemDescriptionPanel descriptionPanel;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        iconGO = transform.GetChild(0).GetChild(0).gameObject;
        itemAmountText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        descriptionPanel = FindObjectOfType<ItemDescriptionPanel>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryManager.IsOpen)
        {
            if (!isEmpty)
            {
                descriptionPanel.ShowDescription(item.itemName, item.itemDescription);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryManager.IsOpen && descriptionPanel != null)
        {
            descriptionPanel.HideDescription();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryManager.SelectSlot(this);
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
        isEmpty = false;
    }

    public void ClearSlot()
    {
        item = null;
        amount = 0;
        isEmpty = true;
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        itemAmountText.text = "";
    }

    public void UpdateSlotUI()
    {
        if (isEmpty)
        {
            iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            itemAmountText.text = "";
        }
        else
        {
            iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            iconGO.GetComponent<Image>().sprite = item.icon;
            itemAmountText.text = amount.ToString();
        }
    }

    public void SetSlotSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
}
