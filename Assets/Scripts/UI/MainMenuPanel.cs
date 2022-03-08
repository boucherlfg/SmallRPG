using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : PanelWrapper
{
    [SerializeField]
    private Controller controller;
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
