using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogPanel : PanelWrapper
{
    [SerializeField]
    private GameObject notificationElementPrefab;
    [SerializeField]
    private Transform container;
    public override bool ExitableByEscape => true;
    void Start()
    {
        ActiveStateChanged += Refresh;
    }

    public void Refresh()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }


        for (int i = 0; i < 100 && i < DataModel.Logs.Count; i++) 
        {
            var log = DataModel.Logs[i];
            var obj = Instantiate(notificationElementPrefab, container);
            var comp = obj.GetComponent<NotificationScript>();
            comp.Text = log;
        }
    }
}
