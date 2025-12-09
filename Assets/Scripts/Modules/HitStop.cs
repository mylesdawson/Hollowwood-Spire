using System.Collections;
using UnityEngine;

public class HitStop: MonoBehaviour
{
    bool isWaiting = false;

    public void Stop(float duration)
    {
        if (isWaiting) return;
        Time.timeScale = 0f;
        StartCoroutine(ResumeAfterDelay(duration));
    }
    
    IEnumerator ResumeAfterDelay(float duration)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        isWaiting = false;
    }
}