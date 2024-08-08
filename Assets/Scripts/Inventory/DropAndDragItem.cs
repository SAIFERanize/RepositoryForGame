using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    public InventorySlot oldSlot;
    private Transform player;
    private Transform originalParent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition
        );

        GetComponent<RectTransform>().localPosition = localPointerPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
        GetComponentInChildren<Image>().raycastTarget = false;

        // Сохраняем оригинальный родительский объект и перемещаем в корень Canvas
        originalParent = transform.parent;
        transform.SetParent(GameObject.Find("InventoryCanvas").transform);

        // Перемещение на передний план
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        GetComponentInChildren<Image>().raycastTarget = true;

        // Возвращаем объект в старый слот
        transform.SetParent(originalParent);
        transform.position = oldSlot.transform.position;

        if (eventData.pointerCurrentRaycast.gameObject.name == "UIPanel")
        {
            GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            itemObject.GetComponent<Item>().amount = oldSlot.amount;
            NullifySlotData();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        oldSlot.OnPointerClick(eventData);
    }

    public void NullifySlotData()
    {
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmountText.text = "";
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject tempItem = oldSlot.item;
        int tempAmount = oldSlot.amount;
        bool tempIsEmpty = oldSlot.isEmpty;

        oldSlot.item = newSlot.item;
        oldSlot.amount = newSlot.amount;
        oldSlot.isEmpty = newSlot.isEmpty;
        oldSlot.UpdateSlotUI();

        newSlot.item = tempItem;
        newSlot.amount = tempAmount;
        newSlot.isEmpty = tempIsEmpty;
        newSlot.UpdateSlotUI();
    }
}
