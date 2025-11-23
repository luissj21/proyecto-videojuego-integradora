using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarNivelScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Rambo")) return;

        int current = SceneManager.GetActiveScene().buildIndex;

        // Si estás en el último nivel (Nivel 3)
        if (current == 3)   // <-- Ajusta al index correcto en tu Build Settings
        {
            SceneManager.LoadScene("MenuInicial");
        }
        else
        {
            SceneManager.LoadScene(current + 1);
        }
    }
}



//using UnityEngine;
//using UnityEngine.SceneManagement;
//public class PasarNivelScript : MonoBehaviour
//{

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Rambo"))
//        {
//            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//        }
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
