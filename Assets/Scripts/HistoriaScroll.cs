using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoriaScroll : MonoBehaviour
{
    public RectTransform texto;   // RectTransform del texto que se desplazará.
    public float velocidad = 50f; // Velocidad de movimiento del texto hacia arriba.
    public float limiteSuperior = 1000f; // Posición a la que cambia de escena.

    public Button botonSaltar; // Botón para saltar la intro.

    void Start()
    {
        // Si el botón existe, asignar la acción de salto.
        if (botonSaltar != null)
        {
            botonSaltar.onClick.AddListener(SaltarIntro);
        }
    }

    void Update()
    {
        // Mover el texto hacia arriba constantemente.
        texto.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

        // Al llegar al límite configurado, pasar a la siguiente escena.
        if (texto.anchoredPosition.y >= limiteSuperior)
        {
            CargarSiguienteEscena();
        }
    }

    // Carga la siguiente escena en el Build Settings.
    void CargarSiguienteEscena()
    {
        int indexActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indexActual + 1);
    }

    // Ejecutado al presionar el botón de "Saltar".
    public void SaltarIntro()
    {
        CargarSiguienteEscena();
    }
}
