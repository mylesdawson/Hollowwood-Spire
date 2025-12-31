

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGameCanvas: MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }


    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnStartButtonClicked()
    {
        EventBus.Instance.onStartGameClicked?.Invoke();
    }
    
    private void OnQuitClicked()
    {
        Application.Quit();
    }
}   