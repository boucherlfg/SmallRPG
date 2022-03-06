using UnityEngine;

public class PanelWrapper : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    public delegate void OnActiveStateChange();
    public event OnActiveStateChange ActiveStateChanged;
    public bool Active
    {
        get => menu.activeSelf;
        set
        {
            menu.SetActive(value);
            ActiveStateChanged?.Invoke();
        }
    }
}