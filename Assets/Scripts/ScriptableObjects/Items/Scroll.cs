using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Scroll")]
public class Scroll : Item
{
    [ScrollsBehaviours.ScrollBehaviourEnum]
    public string spell;
    public override void Use()
    {
        ScrollsBehaviours.ExecuteAsProjectile(spell, Game.Instance.Player.position, Game.Instance.Player.Orientation);
        DataModel.Inventory.Delete(this.name);
    }
}