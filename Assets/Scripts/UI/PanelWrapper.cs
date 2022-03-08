using UnityEngine;

public class PanelWrapper : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public delegate void OnActiveStateChange();
    public event OnActiveStateChange ActiveStateChanged;
    public bool Active
    {
        get => menu.activeSelf && menu.transform.GetSiblingIndex() >= menu.transform.parent.childCount - 1;
        set
        {
            menu.SetActive(value);
            if (menu.activeSelf)
            {
                menu.transform.SetAsLastSibling();
            }
            else
            {
                menu.transform.SetAsFirstSibling();
            }
            ActiveStateChanged?.Invoke();
        }
    }
    public void Toggle()
    {
        Active = !Active;
    }
}