using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Res item", menuName = "Inventory/Item/New Res Item")]
public class ResourcesItem : ItemScriptableObject
{

    private void Start()
    {
        itemType = ItemType.Resources;
    }
}
