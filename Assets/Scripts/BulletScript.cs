using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;          // Velocidad de la bala
    private Rigidbody2D Rigidbody2D;   // Físicas
    private Vector2 Direction;         // Dirección de movimiento
    public AudioClip Sound;            // Sonido al disparar

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        // Reproducir sonido al instanciar la bala
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        // Mover la bala
        Rigidbody2D.linearVelocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        // Asignar dirección desde quien la dispara
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject); // Eliminar bala
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar si golpea al jugador
        NewMonoBehaviourScript rambo = collision.GetComponent<NewMonoBehaviourScript>();

        // O si golpea a un cobra
        CobraScript cobra = collision.GetComponent<CobraScript>();

        // Aplicar daño según el tipo de objeto
        if (rambo != null)
        {
            rambo.Hit();
        }
        if (cobra != null)
        {
            cobra.Hit();
        }

        // Siempre destruir bala al colisionar
        DestroyBullet();
    }

    void Update()
    {
        // No se usa Update, pero el método se deja vacío
    }
}
