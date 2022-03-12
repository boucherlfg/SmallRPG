﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Agent
{
    public Vector2Int position;
    public virtual string ID => GetType().Name + Game.Instance.Agents.IndexOf(this);
}


