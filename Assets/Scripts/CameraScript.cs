using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Rambo;

    // Update is called once per frame
    void Update()
    {
        if (Rambo != null)
        {
            Vector3 position = transform.position;
            position.x = Rambo.transform.position.x;
            transform.position = position;
        }
    }
}
