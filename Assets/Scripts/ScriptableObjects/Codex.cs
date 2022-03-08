using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Codex : MonoSingleton<Codex>
{
    public List<Item> items;
    public List<AgentData> agents;
    public List<Recipe> recipes;
    public Codex()
    {
        items = new List<Item>();
        agents = new List<AgentData>();
    }
    public static MyList<Item> Items 
    {
        get => new MyList<Item>(_instance.items);
    }
    public static MyList<AgentData> Agents
    {
        get => new MyList<AgentData>(_instance.agents);
    }
    public static MyList<Recipe> Recipes => new MyList<Recipe>(_instance.recipes);

    public class MyList<T> : List<T> where T : ScriptableObject
    {
        public MyList(List<T> items) : base(items) { }
        public MyList()
        {
        }
        public T this[string name]
        {
            get => this.Find(x => x.name == name);
        }
    }
}