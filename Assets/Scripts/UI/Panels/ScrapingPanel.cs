using System.Collections;
using UnityEngine;

public class ScrapingPanel : PanelWrapper
{
    [SerializeField]
    private GameObject scrapingMenuElementPrefab;
    [SerializeField]
    private Transform container;
    public override bool ExitableByEscape => true;
    public override bool ExcludeFromPause => false;
    void Awake()
    {
        StartCoroutine(WaitForNull());

        IEnumerator WaitForNull()
        {
            yield return null;
            ActiveStateChanged += Refresh;
        }
    }

    public void Refresh()
    {
        if (!Active) return;
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        DataModel.Inventory.ForEachDistinct(item =>
        {
            GameObject obj = Instantiate(scrapingMenuElementPrefab, container);
            var comp = obj.GetComponent<ScrapingMenuElementScript>();
            comp.Item = Codex.Items[item.name];
        });
    }
}