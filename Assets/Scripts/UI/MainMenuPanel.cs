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
    public void StartGame()
    {
        controller.StartGame();
        Toggle();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
