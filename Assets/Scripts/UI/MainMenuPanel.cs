using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : PanelWrapper
{
    [SerializeField]
    private Controller controller;
    void OnEnable()
    {
        StartCoroutine(PlayMusicWhenReady());

        IEnumerator PlayMusicWhenReady()
        {
            yield return new WaitUntil(() => AudioManager.HasInstance);
            AudioManager.PlayAsMusic("intro_song");
        }
    }
    void OnDisable()
    {
        AudioManager.StopMusic();
    }
    public void Storyline()
    {
        UIManager.Storyline.Toggle();
        Active = false;
    }
    public void StartGame()
    {
        UIManager.Storyline.Toggle();
        controller.StartGame();
        UIManager.SplashScreen.Toggle();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
