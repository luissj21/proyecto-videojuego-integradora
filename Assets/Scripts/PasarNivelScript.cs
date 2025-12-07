using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarNivelScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo avanza si el objeto que entra es el jugador
        if (!collision.CompareTag("Rambo")) return;

        // Obtiene el índice de la escena actual
        int current = SceneManager.GetActiveScene().buildIndex;

        // Si es el último nivel, regresa al menú inicial
        if (current == 4)   // Ajustar según Build Settings
        {
            SceneManager.LoadScene("MenuInicial");
        }
        else
        {
            // Si no es el último nivel, avanza al siguiente
            SceneManager.LoadScene(current + 1);
        }
    }
}







