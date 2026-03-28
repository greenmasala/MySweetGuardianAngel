using System.Collections;
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
    public TextMeshProUGUI TurnsSuccessText;
    public Canvas PauseMenu;
    public Canvas ResultSuccess;
    public Canvas ResultFailure;
    public Canvas SettingsMenu;
    public bool Paused;
    Player player;
    public bool Started;
    bool win;
    public Animator TransitionAnim;
    int levelID;
    int nextLevelID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
        kid = FindFirstObjectByType<Kid>();
        player = FindFirstObjectByType<Player>();
        Unpause();
        levelID = SceneManager.GetActiveScene().buildIndex;
        nextLevelID = levelID + 1;
        Debug.Log(nextLevelID);
        TransitionAnim = GameObject.Find("Transition").GetComponent<Animator>();
        TransitionAnim.updateMode = AnimatorUpdateMode.Normal;
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

        if (kid.Hit & !win)
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
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
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
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(LoadLevel(levelID));
    }

    public void NextLevel()
    {
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(LoadLevel(nextLevelID));
    }

    public void ReturnToMenu()
    {
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Debug.Log("Return to menu");
        TransitionAnim.SetTrigger("Clicked");
        SceneManager.LoadScene("MainMenu");

    }

    void Win()
    {
        win = true;
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Paused = true;
        Debug.Log("WIN!!");
        ResultSuccess.gameObject.SetActive(true);
        ResultFailure.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SlowTimePP.gameObject.SetActive(false);
        TimeSuccess.text = timer.ToString("00.000");
        TurnsSuccessText.text = player.turns.ToString();
    }

    void Lose()
    {
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Paused = true;
        ResultFailure.gameObject.SetActive(true);
        ResultSuccess.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SlowTimePP.gameObject.SetActive(false);
    }

    IEnumerator LoadLevel(int levelID)
    {
        TransitionAnim.SetTrigger("Clicked");
        yield return new WaitForSecondsRealtime(.55f);
        SceneManager.LoadScene(levelID);
        Paused = false;
    }

    public void OpenSettings()
    {
        SettingsMenu.gameObject.SetActive(true);
        PauseMenu.gameObject.SetActive(false);
    }

    public void CloseSettings()
    {
        SettingsMenu.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
    }
}
