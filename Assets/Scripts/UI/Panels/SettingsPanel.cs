using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PanelWrapper
{
    public const string tooltips_tag = "tooltips";
    public const string volume_tag = "volume";
    public override bool ExitableByEscape => true;

    public GameObject volume;
    public GameObject tooltips;
    void Start()
    {
        StartCoroutine(InitWhenReady());

        IEnumerator InitWhenReady()
        {
            yield return new WaitUntil(() => AudioManager.HasInstance);
            var volumeSlider = volume.GetComponentInChildren<Slider>();
            volumeSlider.value = PlayerPrefs.HasKey(volume_tag) ? PlayerPrefs.GetFloat(volume_tag) : AudioManager.Source.volume;
            
            var tooltipToggle = tooltips.GetComponentInChildren<Toggle>();
            tooltipToggle.isOn = !PlayerPrefs.HasKey(tooltips_tag) || PlayerPrefs.GetInt(tooltips_tag) != 0;

            volumeSlider.onValueChanged.AddListener(ChangeVolume);
            tooltipToggle.onValueChanged.AddListener(ChangeTooltips);
        }
    }
    void ChangeVolume(float value) 
    {
        PlayerPrefs.SetFloat(volume_tag, value);
        AudioManager.Volume = value;
    }
    void ChangeTooltips(bool value) 
    {
        PlayerPrefs.SetInt(tooltips_tag, value ? 1 : 0);
    }
}
