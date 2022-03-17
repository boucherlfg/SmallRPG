using UnityEngine;

public class PanelWrapper : MonoBehaviour
{
    public virtual bool ExcludeFromPause => false;
    public static event VoidAction AnyActiveStateChanged;
    [SerializeField]
    protected GameObject menu;

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
            AnyActiveStateChanged?.Invoke();
        }
    }

    public virtual bool ExitableByEscape => false;

    public void Toggle()
    {
        Active = !Active;
        UISound();
    }
    private void UISound()
    {
        string tag = Active ? "openMenu" : "closeMenu";
        AudioManager.PlayAsSound(tag);
    }
}