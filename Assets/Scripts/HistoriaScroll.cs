using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoriaScroll : MonoBehaviour
{
    public RectTransform texto;
    public float velocidad = 50f;
    public float limiteSuperior = 1000f;

    public Button botonSaltar; // ← nuevo

    void Start()
    {
        // Opcional: asegurar que el botón esté conectado
        if (botonSaltar != null)
        {
            botonSaltar.onClick.AddListener(SaltarIntro);
        }
    }

    void Update()
    {
        // Movimiento automático del texto
        texto.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

        // Cuando llegue al límite, pasa a la siguiente escena
        if (texto.anchoredPosition.y >= limiteSuperior)
        {
            CargarSiguienteEscena();
        }
    }

    // 🔵 Función para cargar la siguiente escena
    void CargarSiguienteEscena()
    {
        int indexActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indexActual + 1);
    }

    // 🔵 Función que se llama cuando oprimes el botón
    public void SaltarIntro()
    {
        CargarSiguienteEscena();
    }
}



//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class HistoriaScroll : MonoBehaviour
//{
//    public RectTransform texto;
//    public float velocidad = 50f;
//    public float limiteSuperior = 1000f;

//    void Update()
//    {
//        texto.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

//        if (texto.anchoredPosition.y >= limiteSuperior)
//        {
//            // Cargar la siguiente escena según el índice del build
//            int indexActual = SceneManager.GetActiveScene().buildIndex;
//            SceneManager.LoadScene(indexActual + 1);
//        }
//    }
//}
