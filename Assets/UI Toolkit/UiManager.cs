using UnityEngine;
using System;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    public VisualElement root, mainMenuOverlay;
    void start()
    {
        root = uiDocument.rootVisualElement;

        var playButton = root.Q<VisualElement>("ButtonPlay");
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);

        mainMenuOverlay = root.Q<VisualElement>("MainMenuOverlay");

    }
    void OnPlayButtonClicked(ClickEvent evt)
    {
        mainMenuOverlay.visible = false;
        Debug.Log("play");
    }
}
