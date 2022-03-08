using UnityEngine;

public class PausePanel : PanelWrapper
{
    public void Continue()
    {
        Toggle();
    }
    public void Restart()
    {
        Game.Instance.StartNewGame();
        Toggle();
    }
    public void Quit()
    {
        Application.Quit();
    }
}