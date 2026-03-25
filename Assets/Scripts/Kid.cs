using UnityEngine;

public class Kid : MonoBehaviour
{
    public bool Hit;
    Animator anim;
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.speed = gameManager.timerTimeScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Hit = true;
            Debug.Log("failed");
        }
    }
}
