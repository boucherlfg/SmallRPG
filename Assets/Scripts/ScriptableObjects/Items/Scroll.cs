using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Scroll")]
public class Scroll : Item
{
    [SpellsBehaviours.SpellBehaviourEnum]
    public string spell;
    public override void Use()
    {
        SpellsBehaviours.ExecuteAsProjectile(spell, Game.Instance.Player.position, Game.Instance.Player.Orientation);
        DataModel.Inventory.Delete(this.name);
    }
}