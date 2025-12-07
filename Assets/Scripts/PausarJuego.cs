using UnityEngine;

public class PausarJuego : MonoBehaviour
{
    public GameObject menuPausa;   // Referencia al panel del menú de pausa
    public bool juegoPausado = false;  // Bandera para saber si el juego está pausado

    private void Update()
    {
        // Detecta cuando el jugador presiona ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Alterna entre pausar y reanudar
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        // Oculta el menú de pausa
        menuPausa.SetActive(false);

        // Reactiva el tiempo del juego
        Time.timeScale = 1;

        juegoPausado = false;
    }

    public void Pausar()
    {
        // Muestra el menú de pausa
        menuPausa.SetActive(true);

        // Detiene todo el tiempo del juego
        Time.timeScale = 0;

        juegoPausado = true;
    }
}





