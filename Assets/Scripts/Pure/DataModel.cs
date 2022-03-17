using System;
using System.Collections.Generic;
using System.Linq;

public class DataModel : CSharpSingleton<DataModel>
{
    public delegate void OnChange();
    public event OnChange ChangedPrivate;
    public static event OnChange Changed
    {
        add => _instance.ChangedPrivate += value;
        remove => _instance.ChangedPrivate -= value;
    }

    private List<string> logs;
    private readonly Inventory items;
    private readonly Equipment equipment;
    private readonly ActiveBuffs activeBuffs;
    public static Inventory Inventory => _instance.items;
    public static Equipment Equipment => _instance.equipment;
    public static ActiveBuffs ActiveBuffs => _instance.activeBuffs;
    public static List<string> Logs => _instance.logs;

    #region stats
    private StatBlock stats;
    public static StatBlock StatBlock
    {
        get => _instance.stats;
        set
        {
            _instance.stats = Adjust(value);
            _instance.ChangedPrivate?.Invoke();
        }
    }

    private static StatBlock Adjust(StatBlock value)
    {
        return new StatBlock()
        {
            life = value.life < 0 ? 0 : value.life > 20 ? 20 : value.life,
            mana = value.mana < 0 ? 0 : value.mana > 20 ? 20 : value.mana,
            attack = value.attack < 0 ? 0 : value.attack,
            defense = value.defense < 0 ? 0 : value.defense,
            evasion = value.evasion < 0 ? 0 : value.evasion,
            precision = value.precision < 0 ? 0 : value.precision,
        };
    }

    #endregion

    public DataModel()
    {
        logs = new List<string>();
        stats = StartingGear.Stats;
        activeBuffs = new ActiveBuffs();
        items = new Inventory();
        equipment = new Equipment();

        items.Changed += () => ChangedPrivate?.Invoke();
        equipment.onChanged += () => ChangedPrivate?.Invoke();
    }

    public static void Reset()
    {
        _instance.stats = new StatBlock();
        _instance.stats = StartingGear.Stats;
        _instance.equipment.Init();
        _instance.items.Init();
        _instance.ChangedPrivate?.Invoke();
    }
}


