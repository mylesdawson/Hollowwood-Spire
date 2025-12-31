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

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        restart.onClick.AddListener(OnRestartClicked);
        quit.onClick.AddListener(OnQuitClicked);
        resume.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    private void OnRestartClicked()
    {
        Debug.Log("restart clicked");
        throw new NotImplementedException();
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}   