using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public struct PathMessage
{
    public Enemy startMonster;
    public AstartNode startNode;
    public AstartNode targetNode;
    public short nodeType;


    public PathMessage(Transform _start, Transform _target, short _nodeType)
    {
        nodeType = _nodeType;
        startMonster = _start.GetComponent<Enemy>();
        int startx = Mathf.RoundToInt(_start.position.x);
        int starty = Mathf.RoundToInt(_start.position.z);
        int targetx = Mathf.RoundToInt(_target.position.x);
        int targety = Mathf.RoundToInt(_target.position.z);

        startNode = AstartGrid.GetInstance.nodes[startx, starty];
        targetNode = AstartGrid.GetInstance.nodes[targetx, targety];
    }
}

public class Pathfinder : MonoBehaviour
{
    AstartNode startNode;
    AstartNode targetNode;
    AstartGrid grid;

    Enemy startEnemy;

    PathMessage pm;

    Transform targetTS;

    short nodeType;

    public bool isActive = false;

    public Pathfinder()
    {
        grid = AstartGrid.GetInstance;
        targetTS = GameObject.Find("Target").transform;
    }

    public bool ReceiveOrder(PathMessage _message)
    {
        if (isActive) return false;
        pm = _message;
        startEnemy = pm.startMonster;
        startNode = pm.startNode;
        targetNode = pm.targetNode;
        nodeType = pm.nodeType;
        Find();
        return true;
    }

    void Init()
    {
        startNode.gCost = 0;
        startNode.hCost = GetManhattan(startNode, targetNode);
        startEnemy.path = new List<Vector3>();
    }

    void Find()
    {
        isActive = true;
        List<AstartNode> openList = new List<AstartNode>();
        HashSet<AstartNode> closeList = new HashSet<AstartNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            AstartNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost <= currentNode.fCost &&
                   openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (currentNode == targetNode)
            {
                startEnemy.path = GetFinalPath(currentNode);
                return;
            }

            foreach (AstartNode nearNode in grid.NearNode(currentNode))
            {
                if (nearNode.wallNode > nodeType || closeList.Contains(nearNode)) continue;

                int moveCost = currentNode.gCost + GetManhattan(currentNode, nearNode);

                if (moveCost < nearNode.gCost || !openList.Contains(nearNode))
                {
                    nearNode.gCost = moveCost;
                    nearNode.hCost = GetManhattan(nearNode, targetNode);
                    nearNode.parent = currentNode;

                    if (!openList.Contains(nearNode))
                    {
                        openList.Add(nearNode);
                    }
                }
            }
        }

        //if(nodeType == 1)
        //{
        //    startMonster.fSM.isFailed = true;
        //    isActive = false;
        //}

        //nodeType = 1;
        //Find();

        startEnemy.Faild(true);
        //startEnemy.fSM.isFailed = true;
        isActive = false;
    }
    

    List<Vector3> GetFinalPath(AstartNode _targetNode)
    {
        List<Vector3> nodes = new List<Vector3>();

        AstartNode node = _targetNode;

        while(node != startNode)
        {
            nodes.Add(new Vector3(node.posX + 0.5f, 0.5f, node.posY + 0.5f));
            node = node.parent;
        }

        nodes.Reverse();
        Init();
        isActive = false;

        return nodes;
    }

    int GetManhattan(AstartNode _node, AstartNode _targetNode)
    {
        int x = Mathf.Abs(_node.posX - _targetNode.posX);
        int y = Mathf.Abs(_node.posY - _targetNode.posY);

        return x + y;
    }
}
