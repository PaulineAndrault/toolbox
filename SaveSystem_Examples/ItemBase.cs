using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Inventory System/Item")]
public class ItemBase : ScriptableObject
{
    // Variables
    public string Name;
    public GameObject PrefabItem;
    [TextArea(5, 15)]
    public string Description;

    public ObjectType Type;
    [Range(1, 100)] public int Level = 1;
    public Worlds LootWorld;
    public Sprite[] Sprites = new Sprite[4];    // 4 sprites, for 4 scarcity levels

    [Header("Stat 1 - Quelles stats sont améliorées ?")]    // written in French for the French game designer. Meaning : "Which statistic is improved ?".
    public bool PV;     // Health points
    public bool ATT;    // Attack
    public bool BE;     // Bonus effect
    public bool PUI;    // Power
    public bool ARM;    // Armor
    public bool CD;     // Cooldown
    [Range(0,1)] public float StatModifier;

    [Header("Stat 2 - Bonus de rareté")]    // French for : Scarcity bonus
    public ObjectBonus BonusType;
    [Range(0, 3)] public int RaretyLevel = 0;
    public ItemBase NextItemRarety;

    [Header("Set d'objets")]
    public ObjectSetBonuses SetScript;
}
