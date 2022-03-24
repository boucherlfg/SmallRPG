using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraderPanel : PanelWrapper
{
    public static TraderService service;
    public override bool ExcludeFromPause => false;
    public override bool ExitableByEscape => true;

    [SerializeField]
    private Transform playerContainer;
    [SerializeField]
    private Transform traderContainer;
    [SerializeField]
    private GameObject salesMenuElementPrefab;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private TMP_Text valueIndicator;

    private TraderAgent trader;
    public TraderAgent Trader
    {
        get => trader;
        set
        {
            trader = value;
            service = new TraderService(value);
            service.OnChange += Refresh;
            Refresh();
        }
    }
    public void Refresh()
    {
        var sign = Mathf.Sign(service.Value);
        valueIndicator.text = (sign > 0 ? "+" : "") + service.Value;
        valueIndicator.color = sign < 0 ? Color.red : sign > 0 ? Color.green : Color.gray;
        confirmButton.interactable = sign >= 0;

        foreach (Transform child in playerContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in traderContainer)
        {
            Destroy(child.gameObject);
        }
        service.Player.ForEachDistinct(item =>
        {
            var obj = Instantiate(salesMenuElementPrefab, playerContainer);
            var comp = obj.GetComponent<TradeMenuItemScript>();
            comp.Item = item;
            comp.Count = service.Player.Count(x => x == item);
            comp.Color = Color.white;
            comp.tradeAction = () => service.AddOffer(item);
        });
        service.Demand.ForEachDistinct(item =>
        {
            var obj = Instantiate(salesMenuElementPrefab, playerContainer);
            var comp = obj.GetComponent<TradeMenuItemScript>();
            comp.Item = item;
            comp.Count = service.Demand.Count(x => x == item);
            comp.Color = Color.gray;
            comp.tradeAction = () => service.RemoveDemand(item);
        });
        service.Trader.ForEachDistinct(item =>
        {
            var obj = Instantiate(salesMenuElementPrefab, traderContainer);
            var comp = obj.GetComponent<TradeMenuItemScript>();
            comp.Item = item;
            comp.Count = service.Trader.Count(x => x == item);
            comp.Color = Color.white;
            comp.tradeAction = () => service.AddDemand(item);
        });
        service.Offer.ForEachDistinct(item =>
        {
            var obj = Instantiate(salesMenuElementPrefab, traderContainer);
            var comp = obj.GetComponent<TradeMenuItemScript>();
            comp.Item = item;
            comp.Count = service.Offer.Count(x => x == item);
            comp.Color = Color.gray;
            comp.tradeAction = () => service.RemoveOffer(item);
        });
    }

    public void Confirm()
    {
        if (service.Value < 0) return;
        service.Confirm();
        AudioManager.PlayAsSound("loot");
    }
}
