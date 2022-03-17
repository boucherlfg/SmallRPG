using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : PanelWrapper
{
    [SerializeField]
    private Transform container;
    [SerializeField]
    private GameObject equipmentMenuItemPrefab;
    public override bool ExitableByEscape => true;
    public void Start()
    {
        ActiveStateChanged += Refresh;
        StartCoroutine(HookWhenReady());
        IEnumerator HookWhenReady()
        {
            yield return null;
            DataModel.Equipment.onChanged += Refresh;
        }
        Refresh();
    }
    public void Refresh()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (EquipType e in System.Enum.GetValues(typeof(EquipType)))
        {
            var itemID = DataModel.Equipment[e];
            if (itemID == null) continue;

            var obj = Instantiate(equipmentMenuItemPrefab, container);
            var comp = obj.GetComponent<EquipmentMenuElement>();
            comp.Item = itemID;
        }
        if (DataModel.Equipment.Tool == null) return;
        var tool = Instantiate(equipmentMenuItemPrefab, container);
        var c = tool.GetComponent<EquipmentMenuElement>();

        c.Item = DataModel.Equipment.Tool.name;
    }
}
