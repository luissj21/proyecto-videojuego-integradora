using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    // Singleton para acceder al inventario desde cualquier script.

    // Diccionario que almacena itemName -> cantidad
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        // Configura el Singleton y evita duplicados entre escenas.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena.
        }
        else
        {
            Destroy(gameObject); // Si ya existe un Instance, eliminar este.
        }
    }

    // Añade 1 al item indicado
    public void AddItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return;

        // Si el item no existe aún, iniciarlo en 0.
        if (!items.ContainsKey(itemName))
            items[itemName] = 0;

        items[itemName]++; // Incrementar cantidad.

        // Actualizar UI si existe.
        if (UIInventory.Instance != null)
            UIInventory.Instance.UpdateItem(itemName, items[itemName]);
    }

    // Obtener cantidad de un item
    public int GetItemCount(string itemName)
    {
        if (items.TryGetValue(itemName, out int count)) return count;
        return 0; // Si no existe, devuelve 0.
    }

    // Devuelve una copia del inventario actual.
    public Dictionary<string, int> GetAllItemsSnapshot()
    {
        return new Dictionary<string, int>(items);
    }

    // Elimina todos los items (opcional).
    public void ClearAll()
    {
        items.Clear();
        if (UIInventory.Instance != null)
            UIInventory.Instance.ClearUI(); // Limpia la UI.
    }

    public void ResetInventoryForNewGame()
    {
        items.Clear(); // Reiniciar inventario interno.

        // limpiar la UI si existe
        if (UIInventory.Instance != null)
            UIInventory.Instance.ClearUI();

        Debug.Log("InventoryManager: Inventario reiniciado para nueva partida.");
    }
}
