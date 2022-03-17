using System.Collections;
using UnityEngine;

public class ScrapingPanel : PanelWrapper
{
    [SerializeField]
    private GameObject scrapingMenuElementPrefab;
    [SerializeField]
    private Transform container;
    public override bool ExitableByEscape => true;

    void Awake()
    {
        StartCoroutine(WaitForNull());

        IEnumerator WaitForNull()
        {
            yield return null;
            DataModel.Inventory.Changed += Refresh;
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

        DataModel.Inventory.ForEachDistinct(name =>
        {
            GameObject obj = Instantiate(scrapingMenuElementPrefab, container);
            var comp = obj.GetComponent<ScrapingMenuElementScript>();
            comp.Item = Codex.Items[name];
        });
    }
}