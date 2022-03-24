using System;
using System.Collections.Generic;
using System.Linq;

public class TraderService
{
    private static float persistentValue = 0;
    public event Action OnChange;
    private readonly TraderAgent trader;
    private List<string> offer;
    private List<string> demand;
    float multiplier;

    public string[] Offer => offer.OrderBy(x => x).ToArray();
    public string[] Demand => demand.OrderBy(x => x).ToArray();
    public IEnumerable<string> Player => DataModel.Inventory.Items.Select(x => x.name).Subtract(offer).OrderBy(x => x);
    public IEnumerable<string> Trader => trader.sales.Subtract(demand).OrderBy(x => x);
    public float Value => persistentValue + UnityEngine.Mathf.Round(offer.Sum(x => (1/multiplier) * Codex.Items[x].value) - demand.Sum(x => multiplier * Codex.Items[x].value));

    public TraderService(TraderAgent trader)
    {
        this.trader = trader;
        this.offer = new List<string>();
        this.demand = new List<string>();
        this.multiplier = UnityEngine.Random.value + 1;
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
        persistentValue = (((int)(Value / 10)) * 10);
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