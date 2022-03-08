using System.Collections.Generic;

public class Equipment
{
    public delegate void OnChange();
    public event OnChange onChanged;

    public void Init()
    {
        equipment.Clear();
        StartingGear.Equipment.ForEach(e => equipment[e.equipType] = e.name);
        tool = StartingGear.Tool;
    }
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
    public StatBlock TotalBonus
    {
        get
        {
            StatBlock stat = new StatBlock();
            foreach (EquipType eq in System.Enum.GetValues(typeof(EquipType)))
            {
                stat += GetBuff(eq);
            }
            if (tool != null && tool is Tool)
            {
                stat += (tool as Tool).stats;
            }
            return stat;
        }
    }
    public void Equip(string equipment, EquipType type)
    {
        Unequip(type);
        this[type] = equipment;

        DataModel.Inventory.Delete(this[type]);
        onChanged?.Invoke();
    }
    public void Unequip(EquipType type)
    {
        if (this[type] != null)
        {
            DataModel.Inventory.Add(this[type]);
        }
        equipment[type] = null;
        onChanged?.Invoke();
    }
}
