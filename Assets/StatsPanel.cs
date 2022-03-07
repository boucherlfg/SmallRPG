using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanel : PanelWrapper
{
    [SerializeField]
    private TMP_Text display;
    private StatBlock statBlock;
    void Start()
    {
        ActiveStateChanged += () => StatBlock = PlayerData.StatBlock + PlayerData.Equipment.TotalBonus;
    }
    public StatBlock StatBlock
    {
        get => statBlock;
        set
        {
            statBlock = value;
            display.text = $"life : {value.life}\n" +
                $"mana : {value.mana}\n" +
                $"attack : {value.attack}\n" +
                $"defense : {value.defense}\n" +
                $"precision : {value.precision}\n" +
                $"evasion : {value.evasion}";
        }
    }

}
