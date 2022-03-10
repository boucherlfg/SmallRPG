using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    const float updateTime = 0.1f;
    private bool paused = true;
    public void StartGame()
    {
        InputManager.Moved += InputManager_Moved;
        InputManager.Attacked += InputManager_Attacked;
        InputManager.Used += InputManager_Used;
        //menus
        InputManager.Equipment += InputManager_Equipment;
        InputManager.Inventory += InputManager_Inventory;
        InputManager.Stats += InputManager_Stats;
        InputManager.Escaped += InputManager_Escaped;
        PanelWrapper.AnyActiveStateChanged += PanelWrapper_AnyActiveStateChanged;


        Game.Instance.StartNewGame();
        DisplayManager.Instance.Draw();
    }


    private void PanelWrapper_AnyActiveStateChanged()
    {
        paused = UIManager.Panels.Any(x =>
        {
            return x.Active;
        });
    }

    private void InputManager_Escaped()
    {
        var activePanel = UIManager.Panels.FirstOrDefault(x => !(x is PausePanel) && x.Active);
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
        if (Game.Instance.Player.state is Player.ExitState)
        {
            Game.Instance.NextLevel();
        }
        DisplayManager.Instance.Draw();
        yield return new WaitForSeconds(updateTime);
        update = null;
    }
    private Coroutine update;
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

    private void InputManager_Attacked()
    {
        if (paused) return;
        if (update != null) return;
        Game.Instance.Player.state = new Player.AttackState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
}
