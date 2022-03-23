using System.Collections.Generic;
using System.Linq;

public class TraderService
{
    private readonly TraderAgent trader;
    public List<string> offer;
    public List<string> demand;

    public List<string> Player => DataModel.Inventory.Items.Transform(x => x.name).Subtract(offer);
    public List<string> Trader => trader.sales.Subtract(demand);
    public float Value => offer.Sum(x => Codex.Items[x].value) - demand.Sum(x => Codex.Items[x].value);

    public TraderService(TraderAgent trader)
    {
        this.trader = trader;
        this.offer = new List<string>();
        this.demand = new List<string>();
    }
}