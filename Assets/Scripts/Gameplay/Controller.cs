using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{    
    const float updateTime = 0.5f;
    private bool paused = true;
    void Start()
    {
        InputManager.Moved += InputManager_Moved;
        InputManager.Used += InputManager_Used;
        InputManager.Waited += InputManager_Waited;
        //menus
        InputManager.Equipment += InputManager_Equipment;
        InputManager.Inventory += InputManager_Inventory;
        InputManager.Stats += InputManager_Stats;
        InputManager.Crafting += InputManager_Crafting;
        InputManager.Logs += InputManager_Logs;
        InputManager.Escaped += InputManager_Escaped;
        PanelWrapper.AnyActiveStateChanged += PanelWrapper_AnyActiveStateChanged;
    }


    public void StartGame()
    {
        UIManager.SplashScreen.Toggle();


        Game.Instance.StartNewGame();
        DisplayManager.Instance.Draw();
    }

    private void InputManager_Logs()
    {
        UIManager.Logs.Toggle();
    }

    private void InputManager_Crafting()
    {
        UIManager.Crafting.Toggle();
        UIManager.Crafting.CraftingType = CraftingType.Hand;
    }

    private void PanelWrapper_AnyActiveStateChanged()
    {
        paused = UIManager.Panels.Any(x =>
        {
            return !x.ExcludeFromPause && x.Active;
        });
    }

    private void InputManager_Escaped()
    {
        if (Game.Instance == null || Game.Instance.Player is Tombstone) return;
        var activePanel = UIManager.Panels.FirstOrDefault(x => !(x is PausePanel) && x.Active && x.ExitableByEscape);
        if (activePanel)
        {
            activePanel.Toggle();
        }
        else
        {
            UIManager.Pause.Toggle();
        }
    }

    private void InputManager_Stats()
    {
        UIManager.Stats.Toggle();
    }

    private void InputManager_Inventory()
    {
        UIManager.Inventory.Toggle();
    }

    private void InputManager_Equipment()
    {
        UIManager.Equipment.Toggle();
    }


    IEnumerator UpdateAndWait()
    {
        Game.Instance.Update();
        DisplayManager.Instance.Draw();
        yield return new WaitForSeconds(0.1f);
        update = null;
    }

    private Coroutine update;


    private void InputManager_Waited()
    {
        if (paused) return;
        if (update != null) return;

        Game.Instance.Player.state = new Player.WaitState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
    private void InputManager_Moved(Vector2Int orientation)
    {
        if (paused) return;
        if (update != null) return;

        Game.Instance.Player.Orientation = orientation;
        Game.Instance.Player.state = new Player.MoveState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
    private void InputManager_Used()
    {
        if (paused) return;
        if (update != null) return;

        Game.Instance.Player.state = new Player.UseState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
}
