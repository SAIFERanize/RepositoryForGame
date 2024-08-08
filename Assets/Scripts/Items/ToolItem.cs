using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tool item", menuName = "Inventory/Item/New Tool Item")]
public class ToolItem : ItemScriptableObject
{

    private void Start()
    {
        itemType = ItemType.Tool;
    }
}
