using static UnityEngine.GraphicsBuffer;

public class Buff
{
    private int counter = 0;
    System.Func<Agent> getter;
    private StatBlock start, overtime, end;
    private int duration;

    public bool IsOver => counter >= duration;
    public Buff(System.Func<Agent> getter, StatBlock start = default, StatBlock overtime = default, StatBlock end = default, int duration = 0)
    {
        this.getter = getter;
        this.start = start;
        this.overtime = overtime;
        this.end = end;
        this.duration = duration;
    }

    public void Start()
    {
        if (!Game.Instance.Agents.Contains(getter())) return;
        var obj = getter() as IStats;
        obj.Stats += start;
        if (start.life < 0)
        {
            DisplayManager.Instance.CreateDamageText(-(int)start.life, getter().position);
        }
    }
    public void Update()
    {
        if (!Game.Instance.Agents.Contains(getter())) return;
        var obj = getter() as IStats;
        obj.Stats += overtime;
        if (overtime.life < 0)
        {
            DisplayManager.Instance.CreateDamageText(-(int)start.life, getter().position);
        }
        counter++;
    }
    public void End()
    {
        if (!Game.Instance.Agents.Contains(getter())) return;
        var obj = getter() as IStats;
        obj.Stats += end;
        if (end.life < 0)
        {
            DisplayManager.Instance.CreateDamageText(-(int)start.life, getter().position);
        }
    }
}