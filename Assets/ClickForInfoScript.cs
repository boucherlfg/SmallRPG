using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickForInfoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForInputManager());

        IEnumerator WaitForInputManager()
        {
            yield return null;
            InputManager.Clicked += InputManager_Clicked;
        }
    }

    private void InputManager_Clicked()
    {
        if (Game.Instance == null) return;

        var closest = Game.Instance.Agents.Minimum(a => Vector2.Distance(a.position, InputManager.MousePosition));
        if (Vector2.Distance(closest.position, InputManager.MousePosition) < 1)
        {
            UIManager.Notifications.CreateNotification("this is a " + closest.GetType().Name);
        }
        else
        {
            UIManager.Notifications.CreateNotification("There is nothing here");
        }
    }
}
