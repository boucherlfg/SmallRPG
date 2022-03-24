using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    const float updateTime = 0.5f;
    private bool Paused
    {
        get => UIManager.Panels.Any(x =>
        {
            var value = !x.ExcludeFromPause && x.Active;
            return value;
        });
    }
    void Start()
    {
        InputManager.Moved += InputManager_Moved;
        InputManager.Used += InputManager_Waited;
        InputManager.Hotbar += InputManager_Hotbar;
        //menus
        InputManager.Equipment += InputManager_Equipment;
        InputManager.Inventory += InputManager_Inventory;
        InputManager.Stats += InputManager_Stats;
        InputManager.Crafting += InputManager_Crafting;
        InputManager.Logs += InputManager_Logs;
        InputManager.Escaped += InputManager_Escaped;
        StartCoroutine(UpdateAndWait());
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
        yield return new WaitUntil(() => Game.Instance != null && Game.Instance.Player != null);
        while (true)
        {
            if (Game.Instance.Player.state == null) yield return null;
            else do
            {
                Game.Instance.Update();
                DisplayManager.Instance.Draw();
                yield return new WaitForSeconds(0.25f);
                yield return new WaitUntil(() => !Paused);
                if (InputManager.MoveVector.magnitude > 0.5f)
                {
                    Game.Instance.Player.state = new Player.MoveState(Game.Instance.Player, InputManager.MoveVector);
                }
            }
            while (Game.Instance.Player.state is Player.MoveState);

        }
    }

    private Coroutine update;


    private void InputManager_Waited()
    {
        if (Paused) return;

        Game.Instance.Player.state = new Player.WaitState(Game.Instance.Player);
    }
    private void InputManager_Moved(Vector2Int orientation)
    {
        if (Paused) return;

        Game.Instance.Player.state = new Player.MoveState(Game.Instance.Player, orientation);
    }

    private void InputManager_Hotbar(int value)
    {
        if (Paused) return;

        Game.Instance.Player.state = new Player.UseState(Game.Instance.Player, value);
    }
}
