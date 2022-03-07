using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    private string text;
    private TMP_Text label;
    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TMP_Text>();
        StartCoroutine(HookAfter());
        IEnumerator HookAfter()
        {
            yield return null;
            PlayerData.Changed += () => text = $"life : {PlayerData.Life}";
        }
        text = $"life : {PlayerData.Life}";
    }

    private void Update()
    {
        label.text = text;
    }
}
