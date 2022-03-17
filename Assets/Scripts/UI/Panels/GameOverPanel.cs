using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : PanelWrapper
{
    public void StartAgain()
    {
        Game.Instance.StartNewGame();
        Active = false;
    }
    public void MainMenu()
    {
        UIManager.MainMenu.Toggle();
        Active = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
