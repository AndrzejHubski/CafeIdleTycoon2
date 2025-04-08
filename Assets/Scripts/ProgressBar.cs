using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fillImage; 
    private Coroutine progressRoutine;

    public void StartProgress(float duration, Action onComplete)
    {
        gameObject.SetActive(true);
        if (progressRoutine != null)
            StopCoroutine(progressRoutine);
        progressRoutine = StartCoroutine(FillBar(duration, onComplete));
    }

    private IEnumerator FillBar(float duration, Action onComplete)
    {
        float time = 0f;
        fillImage.fillAmount = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            fillImage.fillAmount = time / duration;
            yield return null;
        }

        fillImage.fillAmount = 1f;
        onComplete?.Invoke();
        Destroy(gameObject); 
    }
}