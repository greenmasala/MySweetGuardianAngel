using System.Collections;
using System.Drawing;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] int turns = 3;
    Vector2 mouse;
    [SerializeField] float MouseSen = 0.5f;
    [SerializeField] float timeValue = 3.5f;
    float maxTimeValue;
    float timeCooldown = 1f;
    public Animator anim;
    public ParticleSystem JumpVFX;
    public ParticleSystem HitVFX;
    GameManager gameManager;
    public Volume SlowTimePP;
    public TextMeshProUGUI TurnsText;
    public TextMeshProUGUI SlowTimeText;
    public CinemachineCamera PlayerCam;
    Screenshake shake;
    bool hasPlayed;

    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip slowTimeSFX;
    [SerializeField] AudioClip normalTimeSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindFirstObjectByType<GameManager>();
        maxTimeValue = timeValue;
        shake = PlayerCam.GetComponent<Screenshake>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        TurnsText.text = turns.ToString();
        SlowTimeText.text = timeValue.ToString("00.000");
        CameraRotation();

        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        if (!gameManager.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Space) & turns != 0)
            {
                JumpVFX.Play();
                anim.SetTrigger("Jump");
                anim.SetBool("OnGround", false);
                rb.linearVelocity = Vector3.zero;
                rb.AddForce((transform.up + transform.forward) * 10, ForceMode.Impulse);//gotta change this to use f = ma / also might remove vector3.up?
                turns--;
                Debug.Log("turns remaining: " + turns);
                SFXManager.Instance.PlaySound(jumpSFX, transform, 1f);
            }

            if (Input.GetKeyDown(KeyCode.Q) && timeValue > 0)
            {
                SFXManager.Instance.PlaySound(slowTimeSFX, transform, 1f);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                SFXManager.Instance.PlaySound(normalTimeSFX, transform, 1f);
            }
        }

            if (Input.GetKey(KeyCode.Q) && timeValue > 0)
            {
                SlowTimePP.gameObject.SetActive(true);
                Mathf.Clamp(timeValue, 0f, 3.5f);
                //Time.timeScale = 0.2f;
                gameManager.enemyTimeScale = .5f;
                gameManager.timerTimeScale = .35f;
                timeValue -= Time.deltaTime;
                timeCooldown = 1f;
            }
            else if (timeValue < maxTimeValue || timeValue <= 0)
            {
                SlowTimePP.gameObject.SetActive(false);
                //Time.timeScale = 1f;
                gameManager.enemyTimeScale = 1f;
                gameManager.timerTimeScale = 1f;
                timeCooldown -= Time.deltaTime;

                if (timeCooldown < 0f)
                {
                    timeValue += Time.deltaTime;
                }
            }
        }
    void CameraRotation()
    {
        if (!gameManager.Paused)
        {
            mouse.x += Input.GetAxis("Mouse X") * MouseSen;
            mouse.y += Input.GetAxis("Mouse Y") * MouseSen;
            var rotation = Quaternion.Euler(Mathf.Clamp(-mouse.y, -75, 75), mouse.x, 0);
            rb.MoveRotation(rotation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("OnGround", true);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            SFXManager.Instance.PlaySound(hitSFX, transform, 1f);
            shake.StartCoroutine(shake.Shake(0.2f, 0.45f));
            var colRb = collision.gameObject.GetComponent<Rigidbody>();
            var hitPrefab = Instantiate(HitVFX, transform.position, Quaternion.identity);
            colRb.useGravity = true;
            colRb.AddForce(transform.forward * 60, ForceMode.Impulse);
            collision.gameObject.GetComponent<TrailRenderer>().enabled = true;
            Destroy(hitPrefab.gameObject, 2f);
            Destroy(colRb.gameObject, 7f);
        }
    }
}
 

   



