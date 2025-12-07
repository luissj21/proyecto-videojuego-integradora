using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Rambo; // Jugador a seguir

    void Update()
    {
        if (Rambo != null)
        {
            // Mantener la altura y posición Z, solo seguir en eje X
            Vector3 position = transform.position;
            position.x = Rambo.transform.position.x;
            transform.position = position;
        }
    }
}
