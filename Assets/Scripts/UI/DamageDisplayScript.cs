using TMPro;
using UnityEngine;

public class DamageDisplayScript : MonoBehaviour
{
    private TMP_Text label;
    private int damage;
    public int Damage
    {
        get => damage;
        set => label.text = value.ToString();
    }
}