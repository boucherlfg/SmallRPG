using System.Collections.Generic;
using System.Linq;

public class PlayerData : CSharpSingleton<PlayerData>
{
    public delegate void OnChange();
    public event OnChange onChanged;
    public static event OnChange Changed
    {
        add => _instance.onChanged += value;
        remove => _instance.onChanged -= value;
    }

    private Inventory items;
    private Equipment equipment;
    public static Inventory Inventory => _instance.items;
    public static Equipment Equipment => _instance.equipment;

    #region stats
    private int maxLife;
    private int maxMana;
    private int maxAttack;
    private int maxDefense;
    private int maxEvasion;
    private int maxPrecision;
    private int life;
    private int mana;
    private int attack;
    private int defense;
    private int evasion;
    private int precision;

    public static int MaxLife
    {
        get => _instance.maxLife;
        set
        {
            _instance.maxLife = value;
            Life = Life > MaxLife ? MaxLife : Life;
            _instance.onChanged?.Invoke();
        }
    }
    public static int MaxMana
    {
        get => _instance.maxMana;
        set
        {
            _instance.maxMana = value;
            Mana = Mana > MaxMana ? MaxMana : Mana;
            _instance.onChanged?.Invoke();
        }
    }
    public static int MaxAttack
    {
        get => _instance.maxAttack;
        set
        {
            _instance.maxAttack = value;
            Attack = Attack > MaxAttack ? MaxAttack : Attack;
            _instance.onChanged?.Invoke();
        }
    }
    public static int MaxDefense
    {
        get => _instance.maxDefense;
        set
        {
            _instance.maxDefense = value;
            Defense = Defense > MaxDefense ? MaxDefense : Defense;
            _instance.onChanged?.Invoke();
        }
    }
    public static int MaxEvasion
    {
        get => _instance.maxEvasion;
        set
        {
            _instance.maxEvasion = value;
            Evasion = Evasion > MaxEvasion ? MaxEvasion : Evasion;
            _instance.onChanged?.Invoke();
        }
    }
    public static int MaxPrecision
    {
        get => _instance.maxPrecision;
        set 
        {
            _instance.maxPrecision = value;
            Precision = Precision > MaxPrecision ? MaxPrecision : Precision;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Life
    {
        get => _instance.life;
        set 
        {
            if (value < 0) return;
            _instance.life = value > MaxLife ? MaxLife : value;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Mana
    {
        get => _instance.mana;
        set 
        {
            if (value < 0) return;
            _instance.mana = value > MaxMana ? MaxMana : value;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Attack
    {
        get => _instance.attack;
        set 
        {
            if (value < 0) return;
            _instance.attack = value > MaxAttack ? MaxAttack : value;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Defense
    {
        get => _instance.defense;
        set 
        {
            if (value < 0) return;
            _instance.defense = value > MaxDefense ? MaxDefense : value;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Evasion
    {
        get => _instance.evasion;
        set 
        {
            if (value < 1) return;
            _instance.evasion = value > MaxEvasion ? MaxEvasion : value;
            _instance.onChanged?.Invoke();
        }
    }
    public static int Precision
    {
        get => _instance.precision;
        set 
        {
            if (value < 1) return;
            _instance.precision = value > MaxPrecision ? MaxPrecision : value;
            _instance.onChanged?.Invoke();
        }
    }
    #endregion
    
    public PlayerData()
    {
        items = new Inventory();
        equipment = new Equipment();

        life = maxLife = 10;
        mana = maxMana = 3;
        attack = maxAttack = 1;
        defense = maxDefense = 0;
        evasion = maxEvasion = 2;
        precision = maxPrecision = 1;

        items.onChanged += () => onChanged?.Invoke();
        equipment.onChanged += () => onChanged?.Invoke();
    }

    public static void Reset()
    {
        _instance = new PlayerData();
    }
}

public class Inventory
{
    public List<string> Items => items;
    private List<string> items;
    public delegate void OnChange();
    public event OnChange onChanged;

    public Inventory()
    {
        items = new List<string>();
    }

    public void Add(string item)
    {
        items.Add(item);
        onChanged?.Invoke();
    }
    public void Delete(string item)
    {
        items.Remove(item);
        onChanged?.Invoke();
}
    public int HowMany(string item)
    {
        return items.Count(x => x == item);
    }
    public void ForEachDistinct(TypedAction<string> action)
    {
        foreach (var item in items.Distinct().OrderBy(x => x))
        {
            action(item);
        }
    }
    
}
public class Equipment
{
    public delegate void OnChange();
    public event OnChange onChanged;

    public Equipment()
    {
        equipment = new Dictionary<EquipType, string>();
    }
    public string this[EquipType type]
    {
        get
        {
            if (!equipment.ContainsKey(type))
            {
                equipment[type] = null;
            }
            return equipment[type];
        }
        set => equipment[type] = value;
    }
    private Dictionary<EquipType, string> equipment;

    private StatBlock GetBuff(EquipType type)
    {
        var item = this[type];
        if (item == null) return new StatBlock()
        {
            life = 0,
            mana = 0,
            attack = 0,
            defense = 0,
            precision = 0,
            evasion = 0
        };
        return (ItemsCodex.Instance[item] as Equipable).buff;
    }
    public StatBlock TotalBonus
    {
        get
        {
            StatBlock stat = new StatBlock();
            foreach (EquipType eq in System.Enum.GetValues(typeof(EquipType))) {
                stat += GetBuff(eq);
            }
            return stat;
        }
    }
    public void Equip(string equipment, EquipType type)
    {
        Unequip(type);
        this[type] = equipment;
        var stat = GetBuff(type);
        PlayerData.MaxLife += stat.life;
        PlayerData.MaxMana += stat.mana;
        PlayerData.MaxAttack += stat.attack;
        PlayerData.MaxDefense += stat.defense;
        PlayerData.MaxPrecision += stat.precision;
        PlayerData.MaxEvasion += stat.evasion;

        PlayerData.Life += stat.life;
        PlayerData.Mana += stat.mana;
        PlayerData.Attack += stat.attack;
        PlayerData.Defense += stat.defense;
        PlayerData.Precision += stat.precision;
        PlayerData.Evasion += stat.evasion;
        
        PlayerData.Inventory.Delete(this[type]);
        onChanged?.Invoke();
    }
    public void Unequip(EquipType type)
    {
        if (this[type] != null)
        {
            PlayerData.Inventory.Add(this[type]);
            var stat = GetBuff(type);

            PlayerData.MaxLife -= stat.life;
            PlayerData.MaxMana -= stat.mana;
            PlayerData.MaxAttack -= stat.attack;
            PlayerData.MaxDefense -= stat.defense;
            PlayerData.MaxPrecision -= stat.precision;
            PlayerData.MaxEvasion -= stat.evasion;
        }
        equipment[type] = null;
    }
}