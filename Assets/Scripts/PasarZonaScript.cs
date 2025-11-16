using UnityEngine;

public class PasarZonaScript : MonoBehaviour
{
    public Transform nuevaPosicionJugador;   // Posición donde aparecerá el jugador
    public Transform nuevaPosicionCamara;    // Posición donde se moverá la cámara
    public float velocidadCamara = 2f;       // Velocidad del movimiento de cámara
    private bool moviendoCamara = false;     // Controla el desplazamiento

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rambo"))
        {
            // Mueve al jugador instantáneamente
            collision.transform.position = nuevaPosicionJugador.position;

            // Inicia la transición suave de cámara
            Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(MoverCamara());
        }
    }

    private System.Collections.IEnumerator MoverCamara()
    {
        moviendoCamara = true;

        Vector3 destino = nuevaPosicionCamara.position;

        // Movimiento suave de la cámara hacia el nuevo punto
        while (Vector3.Distance(Camera.main.transform.position, destino) > 0.05f)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                destino,
                Time.deltaTime * velocidadCamara
            );
            yield return null;
        }

        Camera.main.transform.position = destino;
        moviendoCamara = false;
    }
}
