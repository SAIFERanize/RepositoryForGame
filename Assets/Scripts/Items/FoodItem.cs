using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Food item", menuName ="Inventory/Item/New Food Item")]
public class FoodItem : ItemScriptableObject
{
    public float HealAmount;

    private void OnEnable()
    {
        itemType = ItemType.Food;
        isConsumeable = true;
        changeHealht = HealAmount;
    }
}
