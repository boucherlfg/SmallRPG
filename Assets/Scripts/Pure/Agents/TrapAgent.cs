using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class TrapAgent : Agent, IActivatable, IDrawable, IUsableAgent
{
    public Trap data;
    const string trap_tag = "trap";
    const string basicTrap = nameof(basicTrap);
    public Tile CurrentTile => DisplayManager.Instance[trap_tag];

    public TrapAgent() 
    {
        data = Codex.Items[basicTrap] as Trap;
    }
    public TrapAgent(Trap data)
    {
        this.data = data;
    }
    
    public void Activate(IMovable source)
    {
        if (!(source is IStats)) return;

        if (source is Player)
        {
            AudioManager.PlayAsSound("use");
            UIManager.Notifications.CreateNotification("you just fell in a trap!");

        }
        var buff = new Buff(() => source as Agent, data.heal, data.regen, data.resolve, data.duration);
        DataModel.ActiveBuffs.Add(buff);
        Game.Instance.Destroy(this);
    }

    public void Use(Player user)
    {
        AudioManager.PlayAsSound("empty");
        Game.Instance.Destroy(this);
        UIManager.Notifications.CreateNotification("you just deactivated a trap");
    }
}