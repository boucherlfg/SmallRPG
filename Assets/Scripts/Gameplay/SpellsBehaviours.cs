using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SpellsBehaviours
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SpellBehaviourEnumAttribute))]
    public class ScrollBehaviourDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            List<string> list = ScrollBehaviourNames;

            if (list.Count <= 0)
            {
                EditorGUI.Popup(position, property.name, 0, new string[] { "Nothing there!" });
                return;
            }
            var item = property.stringValue;
            var index = list.IndexOf(item);
            if (index < 0 || index > list.Count) index = 0;

            index = EditorGUI.Popup(position, property.name, index, list.ToArray());
            item = list[index];
            property.stringValue = item;

            EditorGUI.EndProperty();
        }
    }
#endif
    public class SpellBehaviourEnumAttribute : PropertyAttribute { }
    public class SpellBehaviourDefinitionAttribute : Attribute { }

    public static void ExecuteAsTrap(string methodName, Vector2Int target)
    {
        var agent = Game.Instance.Agents.Find(x => x.position == target && x is ICollision);
        if (agent != null)
        {
            scrollBehaviour.Find(x => x.Name == methodName).Invoke(null, new object[] { agent });
            return;
        }
    }
    public static void ExecuteAsProjectile(string methodName, Vector2Int source, Vector2Int direction)
    {
        var iterProtection = 1000;
        var target = source;
        while (true)
        {
            iterProtection--;
            if (iterProtection < 0)
            {
                return;
            }
            target += direction;
            var agent = Game.Instance.Agents.Find(x => x.position == target && x is ICollision);
            if (agent != null)
            {
                scrollBehaviour.Find(x => x.Name == methodName).Invoke(null, new object[] { agent });
                return;
            }
        }
    }

    private static List<MethodInfo> scrollBehaviour => typeof(SpellsBehaviours).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Where(x => Attribute.IsDefined(x, typeof(SpellBehaviourDefinitionAttribute))).ToList();
    public static List<string> ScrollBehaviourNames
    {
        get
        {
            return scrollBehaviour.Select(x => x.Name).ToList();
        }
    }

    [SpellBehaviourDefinition]
    private static void Fire(Agent agent)
    {
        Game.Instance.Create(new FireAgent()
        {
            position = agent.position,
        });
        return;
    }
    [SpellBehaviourDefinition]
    private static void Ice(Agent agent)
    {
        if (agent is Mob)
        {
            (agent as Mob).state = new Mob.IdleState(agent as Mob, (agent as Mob).state);
        }
        var target = agent.position;
        var agents = Game.Instance.Agents.FindAll(x =>
            x.position == target + Vector2Int.up ||
            x.position == target + Vector2Int.down ||
            x.position == target + Vector2Int.left ||
            x.position == target + Vector2Int.right);

        if (!agents.Exists(x => x.position == target + Vector2Int.up))
        {
            Game.Instance.Create(new IceAgent()
            {
                position = target + Vector2Int.up,
            });
        }
        if (!agents.Exists(x => x.position == target + Vector2Int.down))
        {
            Game.Instance.Create(new IceAgent()
            {
                position = target + Vector2Int.down,
            });
        }
        if (!agents.Exists(x => x.position == target + Vector2Int.left))
        {
            Game.Instance.Create(new IceAgent()
            {
                position = target + Vector2Int.left,
            });
        }
        if (!agents.Exists(x => x.position == target + Vector2Int.right))
        {
            Game.Instance.Create(new IceAgent()
            {
                position = target + Vector2Int.right,
            });
        }
    }

    [SpellBehaviourDefinition]
    private static void Summon(Agent agent)
    {
        if (!Game.Instance.Agents.Exists(x => x.position == agent.position + Vector2Int.up) && Game.Instance.Level.Ground.Contains(agent.position + Vector2Int.up)) Create(agent.position + Vector2Int.up);
        else if (!Game.Instance.Agents.Exists(x => x.position == agent.position + Vector2Int.down) && Game.Instance.Level.Ground.Contains(agent.position + Vector2Int.down)) Create(agent.position + Vector2Int.down);
        else if (!Game.Instance.Agents.Exists(x => x.position == agent.position + Vector2Int.left) && Game.Instance.Level.Ground.Contains(agent.position + Vector2Int.left)) Create(agent.position + Vector2Int.left);
        else if (!Game.Instance.Agents.Exists(x => x.position == agent.position + Vector2Int.right) && Game.Instance.Level.Ground.Contains(agent.position + Vector2Int.right)) Create(agent.position + Vector2Int.right);

        void Create(Vector2Int position)
        {
            Game.Instance.Create(new Ally()
            {
                position = position
            });
        }
    }
    [SpellBehaviourDefinition]
    private static void Teleport(Agent agent)
    {
        if (!(agent is IMovable)) return;
        if (agent is Mob)
        {
            (agent as Mob).state = new Mob.IdleState(agent as Mob, (agent as Mob).state);
        }
        var pos = GameHelper.LinearRandom(Game.Instance.Level.Ground);
        while (Game.Instance.Agents.Exists(x => x.position == pos && x is ICollision))
        {
            pos = GameHelper.LinearRandom(Game.Instance.Level.Ground);
        }
        agent.position = pos;
        return;

    }
}