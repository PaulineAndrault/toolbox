using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemBase[] Items;
    public Dictionary<ItemBase, int> GetId = new Dictionary<ItemBase, int>();       // We are using two Dictionary to easily find an item knowing its id or its ItemBase class
    public Dictionary<int, ItemBase> GetItem = new Dictionary<int, ItemBase>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<ItemBase, int>();
        GetItem = new Dictionary<int, ItemBase>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
