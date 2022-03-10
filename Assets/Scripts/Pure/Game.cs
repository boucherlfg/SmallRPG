using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public class Game : CSharpSingleton<Game>
{
    int levelNumber = 1;
    const float level_expansion = 1.25f;
    private int turn = 0;
    public static int Turn => _instance.turn;
    public static event VoidAction OnTurn;
    #region [properties]
    public static Game Instance => _instance;

    private Player player;
    public Player Player
    {
        get
        {
            //constantly seek newest player
            var player = Agents.FirstOrDefault(x => x is Player);
            if (player == null) return this.player;
            this.player = player as Player;
            return this.player;
        }
    }
    public List<Agent> Agents => level != null && level.Agents != null ? level.Agents : new List<Agent>();
    public List<Room> Rooms => level.Rooms;

    public Level Level => level;

    private Level level;
    private List<Agent> toCreate = new List<Agent>();
    private List<Agent> toDestroy = new List<Agent>();
    
    #endregion

    public Game()
    {
        toCreate = new List<Agent>();
        toDestroy = new List<Agent>();
    }
    private void ApplyCollision(Agent agent)
    {
        if (!(agent is IMovable)) return;

        if (!(Agents.Find(col => col != agent && col is ICollision && col.position == agent.position) is ICollision)) return;
        agent.position -= (agent as IMovable).Orientation;
    }
    private void ApplyTrigger(Agent agent)
    {
        if (!(agent is IMovable)) return;

        var activatable = Agents.Find(other => other != agent && other is IActivatable && other.position == agent.position) as IActivatable;
        if (activatable == null) return;

        activatable.Activate(agent as IMovable);
    }

    public void Update()
    {
        OnTurn?.Invoke();
        Agents.FindAll(agent => agent is IUpdatable).ForEach(agent =>
        {
            (agent as IUpdatable).Update();
            if (agent is IMovable)
            {
                ApplyTrigger(agent);
                ApplyCollision(agent);
            }
        });

        toDestroy.ForEach(a =>
        {
            Agents.Remove(a);
            if (a is IEndable)
            {
                (a as IEndable).End();
            }
        });
        toDestroy.Clear();
        toCreate.ForEach(a => Agents.Add(a));
        toCreate.Clear();
    }
    public void Create(Agent agent)
    {
        toCreate.Add(agent);
    }
    public void Destroy(Agent agent)
    {
        toDestroy.Add(agent);
    }

    public void StartNewGame()
    {
        AudioManager.PlayAsMusic("theme_song");
        DataModel.Reset();
        Init(10);
        turn = 0;
        DisplayManager.Instance.Draw();
    }
    public void NextLevel()
    {
        levelNumber++;
        UIManager.Notifications.CreateNotification("welcome to level " + levelNumber);
        int newSize = (int)(Rooms.Count * level_expansion);
        Init(newSize);
    }

    public void Init(int size)
    {
        DisplayManager.Instance.ResetBackground();
        level = new Level(size);
        turn = 0;
        level.Generate();
        level.Ground.ForEach(x => DisplayManager.Instance.Background(x.x, x.y));
        MinimapScript.Instance.UpdateMinimap();

        player = null;
    }
}