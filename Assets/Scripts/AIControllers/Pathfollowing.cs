using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AISandbox {
    public class Pathfollowing : MonoBehaviour {

        public string currentState = "";
        public bool redKey = false;
        public bool blueKey = false;
        public bool greenKey = false;
        public bool redDoor = false;
        public bool blueDoor = false;
        public bool greenDoor = false;
        public bool treasure = false;

        GameObject redKeyObj = null;
        GameObject blueKeyObj = null;
        GameObject greenKeyObj = null;
        GameObject redDoorObj = null;
        GameObject blueDoorObj = null;
        GameObject greenDoorObj = null;
        GameObject redDoorOpenObj = null;
        GameObject blueDoorOpenObj = null;
        GameObject greenDoorOpenObj = null;
        GameObject treasureObj = null;

        public Grid grid;

        private PriorityQueue<int, GridNode> openPQ = null;
        private List<GridNode> openList = null;
        private Dictionary<GridNode, GridNode> cameFrom = null;
        private Dictionary<GridNode, int> costSoFar = null;
        private Dictionary<GridNode, GridNode> goesTo = null;

        private GridNode start;
        private GridNode goal;

        private IActor _actor;

        private bool pathFound;

        private GridNode currentNode = null;
        private GridNode nextNode = null;
        private GridNode previousNode = null;
        //public SimpleActor _target_actor;

        private void Awake()
        {
            _actor = GetComponent<IActor>();
        }

        private void Start() {
            grid = GameObject.Find("Grid").GetComponent<Grid>();

            start = grid.GetNode(UnityEngine.Random.Range(0, 29), UnityEngine.Random.Range(0, 29));
            start.node_color = GridNode.NodeColor.start;
            
            FindAndSetObjects();

            //GenerateRandomGoal();
            UpdateGoalState();


            pathFound = false;
        }

        private void FindAndSetObjects()
        {
            redKeyObj = GameObject.Find("RedKey");
            redDoorObj = GameObject.Find("RedDoor");
            redDoorOpenObj = GameObject.Find("RedDoorOpen");

            blueKeyObj = GameObject.Find("BlueKey");
            blueDoorObj = GameObject.Find("BlueDoor");
            blueDoorOpenObj = GameObject.Find("BlueDoorOpen");

            greenKeyObj = GameObject.Find("GreenKey");
            greenDoorObj = GameObject.Find("GreenDoor");
            greenDoorOpenObj = GameObject.Find("GreenDoorOpen");

            redDoorOpenObj.SetActive(false);
            blueDoorOpenObj.SetActive(false);
            greenDoorOpenObj.SetActive(false);

            treasureObj = GameObject.Find("Treasure");
        }

        private void GenerateRandomGoal()
        {
            goal = grid.GetNode(UnityEngine.Random.Range(0, 29), UnityEngine.Random.Range(0, 49));

            if (goal.node_type == GridNode.NodeType.blocked || goal.node_color == GridNode.NodeColor.goal)
                GenerateRandomGoal();

            goal.node_color = GridNode.NodeColor.goal;
        }

        private void Update()
        {
            if (treasure)
            {
                Application.LoadLevel(Application.loadedLevel);
            }

            if (!pathFound)
            {
                pathFound = true;

                SetNodeColorBasedOnType(start);
                start = grid.GetNodethatActorIsOn(transform.position);
                start.node_color = GridNode.NodeColor.start;

                //ClearPath();
                AStarAlgorithm(start, goal);
                //UpdatePath();

                FillGoesToList();
            }

            if (!grid.GetNodethatActorIsOn(transform.position))
            {
                _actor.SetInput(0.01f, 0);
            }
            else
            {
                currentNode = grid.GetNodethatActorIsOn(transform.position);

                if (currentNode == goal)
                {
                    SetNodeColorBasedOnType(currentNode);
                    //ClearPath();
                    //GenerateRandomGoal();

                    FinishGoalState();
                    UpdateGoalState();

                    Debug.Log(currentState);

                    pathFound = false;
                }
                else
                {
                    if (goesTo.TryGetValue(currentNode, out nextNode))
                        previousNode = currentNode;
                    else
                        nextNode = goesTo[previousNode];

                    Vector2 steering = Vector2.zero;

                    if (nextNode.column > currentNode.column)
                        steering += new Vector2(0.01f, 0.0f);
                    else if (nextNode.column < currentNode.column)
                        steering += new Vector2(-0.01f, 0.0f);
                    else if (nextNode.row > currentNode.row)
                        steering += new Vector2(0.0f, -0.01f);
                    else if (nextNode.row < currentNode.row)
                        steering += new Vector2(0.0f, 0.01f);

                    //steering += grid.GetPositionOfNode(nextNode);

                    _actor.SetInput(steering.x, steering.y);

                    if (currentNode.node_type == GridNode.NodeType.normal)
                        _actor.MaxSpeed = 2.0f;
                    else if (currentNode.node_type == GridNode.NodeType.forest)
                        _actor.MaxSpeed = 1.0f;
                    else if (currentNode.node_type == GridNode.NodeType.swamp)
                        _actor.MaxSpeed = 0.4f;
                    //else if (currentNode.node_type == GridNode.NodeType.blocked)
                    //    _actor.MaxSpeed = 0.0f;
                    
                }
            }

        }

        private void FinishGoalState()
        {
            if (redKey && blueKey && greenKey && redDoor && blueDoor && greenDoor)
            {
                treasure = true;
            }

            else if (redKey && blueKey && greenKey && redDoor && blueDoor)
            {
                greenDoor = true;
                greenDoorObj.SetActive(false);
                greenDoorOpenObj.SetActive(true);
                grid.GetNodethatActorIsOn(greenDoorObj.transform.position).blocked = false;
            }

            else if (redKey && blueKey && redDoor && blueDoor)
            {
                greenKey = true;
                greenKeyObj.SetActive(false);
            }

            else if (redKey && blueKey && redDoor)
            {
                blueDoor = true;
                blueDoorObj.SetActive(false);
                blueDoorOpenObj.SetActive(true);
                grid.GetNodethatActorIsOn(blueDoorObj.transform.position).blocked = false;
            }

            else if (redKey && redDoor)
            {
                blueKey = true;
                blueKeyObj.SetActive(false);
            }

            else if (redKey)
            {
                redDoor = true;
                redDoorObj.SetActive(false);
                redDoorOpenObj.SetActive(true);
                grid.GetNodethatActorIsOn(redDoorObj.transform.position).blocked = false;
            }

            else
            {
                redKey = true;
                redKeyObj.SetActive(false);
            }
        }

        private void UpdateGoalState()
        {
            if (redKey && blueKey && greenKey && redDoor && blueDoor && greenDoor)
            {
                goal = grid.GetNodethatActorIsOn(treasureObj.transform.position);
                currentState = "Seeking Treasure";
            }

            else if (redKey && blueKey && greenKey && redDoor && blueDoor)
            {
                goal = grid.GetNodethatActorIsOn(greenDoorObj.transform.position);
                currentState = "Seeking Green Door";
            }

            else if (redKey && blueKey && redDoor && blueDoor)
            {
                goal = grid.GetNodethatActorIsOn(greenKeyObj.transform.position);
                currentState = "Seeking Green Key";
            }

            else if (redKey && blueKey && redDoor)
            {
                goal = grid.GetNodethatActorIsOn(blueDoorObj.transform.position);
                currentState = "Seeking Blue Door";
            }

            else if (redKey && redDoor)
            {
                goal = grid.GetNodethatActorIsOn(blueKeyObj.transform.position);
                currentState = "Seeking Blue Key";
            }

            else if (redKey)
            {
                goal = grid.GetNodethatActorIsOn(redDoorObj.transform.position);
                currentState = "Seeking Red Door";
            }

            else
            {
                goal = grid.GetNodethatActorIsOn(redKeyObj.transform.position);
                currentState = "Seeking Red Key";
            }
        }

        private void SetNodeColorBasedOnType(GridNode currentNode)
        {
            if (currentNode.node_type == GridNode.NodeType.normal)
                currentNode.node_color = GridNode.NodeColor.normal;
            else if (currentNode.node_type == GridNode.NodeType.forest)
                currentNode.node_color = GridNode.NodeColor.forest;
            else if (currentNode.node_type == GridNode.NodeType.swamp)
                currentNode.node_color = GridNode.NodeColor.swamp;
            else if (currentNode.node_type == GridNode.NodeType.blocked)
                currentNode.node_color = GridNode.NodeColor.blocked;
        }

        private void FillGoesToList()
        {
            goesTo = new Dictionary<GridNode, GridNode>();
            goesTo[goal] = null;

            GridNode current;

            if (cameFrom != null)
            {
                current = cameFrom[goal];
                goesTo[current] = goal;

                while (current != start)
                {
                    goesTo[cameFrom[current]] = current;
                    current = cameFrom[current];
                }
            }
        }

        private void ClearPath()
        {
            GridNode current;

            if (cameFrom != null)
            {
                current = cameFrom[goal];
                while (current.node_color != GridNode.NodeColor.start)
                {
                    //if(current.node_color == GridNode.NodeColor.path || current.node_color == GridNode.NodeColor.calcPath)
                    //    current.node_color = GridNode.NodeColor.normal;
                    SetNodeColorBasedOnType(current);

                    current = cameFrom[current];
                }
            }

            //if (openPQ != null)
            //{
            //    foreach (GridNode node in openList)
            //    {
            //        current = node;
            //        if (current.node_type != GridNode.NodeType.start && current.node_type != GridNode.NodeType.start)
            //        {
            //            if (current.node_type == GridNode.NodeType.path || current.node_type == GridNode.NodeType.calcPath)
            //                current.node_type = GridNode.NodeType.normal;
            //        }
            //    }
            //}
        }

        private void UpdatePath()
        {
            GridNode current = cameFrom[goal];
            while(current.node_color != GridNode.NodeColor.start)
            {
                current.node_color = GridNode.NodeColor.path;
                current = cameFrom[current];
            }

            //foreach (GridNode node in openList)
            //{
            //    current = node;
            //    if (current.node_type != GridNode.NodeType.start && current.node_type != GridNode.NodeType.start)
            //    {
            //        if (current.node_type == GridNode.NodeType.normal)
            //            current.node_type = GridNode.NodeType.calcPath;
            //    }
            //}
        }

        private bool AStarAlgorithm(GridNode i_start, GridNode i_goal) {
            openPQ = new PriorityQueue<int, GridNode>();
            openList = new List<GridNode>();
            cameFrom = new Dictionary<GridNode, GridNode>();
            costSoFar = new Dictionary<GridNode, int>();

            openPQ.Add(new KeyValuePair<int, GridNode>(0, i_start));
            cameFrom.Add(i_start, null);
            costSoFar.Add(i_start, 0);

            while (!openPQ.IsEmpty)
            {
                GridNode currentNode = openPQ.Dequeue().Value;
                openList.Add(currentNode);

                if (currentNode == i_goal)
                    break;

                foreach (GridNode nextNode in grid.GetNodeNeighbors(currentNode.row, currentNode.column))
                {
                    openList.Add(nextNode);
                    int newCost = costSoFar[currentNode] + grid.GetNeighborCost(currentNode, nextNode);

                    if( !costSoFar.ContainsKey(nextNode) || newCost < costSoFar[nextNode])
                    {
                        if (!costSoFar.ContainsKey(nextNode))
                            costSoFar.Add(nextNode, newCost);
                        else
                            costSoFar[nextNode] = newCost;

                        int priority = newCost + grid.GetHeuristicCost(i_goal, nextNode);
                        openPQ.Add(new KeyValuePair<int, GridNode>(priority, nextNode));

                        if (!cameFrom.ContainsKey(nextNode))
                            cameFrom.Add(nextNode, currentNode);
                        else
                            cameFrom[nextNode] = currentNode;
                    }
                }
            }

            if (costSoFar[i_goal] > 9999)
                return false;
            else
                return true;
        }
    }
}