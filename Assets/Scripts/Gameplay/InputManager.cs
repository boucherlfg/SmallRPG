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

    public bool Active
    {
        get => controls.Player.enabled;
        set
        {
            if (value) controls.Player.Disable();
            else controls.Player.Enable();
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
