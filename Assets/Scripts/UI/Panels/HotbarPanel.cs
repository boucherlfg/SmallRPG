using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarPanel : PanelWrapper
{
    
    public HotbarMenuElementScript[] hotbarMenuElements;
    public override bool ExcludeFromPause => true;
    public override bool ExitableByEscape => false;

    void Start()
    {
        StartCoroutine(RefreshWhenReady());
        IEnumerator RefreshWhenReady()
        {
            yield return new WaitUntil(() => DataModel.Hotbar != null);
            DataModel.Hotbar.Changed += Refresh;
            DataModel.Inventory.Changed += Refresh;
            Refresh();
        }
    }
    public void Refresh()
    {
        for (int i = 1; i <= 5; i++)
        {
            var item = DataModel.Hotbar[i];
            if (DataModel.Inventory.HowMany(item) <= 0)
            {
                item = null;
            }
            hotbarMenuElements[i - 1].ItemName = item;
        }
    }
}
