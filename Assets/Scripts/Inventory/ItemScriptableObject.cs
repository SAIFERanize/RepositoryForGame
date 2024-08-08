using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Food, Tool, Weapom, Resources }
public class ItemScriptableObject : ScriptableObject
{
    public int itemID;
    public GameObject itemPrefab;
    public ItemType itemType;
    public string itemName;
    public int maximumAmount;
    public string itemDescription;
    public Sprite icon;
    public bool isConsumeable;

    [Header("Consumeable Characterists")]
    public float changeHealht;
}
