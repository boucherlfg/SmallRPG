using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPanel : PanelWrapper
{
    [SerializeField]
    private GameObject notificationElementPrefab;
    [SerializeField]
    private Transform container;
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

        DataModel.Logs.ForEach(log =>
        {
            var obj = Instantiate(notificationElementPrefab, container);
            var comp = obj.GetComponent<NotificationScript>();
            comp.Text = log;
        });
    }
}
