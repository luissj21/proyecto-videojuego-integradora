using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float Speed;
    public float JumpForce;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private bool Grounded;
    private Animator Animator;
    private float LastShoot;
    public int Health = 25;
    public float FallLimitY = -10f; // Altura mínima antes de morir

    //  Nueva variable para punto de reaparición
    private Vector3 RespawnPosition;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // Guarda la posición inicial como punto de reaparición
        RespawnPosition = transform.position;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.1f);

        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        // Si cae por debajo del límite, "muere" automáticamente
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
        Vector3 direction = (transform.localScale.x == 1.0f) ? Vector2.right : Vector2.left;
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
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

    //  Nueva función para reaparecer al jugador
    private void DieAndRespawn()
    {
        // Opcional: podrías reproducir una animación de muerte aquí

        // Desactivar temporalmente el jugador
        gameObject.SetActive(false);

        // Iniciar el respawn después de 2 segundos
        Invoke(nameof(Respawn), 2f);
    }

    private void Respawn()
    {
        // Restaurar salud
        Health = 25;

        // Mover al punto de reaparición
        transform.position = RespawnPosition;

        // Reactivar el jugador
        gameObject.SetActive(true);
    }
}
