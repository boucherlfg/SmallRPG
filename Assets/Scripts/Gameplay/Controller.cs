using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    private Queue<Player.State> todo = new Queue<Player.State>();
    const float updateTime = 0.5f;
    private bool paused = true;
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
        PanelWrapper.AnyActiveStateChanged += PanelWrapper_AnyActiveStateChanged;
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
        while (true)
        {
            if (todo.Count <= 0)
            {
                yield return null;
            }
            else
            {
                var state = todo.Dequeue();
                Game.Instance.Player.state = state;

                do
                {
                    Game.Instance.Update();
                    DisplayManager.Instance.Draw();
                    yield return new WaitForSeconds(0.25f);
                    yield return new WaitUntil(() => !paused);
                    if (InputManager.MoveVector.magnitude > 0.5f)
                    {
                        Game.Instance.Player.state = new Player.MoveState(Game.Instance.Player, InputManager.MoveVector);
                    }
                }
                while (Game.Instance.Player.state is Player.MoveState);
            }
        }
    }

    private Coroutine update;


    private void InputManager_Waited()
    {
        if (paused) return;

        todo.Enqueue(new Player.WaitState(Game.Instance.Player));
    }
    private void InputManager_Moved(Vector2Int orientation)
    {
        if (paused) return;

        todo.Enqueue(new Player.MoveState(Game.Instance.Player, orientation));
    }

    private void InputManager_Hotbar(int value)
    {
        if (paused) return;

        todo.Enqueue(new Player.UseState(Game.Instance.Player, value));
    }
}
