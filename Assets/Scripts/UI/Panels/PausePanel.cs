using UnityEngine;

public class PausePanel : PanelWrapper
{
    public override bool ExitableByEscape => true;
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