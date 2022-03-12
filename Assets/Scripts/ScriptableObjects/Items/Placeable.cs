[InventoryCategory]
public abstract class Placeable : Item
{
    public abstract Agent AgentFactory { get; }
    public override sealed void Use()
    {
        Player player = Game.Instance.Player;
        if (Game.Instance.Agents.Exists(x => x.position == player.position + player.Orientation))
        {
            UIManager.Notifications.CreateNotification("you can't place something there");
            return;
        }

        var agent = AgentFactory;
        agent.position = player.position + player.Orientation;
        Game.Instance.Create(agent);
        DataModel.Inventory.Delete(name);
        DataModel.Inventory.Delete("you placed " + visibleName);
    }
}