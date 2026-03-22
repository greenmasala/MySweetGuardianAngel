using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] int turns = 3;
    Vector2 mouse;
    [SerializeField] float MouseSen = 0.5f;
    float timeValue = 3.5f;
    float timeCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();

        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Space) & turns != 0)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce((Vector3.up + transform.forward) * 10, ForceMode.Impulse);//gotta change this to use f = ma / also might remove vector3.up?
            turns--;
            Debug.Log("turns remaining: " + turns);
        }

        if (Input.GetKey(KeyCode.Q) && timeValue > 0)
        {
            Mathf.Clamp(timeValue, 0f, 3.5f);
            Time.timeScale = 0.2f;
            timeValue -= Time.deltaTime;
            timeCooldown = 1f;
            Debug.Log(timeValue);
        }
        else if (timeValue < 3.5f)
        {
            Time.timeScale = 1f;
            timeCooldown -= Time.deltaTime;

            if (timeCooldown < 0f)
            {
                timeValue += Time.deltaTime;
                Debug.Log(timeValue);
            }
        }
    }

    void CameraRotation()
    {
        mouse.x += Input.GetAxis("Mouse X") * MouseSen;
        mouse.y += Input.GetAxis("Mouse Y") * MouseSen;
        var rotation = Quaternion.Euler(Mathf.Clamp(-mouse.y, -75, 75), mouse.x, 0);
        rb.MoveRotation(rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var colRb = collision.gameObject.GetComponent<Rigidbody>();

        colRb.AddForce(transform.forward * 30, ForceMode.Impulse);
    }
}

