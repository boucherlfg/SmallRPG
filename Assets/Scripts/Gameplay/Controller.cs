using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    int level = 1;
    const float updateTime = 0.1f;
    const float level_expansion = 1.25f;
    public void StartGame()
    {
        UIManager.Notifications.CreateNotification("welcome to level " + level);
        Game.Instance.StartNewGame();
        DisplayManager.Instance.Draw();
        InputManager.Moved += InputManager_Moved;
        InputManager.Used += InputManager_Used;

        //menus
        InputManager.Equipment += InputManager_Equipment;
        InputManager.Inventory += InputManager_Inventory;
        InputManager.Stats += InputManager_Stats;
        InputManager.Escaped += InputManager_Escaped;
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
            level++;
            UIManager.Notifications.CreateNotification("welcome to level " + level);
            int newSize = (int)(Game.Instance.Rooms.Count * level_expansion);
            Game.Instance.Init(newSize);
            Game.Instance.Update();
        }
        DisplayManager.Instance.Draw();
        yield return new WaitForSeconds(updateTime);
        update = null;
    }
    private Coroutine update;
    private void InputManager_Moved(Vector2Int orientation)
    {
        
        if (update != null) return;
        //NotifManager.CreateNotification("you moved");
        Game.Instance.Player.Orientation = orientation;
        Game.Instance.Player.state = new Player.MoveState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
    private void InputManager_Used()
    {
        if (update != null) return;
        //NotifManager.CreateNotification("you attacked");
        Game.Instance.Player.state = new Player.UseState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
    private void Inputm()
    {
        if (update != null) return;
        //NotifManager.CreateNotification("you waited");
        Game.Instance.Player.state = new Player.WaitState(Game.Instance.Player);
        update = StartCoroutine(UpdateAndWait());
    }
}
