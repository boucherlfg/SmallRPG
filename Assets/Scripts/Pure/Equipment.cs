using System.Collections.Generic;
using System.Linq;
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
                return null;
            }
            return equipment[type];
        }
        set => equipment[type] = value;
    }
    private Dictionary<EquipType, string> equipment;

    public void ConsumeEquipment(EquipType type)
    {
        var name = this[type];
        var buff = GetBuff(type);
        equipment[type] = null;
        DataModel.StatBlock -= buff;

        if (name != null)
        {
            DataModel.Inventory.Delete(name);
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

    internal bool HasItem(string item, out EquipType eqType)
    {
        eqType = EquipType.Weapon;
        foreach (EquipType type in System.Enum.GetValues(typeof(EquipType)))
        {
            if (this[type] == item) return true;
        }
        return false;
    }

    public void Equip(string equipment, EquipType type)
    {
        Unequip(type);

        this[type] = equipment;
        DataModel.StatBlock += GetBuff(type);
        onChanged?.Invoke();
    }
    public void Unequip(EquipType type)
    {
        DataModel.StatBlock -= GetBuff(type);
        equipment[type] = null;
        onChanged?.Invoke();
    }

    public void Damage(params EquipType[] types)
    {
        if (types.Length == 0) types = System.Enum.GetValues(typeof(EquipType)).OfType<EquipType>().ToArray();
        foreach (EquipType type in types)
        {
            var equip = this[type];
            if (equip == null) continue;
            DataModel.Inventory.Damage(equip);
        }
    }
}
