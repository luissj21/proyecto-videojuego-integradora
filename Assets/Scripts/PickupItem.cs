using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "prototype_power";  // Nombre del objeto que se recogerá

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si lo toca el jugador usando tag "Player"
        if (other.CompareTag("Player"))
        {
            Pickup();
            return;
        }

        // Si lo toca el jugador usando el script del player
        if (other.GetComponent<NewMonoBehaviourScript>() != null)
        {
            Pickup();
            return;
        }
    }

    private void Pickup()
    {
        // Debug para verificar el ítem recogido
        Debug.Log("Recogiste: " + itemName);

        // Agrega el objeto al inventario
        InventoryManager.Instance.AddItem(itemName);

        // Destruye el ítem del mapa
        Destroy(gameObject);
    }
}





