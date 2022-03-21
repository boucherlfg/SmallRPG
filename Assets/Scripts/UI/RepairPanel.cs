using System.Collections;
using UnityEngine;

public class RepairPanel : PanelWrapper
{
    public override bool ExitableByEscape => true;
    public override bool ExcludeFromPause => false;
    [SerializeField]
    private Transform container;

    public GameObject repairMenuItemPrefab;

    void Awake()
    {
        StartCoroutine(WaitForNull());

        IEnumerator WaitForNull()
        {
            yield return null;
            ActiveStateChanged += Refresh;
            Refresh();
        }
    }

    public void Refresh()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        var usedItems = DataModel.Inventory.Items.FindAll(x => x.durability < x.maxDurability);

        usedItems.ForEach(item =>
        {
            GameObject obj = Instantiate(repairMenuItemPrefab, container);
            RepairMenuItemScript comp = obj.GetComponent<RepairMenuItemScript>();
            comp.Item = item;
        });
    }
}