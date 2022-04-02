using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoWhenOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent toDo;
    public string tooltipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        if (tooltipText == null) return;
        UIManager.Tooltip.Active = true;
        UIManager.Tooltip.Text = tooltipText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        UIManager.Tooltip.Active = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        if (tooltipText == null) return;
        UIManager.Tooltip.Active = true;
        UIManager.Tooltip.Text = tooltipText;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey(SettingsPanel.tooltips_tag) && PlayerPrefs.GetInt(SettingsPanel.tooltips_tag) == 0) return;
        UIManager.Tooltip.Active = false;
    }
}
