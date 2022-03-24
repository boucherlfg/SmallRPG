using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{

    #region events
    private event TypedAction<Vector2Int> onMoved;
    public static event TypedAction<Vector2Int> Moved
    {
        add => _instance.onMoved += value;
        remove => _instance.onMoved -= value;
    }

    private event VoidAction onUsed;
    public static event VoidAction Used
    {
        add => _instance.onUsed += value;
        remove => _instance.onUsed -= value;
    }

    private event VoidAction onEquipment;
    public static event VoidAction Equipment
    {
        add => _instance.onEquipment += value;
        remove => _instance.onEquipment -= value;
    }

    private event VoidAction onEscaped;
    public static event VoidAction Escaped
    {
        add => _instance.onEscaped += value;
        remove => _instance.onEscaped -= value;
    }

    private event VoidAction onInventory;
    public static event VoidAction Inventory
    {
        add => _instance.onInventory += value;
        remove => _instance.onInventory -= value;
    }

    private event VoidAction onStats;
    public static event VoidAction Stats
    {
        add => _instance.onStats += value;
        remove => _instance.onStats -= value;
    }

    private event VoidAction onCrafting;
    public static event VoidAction Crafting
    {
        add => _instance.onCrafting += value;
        remove => _instance.onCrafting -= value;
    }

    private event VoidAction onLogs;
    public static event VoidAction Logs
    {
        add => _instance.onLogs += value;
        remove => _instance.onLogs -= value;
    }

    private event VoidAction onClick;
    public static event VoidAction Clicked
    {
        add => _instance.onClick += value;
        remove => _instance.onClick -= value;
    }

    private event VoidAction onWait;
    public static event VoidAction Waited
    {
        add => _instance.onWait += value;
        remove => _instance.onWait -= value;
    }
    private event TypedAction<int> onHotbar;
    public static event TypedAction<int> Hotbar
    {
        add => _instance.onHotbar += value;
        remove => _instance.onHotbar -= value;
    }
    #endregion

    public static Vector2 MousePosition => Camera.main.ScreenToWorldPoint(_instance.controls.Player.MousePosition.ReadValue<Vector2>());

    private static Vector2Int lastMove;
    public static Vector2Int MoveVector
    {
        get
        {
            var move = _instance.controls.Player.Move.ReadValue<Vector2>();
            if (Mathf.Abs(move.x) > 0.1f && Mathf.Abs(move.y) > 0.1f)
            {
                return lastMove;
            }
            lastMove = Vector2Int.RoundToInt(move);
            return lastMove;
        }
    }



    public static bool Active
    {
        get => _instance.controls.Player.enabled;
        set
        {
            if (value) _instance.controls.Player.Enable();
            else _instance.controls.Player.Disable();
        }
    }
    private Controls controls;
    public override void Awake()
    {
        base.Awake();
        controls = new Controls();
        controls.Player.Enable();

        controls.Player.Move.performed += Move_performed;
        controls.Player.Use.performed += Use_Performed;
        controls.Player.Click.performed += Click_performed;
        controls.Player.Wait.performed += Wait_performed;

        controls.Player.Escape.performed += Escape_performed;
        controls.Player.Inventory.performed += Inventory_performed;
        controls.Player.Equipment.performed += Equipment_performed;
        controls.Player.Crafting.performed += Crafting_performed;
        controls.Player.Stats.performed += Stats_performed;
        controls.Player.Logs.performed += Logs_performed;
        controls.Player.Hotbar.performed += Hotbar_performed;
    }

    private void Hotbar_performed(InputAction.CallbackContext obj)
    {
        int value = int.Parse(obj.control.name);
        onHotbar?.Invoke(value);
    }

    private void Wait_performed(InputAction.CallbackContext obj)
    {
        onWait?.Invoke();
    }

    private void Click_performed(InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            onClick?.Invoke();
        }
    }

    private void Logs_performed(InputAction.CallbackContext obj)
    {
        onLogs?.Invoke();
    }

    private void Crafting_performed(InputAction.CallbackContext obj)
    {
        onCrafting?.Invoke();
    }

    private void Stats_performed(InputAction.CallbackContext obj)
    {
        onStats?.Invoke();
    }

    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        onInventory?.Invoke();
    }

    private void Escape_performed(InputAction.CallbackContext obj)
    {
        onEscaped?.Invoke();
    }

    private void Equipment_performed(InputAction.CallbackContext obj)
    {
        onEquipment?.Invoke();
    }

    private void Use_Performed(InputAction.CallbackContext obj)
    {
        onUsed?.Invoke();
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        onMoved?.Invoke(MoveVector);
    }
}
