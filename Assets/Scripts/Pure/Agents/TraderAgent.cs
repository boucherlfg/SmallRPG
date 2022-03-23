using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class TraderAgent : Agent, ICollision, IActivatable, IDrawable
{
    public Tile CurrentTile => DisplayManager.Instance["trader"];
    public List<string> sales;
    public TraderAgent()
    {
        sales = new List<string>();
        float value = 5 * Game.Instance.LevelBudget;

        while (value > 0)
        {
            var choice = GameHelper.DistributedRandom(Codex.Items);
            sales.Add(choice.name);
            value -= choice.value;
        }
    }

    public void Activate(IMovable source)
    {
        if (!(source is Player)) return;

        UIManager.Trader.Toggle();
        UIManager.Trader.Trader = this;
    }
}