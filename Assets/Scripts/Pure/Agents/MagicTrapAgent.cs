internal class MagicTrapAgent : TrapAgent
{
    private MagicTrap magicTrap;

    public MagicTrapAgent(MagicTrap magicTrap)
    {
        this.magicTrap = magicTrap;
    }
    public override void Activate(IMovable source)
    {
        if (!(source is IStats)) return;

        if (source is Player)
        {
            AudioManager.PlayAsSound("use");
            UIManager.Notifications.CreateNotification("you just fell in a trap!");

        }
        ScrollsBehaviours.ExecuteAsTrap(magicTrap.spell, position);
        Game.Instance.Destroy(this);
    }
}