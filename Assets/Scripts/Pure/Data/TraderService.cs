using System;
using System.Collections.Generic;
using System.Linq;

public class TraderService
{
    public event Action OnChange;
    private readonly TraderAgent trader;
    private List<string> offer;
    private List<string> demand;

    public string[] Offer => offer.ToArray();
    public string[] Demand => demand.ToArray();
    public IEnumerable<string> Player => DataModel.Inventory.Items.Select(x => x.name).Except(offer);
    public IEnumerable<string> Trader => trader.sales.Subtract(demand);
    public float Value => offer.Sum(x => Codex.Items[x].value) - demand.Sum(x => Codex.Items[x].value);

    public TraderService(TraderAgent trader)
    {
        this.trader = trader;
        this.offer = new List<string>();
        this.demand = new List<string>();
    }

    ~TraderService() 
    {
        OnChange = null;
    }

    public void AddDemand(string name)
    {
        demand.Add(name);
        OnChange?.Invoke();

    }
    public void RemoveDemand(string name)
    {
        demand.Remove(name);
        OnChange?.Invoke();
    }
    public void AddOffer(string name)
    {
        offer.Add(name);
        OnChange?.Invoke();
    }
    public void RemoveOffer(string name)
    {
        offer.Remove(name);
        OnChange?.Invoke();
    }

    internal void Confirm()
    {
        offer.ForEach(x =>
        {
            DataModel.Inventory.Delete(x);
            trader.sales.Add(x);
        });
        demand.ForEach(x =>
        {
            trader.sales.Remove(x);
            DataModel.Inventory.Add(x);
        });
        offer.Clear();
        demand.Clear();
        OnChange?.Invoke();
    }
}