using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderPanel : PanelWrapper
{
    public static TraderService service;
    public override bool ExcludeFromPause => false;
    public override bool ExitableByEscape => true;

    [SerializeField]
    private Transform playerContainer;
    [SerializeField]
    private Transform traderContainer;

    private TraderAgent trader;
    public TraderAgent Trader
    {
        get => trader;
        set
        {
            trader = value;
            service = new TraderService(value);
            Refresh();
        }
    }
    public void Refresh()
    {
        service.Player.ForEachDistinct(item =>
        {

        });
        service.demand.ForEachDistinct(item =>
        {

        });
        service.Trader.ForEachDistinct(item =>
        {

        });
        service.offer.ForEachDistinct(item =>
        {

        });
    }
}
