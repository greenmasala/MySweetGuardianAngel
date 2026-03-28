using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    [SerializeField] private CinemachineCamera mainMenuCam, levelSelectCam;

    public VisualElement root, mainMenuOverlay, creditOverlay, levelOverlay;
    private Button backButton;
    public GameObject transitionPrefab;

    void Start()
    {
        root = uiDocument.rootVisualElement;

        var playButton = root.Q<VisualElement>("ButtonPlayIn");
        var creditButton = root.Q<VisualElement>("CreditButton");
        var quitButton = root.Q<VisualElement>("QuitButton");

        mainMenuOverlay = root.Q<VisualElement>("MainMenuOverlay");
        creditOverlay = root.Q<VisualElement>("Credit");
        levelOverlay = root.Q<VisualElement>("LevelMenuOverlay");

        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        creditButton.RegisterCallback<ClickEvent>(OnCreditButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        
        backButton = root.Q<Button>("Back");
        root.Query<Button>("Back").ForEach(btn => {btn.RegisterCallback<ClickEvent>(OnBackButtonClicked);});

        var levelButtons = levelOverlay.Query<VisualElement>(className: "level-button").ToList();
        Debug.Log("Found " + levelButtons.Count);

        foreach (var btn in levelButtons)
        {

            btn.RegisterCallback<ClickEvent>(evt =>
            {
                string levelNumber = btn.name.Replace("LevelButton", "");
                string sceneToLoad = "Level" + levelNumber;
                LoadLevel(sceneToLoad);
            });
        }
    }
    void OnPlayButtonClicked(ClickEvent evt)
    {
        mainMenuCam.Priority = 11;
        levelSelectCam.Priority = 12;
        mainMenuOverlay.style.display = DisplayStyle.None;
        levelOverlay.style.display = DisplayStyle.Flex;
        GameObject instance = Instantiate(transitionPrefab);
        Destroy(instance, 1f);
        Debug.Log("play");
    }
    void OnCreditButtonClicked(ClickEvent evt)
    {

        mainMenuOverlay.style.display = DisplayStyle.None;
        GameObject instance = Instantiate(transitionPrefab);
        Destroy(instance, 2f);
        creditOverlay.style.display = DisplayStyle.Flex;
        Debug.Log("Credit");
    }

    void OnBackButtonClicked (ClickEvent evt) 
    {

        bool checkOpenUI = false;
        if (creditOverlay.style.display == DisplayStyle.Flex)
        {
            creditOverlay.style.display = DisplayStyle.None;
            checkOpenUI = true;
        }
        if (levelOverlay.style.display == DisplayStyle.Flex)
        {
            levelOverlay.style.display = DisplayStyle.None;
            checkOpenUI = true;
        }
        if (checkOpenUI)
        {
            mainMenuOverlay.style.display = DisplayStyle.Flex;
        }
        Debug.Log("back");
        GameObject instance = Instantiate(transitionPrefab);
        Destroy(instance, 2f);
    }
    void OnQuitButtonClicked(ClickEvent evt)
    {
        GameObject instance = Instantiate(transitionPrefab);
        Destroy(instance, 2f);
        Application.Quit();
        Debug.Log("Quit");
    }

    void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Debug.Log("Scene"+ name + "loaded");
    }
}
