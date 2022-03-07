using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifManager : MonoSingleton<NotifManager>
{
    public GameObject notifPrefab;
    public Transform container;
    static Queue<GameObject> notifs = new Queue<GameObject>();
    public static void CreateNotification(string text)
    {
        var obj = Instantiate(_instance.notifPrefab, _instance.container);
        obj.GetComponent<NotificationScript>().Text = text;
        notifs.Enqueue(obj);
        while (notifs.Count > 5)
        {
            Destroy(notifs.Dequeue());
        }
    }
}
