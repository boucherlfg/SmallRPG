using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : PanelWrapper
{

    public override bool ExcludeFromPause => false;
    public override bool ExitableByEscape => false;
    [SerializeField]
    private Controller controller;
    void Start()
    {
        ActiveStateChanged += PlayIntroMusic;
        StartCoroutine(PlayMusicWhenReady());

        IEnumerator PlayMusicWhenReady()
        {
            yield return new WaitUntil(() => AudioManager.HasInstance);
            PlayIntroMusic();
        }
    }

    private void PlayIntroMusic()
    {
        if (Active)
        {
            AudioManager.PlayAsMusic("intro_song");
        }
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
    }
    public void Quit()
    {
        Application.Quit();
    }
}
