using UnityEngine;

public class PanelWrapper : MonoBehaviour
{
    public virtual bool ExitableByEscape => true;
    public virtual bool ExcludeFromPause => false;
    public static event VoidAction AnyActiveStateChanged;
    [SerializeField]
    protected GameObject menu;

    public delegate void OnActiveStateChange();
    public event OnActiveStateChange ActiveStateChanged;
    public bool Active
    {
        get => menu.activeSelf;
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