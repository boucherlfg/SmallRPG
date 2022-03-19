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
        StartingGear.Equipment.ForEach(e => Equip(e, e.equipType));
    }
    public Equipment()
    {
        equipment = new Dictionary<EquipType, ItemState?>();
        buffs = new List<Buff>();
    }
    public ItemState? this[EquipType type]
    {
        get
        {
            if (!equipment.ContainsKey(type))
            {
                return null;
            }
            return equipment[type];
        }
        set => equipment[type] = value.Value;
    }
    private Dictionary<EquipType, ItemState?> equipment;

    public void ConsumeEquipment(EquipType type)
    {
        var buff = GetBuff(type);
        equipment[type] = null;
        DataModel.StatBlock -= buff;
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
        return (Codex.Items[item.Value.name] as Equipable).buff;
    }
    public void Equip(ItemState equipment, EquipType type)
    {
        Unequip(type);

        this[type] = equipment;

        DataModel.Inventory.Delete(this[type].Value.name);
        DataModel.StatBlock += GetBuff(type);
        onChanged?.Invoke();
    }
    public void Unequip(EquipType type)
    {
        DataModel.StatBlock -= GetBuff(type);
        if (this[type] != null)
        {
            DataModel.Inventory.Add(this[type].Value.name);
        }
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
            var equipNN = equip.Value;
            var info = Codex.Items[equipNN.name];
            equipNN.durability--;
            equipment[type] = equipNN;
            if (equipNN.durability <= 0)
            {
                ConsumeEquipment(type);
                UIManager.Notifications.CreateNotification($"your {info.visibleName} just broke");
            }
        }
    }
}
