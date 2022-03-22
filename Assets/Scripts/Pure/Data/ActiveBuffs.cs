
using System.Collections.Generic;

public class ActiveBuffs
{
    private List<Buff> buffs;
    public ActiveBuffs()
    {
        buffs = new List<Buff>();
        Game.OnTurn += Game_OnTurn;
    }
    public void Add(Buff buff)
    {
        buffs.Add(buff);
        buff.Start();
    }
    void Game_OnTurn()
    {
        buffs.RemoveAll(x =>
        {
            if (x.IsOver) x.End();
            return x.IsOver;
        });
        buffs.ForEach(x => x.Update());
    }
}
