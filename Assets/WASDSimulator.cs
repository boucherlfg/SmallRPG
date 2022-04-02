using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WASDSimulator : MonoBehaviour
{
    public void Appear(Image image)
    {
        image.color = new Color(1, 1, 1, 0.2f);
    }
    public void Disappear(Image image)
    {
        image.color = new Color(0, 0, 0, 0);
    }
    public void SimulateLeft()
    {
        InputManager.SimulateInput(Vector2Int.left);
    }
    public void SimulateRight()
    {
        InputManager.SimulateInput(Vector2Int.right);
    }
    public void SimulateUp()
    {
        InputManager.SimulateInput(Vector2Int.up);
    }
    public void SimulateDown()
    {
        InputManager.SimulateInput(Vector2Int.down);
    }
    public void SimulateWait()
    {
        InputManager.SimulateInput(Vector2Int.zero);
    }
}
