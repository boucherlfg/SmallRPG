using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Furniture")]
public class Furniture : Placeable
{
    public override Agent AgentFactory => new FurnitureAgent(this);
   
}