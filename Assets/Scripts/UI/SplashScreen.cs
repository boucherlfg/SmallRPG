using System.Collections;
using UnityEngine;

public class SplashScreen : PanelWrapper
{
    CanvasGroup canvasGroup;
    public float timeBeforeFadeout = 3;
    public float fadeoutTime = 2;
    void Update()
    {
        if (!Active) return;
        timeBeforeFadeout -= Time.deltaTime;
        if (timeBeforeFadeout > 0) return;
        canvasGroup.alpha -= Time.deltaTime;
        if (canvasGroup.alpha > 0) return;
        canvasGroup.alpha = 1;
        Active = false;
    }
    void OnEnable()
    {
        canvasGroup = menu.GetComponent<CanvasGroup>();
        timeBeforeFadeout = 3;
        fadeoutTime = 2;
    }
}