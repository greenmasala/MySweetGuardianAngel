using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    int turns = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Space) & turns != 0)
        {
            rb.AddForce(new Vector3(0f, 1f, 1f) * 10, ForceMode.Impulse);
            turns--;
            Debug.Log("turns remaining: " + turns);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Time.timeScale = 0.2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}

