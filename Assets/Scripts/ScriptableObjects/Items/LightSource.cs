using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Light Source")]
public class LightSource : Placeable
{
    public float range = 5;
    public override Agent AgentFactory => new LightSourceAgent() { lightSource = this };
}