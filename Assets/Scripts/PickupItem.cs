using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "prototype_power";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
            return;
        }

        if (other.GetComponent<NewMonoBehaviourScript>() != null)
        {
            Pickup();
            return;
        }
    }

    private void Pickup()
    {
        Debug.Log("Recogiste: " + itemName);
        InventoryManager.Instance.AddItem(itemName);
        Destroy(gameObject);
    }
}
