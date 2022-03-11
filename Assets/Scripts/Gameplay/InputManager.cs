using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
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

    private event VoidAction onAttacked;
    public static event VoidAction Attacked
    {
        add => _instance.onAttacked += value;
        remove => _instance.onAttacked -= value;
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
        controls.Player.Attack.performed += Attack_performed;
        controls.Player.Escape.performed += Escape_performed;
        controls.Player.Inventory.performed += Inventory_performed;
        controls.Player.Equipment.performed += Equipment_performed;
        controls.Player.Crafting.performed += Crafting_performed;
        controls.Player.Stats.performed += Stats_performed;
        controls.Player.Use.performed += Use_performed;
        controls.Player.Logs.performed += Logs_performed;
    }

    private void Logs_performed(InputAction.CallbackContext obj)
    {
        onLogs?.Invoke();
    }

    private void Crafting_performed(InputAction.CallbackContext obj)
    {
        onCrafting?.Invoke();
    }

    private void Use_performed(InputAction.CallbackContext obj)
    {
        onUsed?.Invoke();
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

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        onAttacked?.Invoke();
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        var vect = obj.ReadValue<Vector2>();
        float len = vect.magnitude;
        if (len > 1) return;
        onMoved?.Invoke(Vector2Int.RoundToInt(vect));
    }
}
