﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Tilemaps;

public class Wolf : Mob
{
    private Vector2Int savedPosition;
    private bool friendly;

    public override float Value => 3;

    public override AgentData.AgentType AgentType => AgentData.AgentType.Carnivore;

    protected override State InitialState => new RandomWalk(this);


    public override void Activate(IMovable user)
    {
        base.Activate(user);
        friendly = false;
        state = new AttackState(this, user as Agent);
    }

    public abstract class WolfState : State
    {
        public WolfState(Wolf self) : base(self) { }

        public Agent FindATarget()
        {
            var all = GameHelper.FOV(self.position, self.detectionTreshold);
            // -------------------------------------------- trying to find a lure of type wolf
            var hit = all.Find(x => x is LureAgent && (x as LureAgent).lure.type == Lure.LureType.Carnivore);
            if (hit != null)
            {
                return hit;
            }
            // -------------------------------------------- trying to find a deer
            hit = all.Find(x => x is Deer);
            if (hit != null)
            {
                return hit;
            }
            return null;
        }
    }
    public class ReturnToPosition : WolfState
    {
        public ReturnToPosition(Wolf self) : base(self)
        {
            path = new List<Vector2Int>();
        }

        public override State Update()
        {
            var target = FindATarget();
            if (target != null)
            {
                return new GotoState(self as Wolf, target);
            }

            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos);
                path = Astar.GetPath(self.position, (self as Wolf).savedPosition);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return new RandomWalk(self as Wolf);
            }

            var step = path[0];
            if (collisionDetection(step))
            {
                path.Clear();
                return this;
            }
            self.Orientation = step - self.position;
            self.position = path[0];
            path.RemoveAt(0);
            return this;
        }
    }
    public class RandomWalk : WolfState
    {
        public RandomWalk(Wolf self) : base(self) { }


        public override State Update()
        {
            var target = FindATarget();
            if (target != null)
            {
                return new GotoState(self as Wolf, target);
            }


            // ---------------------------------------- random walk
            if (path.Count <= 0)
            {
                var astarColDetection = Astar.CollisionDetection;
                Astar.CollisionDetection = pos => collisionDetection(pos, self); //override collision detection method
                var u = GameHelper.LinearRandom(Game.Instance.Level.Ground);
                path = Astar.GetPath(self.position, u);
                Astar.CollisionDetection = astarColDetection;
                if (path.Count <= 0) return this;

            }

            var step = path[0];
            if (collisionDetection(step))
            {
                path.Clear();
                return this;
            }
            self.Orientation = step - self.position;
            self.position = path[0];
            path.RemoveAt(0);

            return this;
        }
    }
    public class GotoState : WolfState
    {
        Vector2Int lastSeenAt;
        private Agent target;
        public GotoState(Wolf self, Agent target) : base(self) 
        {
            self.savedPosition = self.position;
            this.target = target;
            path = new List<Vector2Int>();
            lastSeenAt = target.position;
        }

        public override State Update()
        {
            if (target == self) return new RandomWalk(self as Wolf);

            var all = GameHelper.FOV(self.position, self.detectionTreshold);
            var isInRange = all.Exists(x => x == target);

            //if target is next to us, we attack/eat it
            if (IsNextToMe(target))
            {
                if (target is LureAgent)
                {
                    return new EatState(self as Wolf, target);
                }
                
                else if (!(self as Wolf).friendly || !(target is Player))
                {
                    return new AttackState(self as Wolf, target);
                }
                else
                {
                    return this;
                }
            }

            //if target is in range, we "see" it
            if (isInRange)
            {
                lastSeenAt = target.position;
            }

            if (path.Count <= 0)
            {

                if (!isInRange)
                {
                    //follow player back after loosing track of foe
                    if ((self as Wolf).friendly && !(target is Player) && all.Exists(x => x == Game.Instance.Player))
                    {
                        return new GotoState(self as Wolf, Game.Instance.Player);
                    }
                    else
                    {
                        (self as Wolf).friendly = false;
                        return new ReturnToPosition(self as Wolf);
                    }
                }
                else
                {
                    var astarColDetection = Astar.CollisionDetection;
                    Astar.CollisionDetection = pos => collisionDetection(pos, target);
                    path = Astar.GetPath(self.position, lastSeenAt);
                    Astar.CollisionDetection = astarColDetection;
                    return this;
                }
            }
            else
            {
                var step = path[0];
                if (collisionDetection(step))
                {
                    path.Clear();
                    return this;
                }
                self.Orientation = step - self.position;
                self.position = path[0];
                path.RemoveAt(0);
                return this;
            }
        }
    }
    public class EatState : WolfState
    {
        private Agent target;
        public EatState(Wolf self, Agent target) : base(self)
        {
            this.target = target;
        }

        public override State Update()
        {
            if (target == self) return new RandomWalk(self as Wolf);
            Game.Instance.Destroy(target);
            if (IsInRange(Game.Instance.Player, self))
            {
                (self as Wolf).friendly = true;
                return new GotoState(self as Wolf , Game.Instance.Player);
            }
            else
            {
                return new RandomWalk(self as Wolf);
            }
        }
    }
    public class AttackState : WolfState
    {
        private Agent target;
        public AttackState(Wolf wolf, Agent target) : base(wolf)
        {
            this.target = target;
        }

        public override State Update()
        {
            if (target == self) return new RandomWalk(self as Wolf);
            bool isPlayer = target is Player;

            if (isPlayer) UIManager.Notifications.CreateNotification($"{self.data.visibleName} tries to hit you");
            if (target == self) return new RandomWalk(self as Wolf);
            var hit = GameHelper.CalculateHit(self, target as IStats);
            if (hit)
            {
                var dmg = GameHelper.CalculateDamage(self, target as IStats);

                var s = (target as IStats).Stats;
                s.life -= dmg;
                (target as IStats).Stats = s;

                if (isPlayer)
                {
                    UIManager.Notifications.CreateNotification($"and deals {dmg} damage to you.");
                    DataModel.Equipment.Damage(EquipType.Body, EquipType.Leg, EquipType.Head);
                    DisplayManager.Instance.CreateDamageText(dmg, target.position);
                }
                if ((target as IStats).Stats.life <= 0)
                {
                    return new RandomWalk(self as Wolf);
                }
            }
            if (IsNextToMe(target)) return this;
            else return new GotoState(self as Wolf, target);
        }
    }
}