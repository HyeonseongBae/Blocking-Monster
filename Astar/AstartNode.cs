using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class AstartNode
{
    public int posX;
    public int posY;
    

    public AstartNode parent;

    public short wallNode = 0;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public AstartNode(int _x, int _y)
    {
        posX = _x;
        posY = _y;
    }

    static public bool operator <(AstartNode _node, AstartNode _node2)
    {
        if(_node.fCost <= _node2.fCost &&
            _node.hCost < _node2.hCost)
        {
            return true;
        }
        return false;
    }

    static public bool operator >(AstartNode _node, AstartNode _node2)
    {
        if (_node.fCost >= _node2.fCost &&
            _node.hCost > _node2.hCost)
        {
            return true;
        }
        return false;
    }
}
