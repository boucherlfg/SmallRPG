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
    }
    public void Update()
    {
        if (!Game.Instance.Agents.Contains(getter())) return;
        var obj = getter() as IStats;
        obj.Stats += overtime;
        counter++;
    }
    public void End()
    {
        if (!Game.Instance.Agents.Contains(getter())) return;
        var obj = getter() as IStats;
        obj.Stats += end;
    }
}