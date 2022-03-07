using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    [SerializeField]
    private float sustain = 2;
    [SerializeField]
    private float fadeOut = 2;
    private float increment;
    private float alpha;
    [SerializeField]
    private TMP_Text text;

    public string Text
    {
        get => text.text;
        set => text.text = value;
    }
    void Start()
    {
        alpha = 1;
        increment = 1 / fadeOut;
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
