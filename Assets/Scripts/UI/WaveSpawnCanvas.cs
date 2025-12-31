

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveSpawnCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text countdown;
    public Action onCountdownFinished;


    public void Initialize(int roundNumber)
    {
        title.text = $"Round {roundNumber}";
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        for (int i = 3; i >= 0; i--)
        {
            // Trigger LeanTween animation
            PlayTickAnimation(i);

            Debug.Log(i);

            // Wait exactly 1 second
            yield return new WaitForSecondsRealtime(1f);
        }

        onCountdownFinished?.Invoke();
    }

    void PlayTickAnimation(int number)
    {
        // Example animation: punch scale
        if(number == 0)
        {
            countdown.text = "GO!";
        } else
        {
            countdown.text = number.ToString();
        }
        LeanTween.scale(countdown.gameObject, Vector3.one * 1.2f, 0.25f)
                 .setEasePunch()
                 .setIgnoreTimeScale(true);
    }

}