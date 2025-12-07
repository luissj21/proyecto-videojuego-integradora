using UnityEngine;

public class CobraScript : MonoBehaviour
{
    public GameObject BulletPrefab; // Bala que dispara el enemigo
    public GameObject Rambo;       // Referencia al jugador

    private float LastShoot;       // Controla el tiempo entre disparos
    private int Health = 3;        // Vida de este enemigo

    private void Update()
    {
        // Si Rambo no existe o está desactivado, no hacer nada
        if (Rambo == null || !Rambo.activeInHierarchy) return;

        // Mirar hacia Rambo
        Vector3 direction = Rambo.transform.position - transform.position;
        if (direction.x >= 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        // Distancia horizontal entre Cobra y Rambo
        float distance = Mathf.Abs(Rambo.transform.position.x - transform.position.x);

        // Si está cerca y pasó el tiempo suficiente, dispara
        if (distance < 1.0f && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        // Determinar dirección según hacia dónde mira
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        // Crear bala
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);

        // Aplicar dirección a la bala
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    public void Hit()
    {
        // Recibir daño
        Health = Health - 1;

        // Si vida llega a 0, destruir
        if (Health == 0) Destroy(gameObject);
    }
}
