﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnAgent : Agent, IUpdatable
{
    public void Update()
    {
        //dont spawn if player is in room
        var playerIsInRoom = Game.Instance.Rooms.Exists(room => room.Ground.Contains(this.position) && room.Ground.Contains(Game.Instance.Player.position));
        if (playerIsInRoom) return;

        //dont spawn if we already have max spawn
        var mobCount = Game.Instance.Agents.Count(x => x is Mob);
        if (mobCount > Game.Instance.LevelBudget / 5) return;
        
        //random chance of not spawning
        if (Random.value < 0.5f) return;

        var types = GameHelper.GetAllSubTypes<Mob>();
        var type = GameHelper.LinearRandom(types);
        var obj = type.GetConstructor(new System.Type[] { }).Invoke(new object[] { }) as Mob;
        obj.position = this.position;
        Game.Instance.Create(obj);

    }
}