using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TooltipWhenOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        UIManager.Tooltip.Active = true;
        UIManager.Tooltip.Text = tooltipText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        UIManager.Tooltip.Active = false;
    }
}