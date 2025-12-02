using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Diccionario que almacena itemName -> cantidad
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Añade 1 al item indicado
    public void AddItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return;

        if (!items.ContainsKey(itemName))
            items[itemName] = 0;

        items[itemName]++;

        if (UIInventory.Instance != null)
            UIInventory.Instance.UpdateItem(itemName, items[itemName]);
    }

    // Obtener cantidad de un item
    public int GetItemCount(string itemName)
    {
        if (items.TryGetValue(itemName, out int count)) return count;
        return 0;
    }

    // Devuelve una copia del diccionario actual (snapshot)
    public Dictionary<string, int> GetAllItemsSnapshot()
    {
        return new Dictionary<string, int>(items);
    }

    // (Opcional) método para resetear inventario
    public void ClearAll()
    {
        items.Clear();
        if (UIInventory.Instance != null)
            UIInventory.Instance.ClearUI();
    }

    public void ResetInventoryForNewGame()
    {
        items.Clear();

        // limpiar la UI si existe
        if (UIInventory.Instance != null)
            UIInventory.Instance.ClearUI();

        Debug.Log("InventoryManager: Inventario reiniciado para nueva partida.");
    }

}


//using UnityEngine;

//public class InventoryManager : MonoBehaviour
//{
//    public static InventoryManager Instance;

//    public int itemCount = 0;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // Se mantiene entre escenas
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void AddItem(string itemName)
//    {
//        itemCount++;
//        UIInventory.Instance.UpdateItem(itemName, itemCount);
//    }
//}
