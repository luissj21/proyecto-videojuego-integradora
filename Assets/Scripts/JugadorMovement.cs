using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject BulletPrefab; // Bala del jugador
    public float Speed;             // Velocidad horizontal
    public float JumpForce;         // Fuerza del salto

    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private bool Grounded;
    private Animator Animator;
    private float LastShoot;

    public int Health = 25;         // Vida del jugador

    public float FallLimitY = -10f; // Si cae más abajo, muere

    private Vector3 RespawnPosition; // Punto donde reaparece

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // Guardar punto de inicio como respawn
        RespawnPosition = transform.position;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Voltear sprite según dirección de movimiento
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        // Comprobar si está tocando el piso
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.1f);

        // Saltar
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        // Si cae fuera del mapa, morir y respawnear
        if (transform.position.y < FallLimitY)
        {
            Health = 0;
            DieAndRespawn();
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot()
    {
        // Dirección según hacia dónde mira el jugador
        Vector3 direction = (transform.localScale.x == 1.0f) ? Vector2.right : Vector2.left;

        // Crear bala
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);

        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        Rigidbody2D.linearVelocity = new Vector2(Horizontal, Rigidbody2D.linearVelocity.y);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health <= 0)
        {
            DieAndRespawn();
        }
    }

    private void DieAndRespawn()
    {
        // Podrías animar muerte aquí

        // Desactivar jugador
        gameObject.SetActive(false);

        // Reaparecer después de 2 segundos
        Invoke(nameof(Respawn), 2f);
    }

    private void Respawn()
    {
        Health = 25; // Restaurar vida

        transform.position = RespawnPosition; // Volver al inicio

        gameObject.SetActive(true); // Reactivar jugador
    }
}
