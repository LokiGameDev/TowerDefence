using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    Dictionary<int, int> items = new Dictionary<int, int>();

    [SerializeField]
    private ObjectsDatabaseSO database;

    public void AddItem(int id, int qty)
    {
        if (items.ContainsKey(id))
        {
            items[id] += qty;
        }
        else
        {
            items.Add(id, qty);
        }
        UIManager.Instance.UpdateInvetory(id, items[id]);
    }

    public bool RemoveItem(int id, int qty)
    {
        if (items.ContainsKey(id) && items[id] >= qty)
        {
            items[id] -= qty;
            if (items[id] == 0)
            {
                items.Remove(id);
            }
            UIManager.Instance.UpdateInvetory(id, items.ContainsKey(id) ? items[id] : 0);
            return true;
        }
        UIManager.Instance.UpdateInvetory(id, items.ContainsKey(id) ? items[id] : 0);
        return false;
    }

    public int GetItemCount(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        return 0;
    }
    
    public Dictionary<int, int> GetAllItems()
    {
        return new Dictionary<int, int>(items);
    }

    public void LoadItems(Dictionary<int, int> loadedItems)
    {
        items = new Dictionary<int, int>(loadedItems);
        foreach (var item in items)
        {
            UIManager.Instance.UpdateInvetory(item.Key, item.Value);
        }
    }
}