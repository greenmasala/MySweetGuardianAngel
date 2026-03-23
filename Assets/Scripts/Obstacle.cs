using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
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
        rb.linearVelocity *= gameManager.enemyTimeScale;
        rb.angularVelocity *= gameManager.enemyTimeScale;
    }
}
