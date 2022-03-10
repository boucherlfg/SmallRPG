using System.Collections.Generic;

public class Equipment
{
    private List<Buff> buffs;
    public delegate void OnChange();
    public event OnChange onChanged;

    public void Init()
    {
        equipment.Clear();
        buffs.Clear();
        StartingGear.Equipment.ForEach(e => Equip(e.name, e.equipType));
        Tool = StartingGear.Tool;
    }
    public Equipment()
    {
        equipment = new Dictionary<EquipType, string>();
        buffs = new List<Buff>();
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

    private Item tool;
    public void ConsumeTool() => tool = null;
    public Item Tool
    {
        get => tool;
        set
        {
            if(tool != null) DataModel.Inventory.Add(tool.name);
            if(value != null) DataModel.Inventory.Delete(value.name);
            
            tool = value;
            onChanged?.Invoke();
        }
    }

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
        return (Codex.Items[item] as Equipable).buff;
    }
    public void Equip(string equipment, EquipType type)
    {
        Unequip(type);
        this[type] = equipment;

        DataModel.Inventory.Delete(this[type]);
        DataModel.StatBlock += GetBuff(type);
        onChanged?.Invoke();
    }
    public void Unequip(EquipType type)
    {
        DataModel.StatBlock -= GetBuff(type);
        if (this[type] != null)
        {
            DataModel.Inventory.Add(this[type]);
        }
        equipment[type] = null;
        onChanged?.Invoke();
    }
}
