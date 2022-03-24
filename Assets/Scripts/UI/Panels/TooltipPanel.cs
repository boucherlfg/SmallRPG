using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipPanel : PanelWrapper
{
    [SerializeField]
    private TMP_Text label;
    public override bool ExcludeFromPause => true;
    public override bool ExitableByEscape => false;
    public string Text
    {
        get => label.text;
        set => label.text = value;
    }
}
