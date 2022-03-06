using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifManager : MonoSingleton<NotifManager>
{
    public GameObject notifPrefab;
    public Transform container;
    public static void CreateNotification(string text)
    {
        var obj = Instantiate(_instance.notifPrefab, _instance.container);
        obj.GetComponent<NotificationScript>().Text = text;
    }
}
