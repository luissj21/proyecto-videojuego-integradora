using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        // Si existe un inventario, lo reinicia al empezar nueva partida
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ResetInventoryForNewGame();

        // Carga la siguiente escena según el índice de build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        // Mensaje para debug, solo visible en editor
        Debug.Log("Salir...");

        // Cierra el juego cuando es ejecutable
        Application.Quit();
    }
}



