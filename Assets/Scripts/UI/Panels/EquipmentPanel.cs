using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : PanelWrapper
{
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform body;
    [SerializeField]
    private Transform leg;
    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private Transform necklace;
    [SerializeField]
    private Transform ring;
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
        DestroyChildren(head);
        DestroyChildren(body);
        DestroyChildren(leg);
        DestroyChildren(weapon);
        DestroyChildren(necklace);
        DestroyChildren(ring);

        InstantiateEquipment(EquipType.Head, head);
        InstantiateEquipment(EquipType.Body, body);
        InstantiateEquipment(EquipType.Leg, leg);
        InstantiateEquipment(EquipType.Necklace, necklace);
        InstantiateEquipment(EquipType.Ring, ring);
        InstantiateEquipment(EquipType.Weapon, weapon);
    }

    private void InstantiateEquipment(EquipType type, Transform container)
    {
        var item = DataModel.Equipment[type];
        if (item == null) return;
        var obj = Instantiate(equipmentMenuItemPrefab, container);
        var comp = obj.GetComponent<EquipmentMenuElement>();
        comp.Item = item;
    }
    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
