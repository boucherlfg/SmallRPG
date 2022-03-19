using UnityEngine;
using UnityEngine.Tilemaps;

public class FurnitureAgent : Agent, IActivatable, ICollision, IDrawable, IEndable
{
    private int durability;
    public Furniture item;
    public Tile CurrentTile => GameHelper.CreateTile(item.sprite);
    public FurnitureAgent()
    {
        var breakables = Codex.Items.FindAll(x => x is Furniture);
        item = GameHelper.DistributedRandom(breakables) as Furniture;
        this.durability = item.durability;
    }
    public FurnitureAgent(Furniture item)
    {
        this.item = item;
        this.durability = item.durability;
    }
    public void Activate(IMovable source)
    {
        AudioManager.PlayAsSound("use");
        this.durability--;
        if (this.durability <= 0)
        {
            Game.Instance.Destroy(this);
        }
    }
    public void End()
    {
        AudioManager.PlayAsSound("empty");
        var recipe = Codex.Recipes.Find(x => x.output.Contains(item));
        if (!recipe) return;
        var choice = GameHelper.DistributedRandom(recipe.input);
        Game.Instance.Create(new FloorItem(choice) { position = position });
    }
}