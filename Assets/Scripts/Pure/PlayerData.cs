using System.Collections.Generic;
using System.Linq;

public class PlayerData : CSharpSingleton<PlayerData>
{
    public delegate void OnChange();
    public event OnChange ChangedPrivate;
    public static event OnChange Changed
    {
        add => _instance.ChangedPrivate += value;
        remove => _instance.ChangedPrivate -= value;
    }

    private readonly Inventory items;
    private readonly Equipment equipment;
    public static Inventory Inventory => _instance.items;
    public static Equipment Equipment => _instance.equipment;

    #region stats
    private StatBlock current;
    public static StatBlock StatBlock => _instance.current;
    public static int Life
    {
        get => (int)_instance.current.life;
        set 
        {
            if (value < 0) return;
            _instance.current.life = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    public static int Mana
    {
        get => (int)_instance.current.mana;
        set 
        {
            if (value < 0) return;
            _instance.current.mana = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    public static int Attack
    {
        get => (int)_instance.current.attack;
        set 
        {
            if (value < 0) return;
            _instance.current.attack = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    public static int Defense
    {
        get => (int)_instance.current.defense;
        set 
        {
            if (value < 0) return;
            _instance.current.defense = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    public static int Evasion
    {
        get => (int)_instance.current.evasion;
        set 
        {
            if (value < 1) return;
            _instance.current.evasion = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    public static int Precision
    {
        get => (int)_instance.current.precision;
        set 
        {
            if (value < 1) return;
            _instance.current.precision = value;
            _instance.ChangedPrivate?.Invoke();
        }
    }
    #endregion
    
    public PlayerData()
    {
        items = new Inventory();
        equipment = new Equipment();

        current.life = 10;
        current.mana = 3;

        items.Changed += () => ChangedPrivate?.Invoke();
        equipment.onChanged += () => ChangedPrivate?.Invoke();
    }

    public static void Reset()
    {
        _instance = new PlayerData();
    }
}


