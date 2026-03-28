using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    //uitool kit (hud menus)
    public UIDocument uiDocument;
    private VisualElement root, pauseMenu, resultSuccess, resultFailure, hud;
    private Label timerLabel, turnsSuccessLabel, timeSuccessLabel, slowTimeLabel;
    private List<VisualElement> heartIcons = new List<VisualElement>();

    //asset ref ui
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite brokenHeart;
    public Animator TransitionAnim;

    //game state
    public bool Paused;
    public bool Started;
    bool win;

    Player player;
    int levelID;
    int nextLevelID;

    //gui
    public Canvas settingsCanvas;


    void Awake()
    {
        root = uiDocument.rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");
        resultSuccess = root.Q<VisualElement>("ResultSuccess");
        resultFailure = root.Q<VisualElement>("ResultFailure");
        hud = root.Q<VisualElement>("HUD");

        timerLabel = root.Q<Label>("TimerLabel");
        timeSuccessLabel = root.Q<Label>("TimeSuccessText");
        turnsSuccessLabel = root.Q<Label>("TurnsSuccessText");

        root.Q<Button>("ResumeButton")?.RegisterCallback<ClickEvent>(evt => Unpause());
        root.Q<Button>("RestartButton")?.RegisterCallback<ClickEvent>(evt => Restart());
        root.Q<Button>("NextLevelButton")?.RegisterCallback<ClickEvent>(evt => NextLevel());
        root.Q<Button>("MenuButton")?.RegisterCallback<ClickEvent>(evt => ReturnToMenu());
        root.Q<Button>("SettingButton")?.RegisterCallback<ClickEvent>(evt => OpenSettings());

        //find hearts
        for (int i = 0; i < 3; i++)
        {
            VisualElement h = root.Q<VisualElement>($"Heart{i}");
            if (h != null) heartIcons.Add(h);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //display
        SetDisplay(pauseMenu, false);
        SetDisplay(resultSuccess, false);
        SetDisplay(resultFailure, false);
        SetDisplay(hud, true);

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

        if (!Paused && Started)
        {
            timer -= Time.deltaTime * timerTimeScale;
        }

        if (Started && !Paused)
        {
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
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        SetDisplay(pauseMenu, true);
        SetDisplay(hud, false);
        Paused = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        SetDisplay(pauseMenu, false);
        settingsCanvas.gameObject.SetActive(false);
        SetDisplay(hud, true);
        Paused = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    void SetDisplay(VisualElement element, bool show)
    {
        if (element != null)
            element.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
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
        Debug.Log("Return to menu");
        TransitionAnim.SetTrigger("Clicked");
        StartCoroutine(LoadMenuScene());
    }

    IEnumerator LoadMenuScene()
    {
        TransitionAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        TransitionAnim.SetTrigger("Clicked");
        yield return new WaitForSecondsRealtime(.55f);
        SceneManager.LoadScene("MainMenu");

    }

    public void Win()
    {
        win = true;
        Paused = true;
        Time.timeScale = 0f;
        SetDisplay(resultSuccess, true);
        SetDisplay(hud, false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        if (timeSuccessLabel != null) timeSuccessLabel.text = timer.ToString("00.000");
        if (turnsSuccessLabel != null) turnsSuccessLabel.text = player.turns.ToString();
    }

    void Lose()
    {
        Paused = true;
        Time.timeScale = 0f;
        SetDisplay(resultFailure, true);
        SetDisplay(hud, false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
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
        SetDisplay(pauseMenu, false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.gameObject.SetActive(false);
        SetDisplay(pauseMenu, true);
    }
}
