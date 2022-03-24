using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotifManager : PanelWrapper
{
    public GameObject notifPrefab;
    Queue<GameObject> notifs = new Queue<GameObject>();
    public override bool ExcludeFromPause => true;
    public override bool ExitableByEscape => false;

    CanvasGroup group;
    float holdTimer = 0;
    float decayTimer = 0;
    const float holdTime = 3;
    const float decayTime = 3;
    public void CreateNotification(string text)
    {
        var obj = Instantiate(notifPrefab, menu.transform);
        obj.GetComponent<NotificationScript>().Text = text;
        DataModel.Logs.Add(text);
        notifs.Enqueue(obj);
        while (notifs.Count > 5)
        {
            Destroy(notifs.Dequeue());
        }
        ResetFadeTimers();
    }
    void Update()
    {
        group = group ? group : menu.GetComponent<CanvasGroup>();
        //just skur if the notifs are faded out
        if (decayTimer >= decayTime) return;

        holdTimer += Time.deltaTime;
        //start fading out only when hold is over
        if (holdTimer < holdTime) return;

        decayTimer += Time.deltaTime;
        group.alpha -= Time.deltaTime / decayTime;

    }
    public void ResetFadeTimers()
    {
        holdTimer = 0;
        decayTimer = 0;
        group.alpha = 1;
    }
}
