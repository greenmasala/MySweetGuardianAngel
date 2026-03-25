using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float enemyTimeScale = 1;
    public float timerTimeScale = 1;
    public List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] float timer = 25f;
    Kid kid;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI TimeSuccess;
    public Canvas PauseMenu;
    public Canvas ResultSuccess;
    public Canvas ResultFailure;
    public bool Paused;
    Player player;
    public bool Started;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
        kid = FindFirstObjectByType<Kid>();
        player = FindFirstObjectByType<Player>();
        Unpause();
    }

    // Update is called once per frame
    void Update()
    {
        TimerText.text = timer.ToString("00.000");

        if (!Paused & Started)
        {
            timer -= Time.deltaTime * timerTimeScale;
        }

        if (obstacles.Count <= 0 & !Paused & !kid.Hit)
        {
            Win();
        }

        if (kid.Hit)
        {
            Lose();
        }
        
        if (!kid.Hit & timer <= 0)
        {
            timer = 0;
            Win();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused == true)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.gameObject.SetActive(true);
        Paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("paused");
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseMenu.gameObject.SetActive(false);
        Paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Paused = false;
    }

    public void ReturnToMenu()
    {
        Debug.Log("Return to menu");
    }

    void Win()
    {
        Paused = true;
        Debug.Log("WIN!!");
        ResultSuccess.gameObject.SetActive(true);
        ResultFailure.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SlowTimePP.gameObject.SetActive(false);
        TimeSuccess.text = timer.ToString("00.000");
    }

    void Lose()
    {
        Paused = true;
        ResultFailure.gameObject.SetActive(true);
        ResultSuccess.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SlowTimePP.gameObject.SetActive(false);
    }
}
