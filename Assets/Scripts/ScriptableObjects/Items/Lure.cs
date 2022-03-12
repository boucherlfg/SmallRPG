
using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Items/Lure")]
public class Lure : Placeable
{
    public override Agent AgentFactory => new LureAgent(this);
    public LureType type;
    public enum LureType
    {
        Bandit = 0,
        Carnivore = 1,
        Herbivore = 2,
    }
    
}