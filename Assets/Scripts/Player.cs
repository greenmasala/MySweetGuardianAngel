using System.Collections;
using System.Drawing;
using System.Transactions;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public int turns = 3;
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
    Settings settings;
    public TrailRenderer[] Trails;
    bool onGround = true;
    Kid kid;

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
        settings = FindFirstObjectByType<Settings>();
        TurnsText = GameObject.Find("Turns").GetComponent<TextMeshProUGUI>();
        SlowTimeText = GameObject.Find("SlowTime").GetComponent<TextMeshProUGUI>();
        SlowTimePP = GameObject.FindWithTag("PostProcess").GetComponent<Volume>();
        kid = FindFirstObjectByType<Kid>();
        Debug.Log(SlowTimePP);
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
                foreach (TrailRenderer trail in Trails)
                {
                    trail.enabled = true;
                }
                JumpVFX.Play();
                anim.SetTrigger("Jump");
                anim.SetBool("OnGround", false);
                rb.linearVelocity = Vector3.zero;
                rb.AddForce((transform.up + transform.forward) * 10, ForceMode.Impulse);//gotta change this to use f = ma / also might remove vector3.up?
                turns--;
                Debug.Log("turns remaining: " + turns);
                SFXManager.Instance.PlaySound(jumpSFX, transform, 1f);
                onGround = false;
                gameManager.Started = true;
            }

            if (Input.GetKeyDown(KeyCode.Q) && timeValue > 0)
            {
                SFXManager.Instance.PlaySound(slowTimeSFX, transform, 1f);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                SFXManager.Instance.PlaySound(normalTimeSFX, transform, 1f);
            }

            if(Input.GetKey(KeyCode.Q) && timeValue > 0)
            {
                SlowTimePP.enabled = true;
                Mathf.Clamp(timeValue, 0f, 3.5f);
                //Time.timeScale = 0.2f;
                gameManager.enemyTimeScale = .65f; //.5f
                gameManager.timerTimeScale = .35f;
                timeValue -= Time.deltaTime;
                timeCooldown = 1f;
            }
            else if (timeValue < maxTimeValue || timeValue <= 0)
            {
                SlowTimePP.enabled = false;
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
    }

    void CameraRotation()
    {
        if (!gameManager.Paused)
        {
            mouse.x += Input.GetAxis("Mouse X") * settings.MouseSen;
            mouse.y += Input.GetAxis("Mouse Y") * settings.MouseSen;
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
            foreach (TrailRenderer trail in Trails)
            {
                trail.enabled = false;
            }

            if (!onGround)
            {
                JumpVFX.Play();
                onGround = true;
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Kid"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            SFXManager.Instance.PlaySound(hitSFX, transform, 1f);
            shake.StartCoroutine(shake.Shake(0.2f, 0.45f));
            var colRb = collision.gameObject.GetComponent<Rigidbody>();
            var hitPrefab = Instantiate(HitVFX, transform.position, Quaternion.identity);
            colRb.useGravity = true;
            colRb.AddForce((transform.up + transform.forward) * 55, ForceMode.Impulse); //prev transform.forward * 60
            collision.gameObject.GetComponent<TrailRenderer>().enabled = true;
            Destroy(hitPrefab.gameObject, 2f);

            if (collision.gameObject.CompareTag("Kid"))
            {
                kid.Hit = true;
            }
            //Destroy(colRb.gameObject, 7f);
            //need to remove from list SOMEHOW
        }
    }
}
 

   



