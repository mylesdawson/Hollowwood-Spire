using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscMenuCanvas: MonoBehaviour
{
    [SerializeField] Button resume;
    [SerializeField] Button restart;
    [SerializeField] Button quit;

    void OnEnable()
    {
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(resume.gameObject);
    }

    void Start()
    {
        restart.onClick.AddListener(OnRestartClicked);
        quit.onClick.AddListener(OnQuitClicked);
        resume.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        });
    }

    private void OnRestartClicked()
    {
        EventBus.Instance.onStartGameClicked?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}   