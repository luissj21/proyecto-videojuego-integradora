using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;
    public AudioClip Sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        NewMonoBehaviourScript rambo = collision.GetComponent<NewMonoBehaviourScript>();
        CobraScript cobra = collision.GetComponent<CobraScript>();

        if (rambo != null)
        {
            rambo.Hit();
        }
        if (cobra != null)
        {
            cobra.Hit();
        }
        DestroyBullet();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    NewMonoBehaviourScript rambo = collision.collider.GetComponent<NewMonoBehaviourScript>();
    //    CobraScript cobra = collision.collider.GetComponent<CobraScript>();

    //    if (rambo != null)
    //    {
    //        rambo.Hit();
    //    }
    //    if (cobra != null)
    //    {
    //        cobra.Hit();
    //    }
    //    DestroyBullet();

    //}
    // Update is called once per frame
    void Update()
    {
        
    }



}
