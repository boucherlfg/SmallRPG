
using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Staff")]
public class Staff : Item
{
    public float manaCost = 1;
    [SpellsBehaviours.SpellBehaviourEnum]
    public string spell;
    public override void Use()
    {
        if (DataModel.StatBlock.mana < 1)
        {
            UIManager.Notifications.CreateNotification("You don't have enough mana");
            return;
        }
        var player = Game.Instance.Player;
        SpellsBehaviours.ExecuteAsProjectile(spell, player.position, player.Orientation);
        DataModel.StatBlock -= new StatBlock { mana = manaCost };
    }
}