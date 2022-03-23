using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderPanel : PanelWrapper
{
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
            Refresh();
        }
    }
    public void Refresh()
    {
        
    }
}
