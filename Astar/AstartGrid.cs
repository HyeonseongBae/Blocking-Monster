using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstartGrid : MonoBehaviour
{
    static public AstartGrid instance;
    static public AstartGrid GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new AstartGrid();
            }
            return instance;
        }
    }

    public GameObject start;
    public GameObject end;

    public AstartNode[,] nodes;

    public List<AstartNode> finalPath;

    public int posMaxX;
    public int posMaxY;

    int wallLayer;
    int woodLayer;

    private void Awake()
    {
        instance = this;
        Vector3 groundPos = GameObject.Find("Ground").transform.position;
        posMaxX = (int)groundPos.x * 2;
        posMaxY = (int)groundPos.z * 2;
        nodes = new AstartNode[posMaxX, posMaxY];
        wallLayer = LayerMask.GetMask("Wall");
        woodLayer = LayerMask.GetMask("Wood");
        CreateNodes();
    }

    void Start()
    {

    }

    void CreateNodes()
    {
        for (int i = 0; i < posMaxX; i++)
        {
            for (int j = 0; j < posMaxY; j++)
            {
                nodes[i, j] = new AstartNode(i, j);

                Vector3 vec = new Vector3(i + 0.5f, 0.5f, j + 0.5f);

                if (Physics.CheckSphere(vec, 0.25f, wallLayer))
                {
                    nodes[i, j].wallNode = 2;
                }
                if (Physics.CheckSphere(vec, 0.25f, woodLayer))
                {
                    nodes[i, j].wallNode = 1;
                }
            }
        }
    }

    public List<AstartNode> NearNode(AstartNode _node)
    {
        List<AstartNode> nodes = new List<AstartNode>();

        int indexX;
        int indexY;

        indexX = _node.posX + 1;
        indexY = _node.posY;

        if (indexX >= 0 && indexX < posMaxX)
        {
            if (indexY >= 0 && indexY < posMaxY)
            {
                nodes.Add(this.nodes[indexX, indexY]);
            }
        }

        indexX = _node.posX - 1;
        indexY = _node.posY;

        if (indexX >= 0 && indexX < posMaxX)
        {
            if (indexY >= 0 && indexY < posMaxY)
            {
                nodes.Add(this.nodes[indexX, indexY]);
            }
        }

        indexX = _node.posX;
        indexY = _node.posY + 1;

        if (indexX >= 0 && indexX < posMaxX)
        {
            if (indexY >= 0 && indexY < posMaxY)
            {
                nodes.Add(this.nodes[indexX, indexY]);
            }
        }

        indexX = _node.posX;
        indexY = _node.posY - 1;

        if (indexX >= 0 && indexX < posMaxX)
        {
            if (indexY >= 0 && indexY < posMaxY)
            {
                nodes.Add(this.nodes[indexX, indexY]);
            }
        }

        // Cross NearNode
        //for (int i = -1; i < 2; i++)
        //{
        //    for (int j = -1; j < 2; j++)
        //    {
        //        if (i == 0 && j == 0) continue;

        //        indexX = _node.posX + i;
        //        indexY = _node.posY + j;

        //        if (indexX >= 0 && indexX < posMaxX)
        //        {
        //            if (indexY >= 0 && indexY < posMaxY)
        //            {
        //                nodes.Add(this.nodes[indexX, indexY]);
        //            }
        //        }
        //    }
        //}
        return nodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0, 1, 0));

        if (nodes != null)
        {
            foreach (AstartNode node in nodes)
            {
                if (node.wallNode == 2)
                {
                    Gizmos.color = Color.black;
                }
                else if (node.wallNode == 1)
                {
                    Gizmos.color = Color.yellow;

                }
                else
                {
                    Gizmos.color = Color.white;
                }

                if (finalPath != null)
                {
                    if (finalPath.Contains(node))
                    {
                        Gizmos.color = Color.red;
                    }
                }
                Vector3 vec = new Vector3(node.posX + 0.5f, 0.5f, node.posY + 0.5f);
                Gizmos.DrawCube(vec, Vector3.one);
            }
        }
    }
}
