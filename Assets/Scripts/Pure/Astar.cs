using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : CSharpSingleton<Astar>
{
    private int _maxNumberOfNodes = 250; 
    public static int MaxNumberOfNodes
    {
        get => _instance._maxNumberOfNodes;
        set => _instance._maxNumberOfNodes = value;
    }
    public static Func<Vector2Int, bool> CollisionDetection
    {
        get => _instance.obstacleDetection;
        set => _instance.obstacleDetection = value;
    }
    Func<Vector2Int, bool> obstacleDetection;


    public Astar()
    {
        obstacleDetection = pos => false;
    }



    private class Node
    {
        public override string ToString()
        {
            return $"pos : {Position}, cost : {Cost}, pred : {Predecessor}";
        }
        private Node _pred;
        private Vector2Int _pos;
        private float _stepsDone;
        private float _stepsTodo;

        public Vector2Int Position => _pos;
        public Node Predecessor => _pred;
        public float Cost => _stepsDone + _stepsTodo;
        public float StepsDone => _stepsDone;
        public float StepsToDo => _stepsTodo;

        public Node(Vector2Int pos, Vector2Int destination, Node pred = null)
        {
            this._pos = pos;
            this._pred = pred;
            this._stepsDone = PredCount;
            this._stepsTodo = Vector2Int.Distance(pos, destination);
        }
        public float PredCount
        {
            get
            {
                Node temp = this;
                int count = 0;
                while (temp._pred != null) 
                {
                    count++;
                    temp = temp._pred;
                }
                return count;
            }
        }
    }
    public static List<Vector2Int> GetPath(Vector2Int start, Vector2Int end) => _instance._GetPath(start, end);
    private List<Vector2Int> _GetPath(Vector2Int start, Vector2Int end)
    {
        if (IsObstacle(end)) return new List<Vector2Int>(); 

        List<Node> toVisit = new List<Node>();
        List<Node> visited = new List<Node>();

        Node current = new Node(start, end);
        toVisit.Add(current);

        while (toVisit.Count > 0)
        {
            //get smallest cost node
             toVisit.Sort(Comp);
            current = toVisit.First();

            //if it's the end node, return the path
            if (current.Position == end) return BuildPath(current);

            //for all neighbours
            var neighs = GetNeighbours(current.Position);
            neighs.ForEach(pos =>
            {
                Node newNode = new Node(pos, end, current);
                //if this node has not been "visited", update toVisit list
                if (!visited.Any(x => x.Position == newNode.Position))
                {
                    var alreadyExisting = toVisit.Find(x => x.Position == newNode.Position);
                    if (alreadyExisting == null || newNode.Cost < alreadyExisting.Cost || newNode.StepsToDo < alreadyExisting.StepsToDo)
                    {
                        toVisit.RemoveAll(x => x.Position == newNode.Position);
                        toVisit.Add(newNode);
                    }
                }
            });
            //transfer newly visited node to visited list
            toVisit.Remove(current);
            visited.Add(current);
            if (visited.Count >= _maxNumberOfNodes) return new List<Vector2Int>();
        }
        return new List<Vector2Int>();
    }

    static List<Vector2Int> BuildPath(Node node)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        while (node.Predecessor != null) //we won't get the last node which is the start node
        {
            ret.Insert(0, node.Position);
            node = node.Predecessor;
        }
        return ret;
    }

    /// <summary>
    /// for sorting
    /// </summary>
    static int Comp(Node a, Node b)
    {
        var ret = a.Cost - b.Cost;
        if (ret == 0) return (int)(a.StepsToDo - b.StepsToDo);
        else return (int)ret;
    } 
    static List<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        if (!IsObstacle(pos + Vector2Int.up)) ret.Add(pos + Vector2Int.up);
        if (!IsObstacle(pos + Vector2Int.down)) ret.Add(pos + Vector2Int.down);
        if (!IsObstacle(pos + Vector2Int.left)) ret.Add(pos + Vector2Int.left);
        if (!IsObstacle(pos + Vector2Int.right)) ret.Add(pos + Vector2Int.right);
        return ret;
    }

    
    public static bool IsObstacle(Vector2Int pos)
    {
        var ret = _instance.obstacleDetection?.Invoke(pos);
        return ret.Value;
    }
}
