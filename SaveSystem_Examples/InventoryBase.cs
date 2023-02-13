using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string SavePath;
    private ItemDatabase _database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        _database = (ItemDatabase)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/ItemDatabase.asset", typeof(ItemDatabase));
#else
        _database = Resources.Load<ItemDatabase>("ItemDatabase");
#endif
    }

    #region Add / Remove items methods
    public void AddItem(ItemBase item, int amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if(Container[i].Item == item)
            {
                Container[i].AddAmount(amount);
                return;
            }
        }

        Container.Add(new InventorySlot(_database.GetId[item], item, amount));
    }

    public void RemoveItem(ItemBase item, int amount = 1)
    {
        // We verify if the item is in the inventory, if so, we remove it
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].Item == item)
            {
                Container[i].AddAmount(-1);
                if (Container[i].Amount == 0) Container.RemoveAt(i);
                return;
            }
        }
    }
    #endregion

    #region Save methods

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, SavePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    #endregion

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
            Container[i].Item = _database.GetItem[Container[i].ID];
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemBase Item;
    public int Amount;
    public InventorySlot(int id, ItemBase item, int amount)
    {
        ID = id;
        Item = item;
        Amount = amount;
    }
    public void AddAmount(int value)
    {
        Amount += value;
    }
}
