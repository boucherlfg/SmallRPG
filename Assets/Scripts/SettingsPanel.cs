using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PanelWrapper
{
    void Start()
    {
        StartCoroutine(InitWhenReady());

        IEnumerator InitWhenReady()
        {
            yield return new WaitUntil(() => AudioManager.HasInstance);
            var volumeSlider = menu.GetComponentsInChildren<Slider>().FirstOrDefault(x => x.name == "VolumeSlider");
            volumeSlider.value = AudioManager.Volume;
        }
    }
}
