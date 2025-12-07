using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;                   // Referencia al fill de la barra de vida (UI)
    private NewMonoBehaviourScript playerController; // Controlador del jugador para acceder a su salud
    private float vidaMaxima;                        // Vida máxima del jugador, usada para calcular porcentaje

    void Start()
    {
        // Obtiene el script del jugador (Rambo) para leer su vida
        playerController = GameObject.Find("Rambo").GetComponent<NewMonoBehaviourScript>();

        // Guarda la vida máxima al iniciar el juego
        vidaMaxima = playerController.Health;
    }

    void Update()
    {
        // Actualiza el fill de la barra en base al porcentaje de vida actual
        rellenoBarraVida.fillAmount = playerController.Health / vidaMaxima;
    }
}





