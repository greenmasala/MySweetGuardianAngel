using System.Drawing;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] bool hit;
    Rigidbody rb;
    public GameObject ExplosionVFX;
    [SerializeField] AudioClip explosionSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hit)
        {
            rb.linearVelocity *= gameManager.enemyTimeScale;
            rb.angularVelocity *= gameManager.enemyTimeScale;
        }
        if (!gameManager.Started)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if (collision.gameObject.GetComponent<ConstantForce>())
            {
                collision.gameObject.GetComponent<ConstantForce>().enabled = false;
            }
            hit = true;
        }
        if (hit & !collision.gameObject.GetComponent<Player>() || collision.gameObject.GetComponent<Kid>() || collision.gameObject.CompareTag("Obstacle"))
        {
            var explosionPrefab = Instantiate(ExplosionVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosionPrefab.gameObject, 2f);
            gameManager.obstacles.Remove(gameObject); //need to remove from list in auto delete aswell
            SFXManager.Instance.PlaySound(explosionSFX, transform, 1f);

            if (collision.gameObject.GetComponent<Rigidbody>() & !collision.gameObject.GetComponent<Player>())
            {
                Destroy(collision.gameObject);
            }      
        }
    }
}
