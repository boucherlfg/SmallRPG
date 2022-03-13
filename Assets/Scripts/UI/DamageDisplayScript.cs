using TMPro;
using UnityEngine;

public class DamageDisplayScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        label.color -= new Color(0, 0, 0, Time.deltaTime);
        if (label.color.a <= 0) Destroy(gameObject);
    }
    public int Damage
    {
        get => int.Parse(label.text);
        set => label.text = value.ToString();
    }
}