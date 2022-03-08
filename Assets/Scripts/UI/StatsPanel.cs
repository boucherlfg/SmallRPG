using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanel : PanelWrapper
{
    [SerializeField]
    private TMP_Text life;
    [SerializeField]
    private TMP_Text mana;
    [SerializeField]
    private TMP_Text attack;
    [SerializeField]
    private TMP_Text defense;
    [SerializeField]
    private TMP_Text precision;
    [SerializeField]
    private TMP_Text evasion;
    private StatBlock statBlock;
    void Start()
    {
        ActiveStateChanged += () => StatBlock = DataModel.StatBlock + DataModel.Equipment.TotalBonus;
    }
    public StatBlock StatBlock
    {
        get => statBlock;
        set
        {
            statBlock = value;
            life.text = $"life : {value.life}";
            mana.text = $"mana : {value.mana}";
            attack.text = $"attack : {value.attack}";
            defense.text = $"defense : {value.defense}";
            precision.text = $"precision : {value.precision}";
            evasion.text = $"evasion : {value.evasion}";
        }
    }

}
