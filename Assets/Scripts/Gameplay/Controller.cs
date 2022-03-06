using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    int level = 1;
    const float updateTime = 0.1f;
    const float level_expansion = 1.25f;
    public void Start()
    {
        NotifManager.CreateNotification("welcome to level " + level);
        Game.Instance.StartNewGame();
        DisplayManager.Instance.Draw();
        InputManager.Moved += InputManager_Moved;
        InputManager.Attacked += InputManager_Attacked;
        InputManager.Waited += InputManager_Waited;
    }

    IEnumerator UpdateEvery()
    {
        Game.Instance.Update();
        if (Game.Instance.Player.state is Player.ExitState)
        {
            level++;
            NotifManager.CreateNotification("welcome to level " + level);
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
        update = StartCoroutine(UpdateEvery());
    }
    private void InputManager_Attacked()
    {
        if (update != null) return;
        //NotifManager.CreateNotification("you attacked");
        Game.Instance.Player.state = new Player.UseState(Game.Instance.Player);
        update = StartCoroutine(UpdateEvery());
    }
    private void InputManager_Waited()
    {
        if (update != null) return;
        //NotifManager.CreateNotification("you waited");
        Game.Instance.Player.state = new Player.WaitState(Game.Instance.Player);
        update = StartCoroutine(UpdateEvery());
    }
}
