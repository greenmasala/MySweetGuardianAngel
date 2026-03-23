using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] bool hit;
    Rigidbody rb;
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
        if (hit & !collision.gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }
}
