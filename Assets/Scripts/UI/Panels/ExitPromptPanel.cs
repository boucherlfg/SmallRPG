using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPromptPanel : PanelWrapper
{
    public void LoadNextLevel()
    {
        Game.Instance.NextLevel();
        Toggle();
    }
}
