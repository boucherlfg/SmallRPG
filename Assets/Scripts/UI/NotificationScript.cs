using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    public string Text
    {
        get => text.text;
        set => text.text = value;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    sustain -= Time.deltaTime;
    //    if (sustain > 0) return;

    //    var color = text.color;
    //    color.a = alpha;
    //    text.color = color;

    //    alpha -= Time.deltaTime * increment;
    //    fadeOut -= Time.deltaTime;
    //    if (fadeOut <= 0) Destroy(gameObject);
    //}
}
