using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Build
{
    normal, Wood, Block
}

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    static public GameManager GetInstance
    {
        get
        {
            if (instance == null)
            {
                print("sssss");
                instance = new GameManager();
            }
            return instance;
        }
    }

    public Transform mouseLight;
    public GameObject wall;
    public GameObject wood;

    public float overTime = 30f;
    public int overMoney = 100;

    public int level;

    public bool start = false;

    [SerializeField] Text moneyUI;
    [SerializeField] Text timeUI;

    Pathfinder pathfinder;
    Queue<PathMessage> pathMessages;

    Build build = Build.Block;

    Transform player;
    Transform enemy;

    PathMessage message;

    private void Awake()
    {
        instance = this;
        pathMessages = new Queue<PathMessage>();
    }

    private void Start()
    {
        mouseLight = GameObject.Find("MousePos").transform;
        enemy = GameObject.Find("Target").transform;
        timeUI = GameObject.Find("Time").GetComponent<Text>();
        moneyUI = GameObject.Find("Money").GetComponent<Text>();

        pathfinder = new Pathfinder();
    }

    void Update()
    {
        ClickMap();
        ReadMessage();
        UIUpdate();
    }

    void UIUpdate()
    {
        timeUI.text = Mathf.FloorToInt(overTime).ToString();
        moneyUI.text = overMoney.ToString();

        if(overTime <= 0)
        {
            SceneManager.LoadScene(level);
        }

        if (start)
        {
            overTime -= Time.deltaTime;
        }
    }

    void ClickMap()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 objectHit = hit.point;
            AstartNode node = PositionToNode(objectHit);

            Vector3 mousePos = mouseLight.position;
            mousePos.x = node.posX + 0.5f;
            mousePos.z = node.posY + 0.5f;
            mouseLight.position = mousePos;

            if (Input.GetMouseButtonDown(0))
            {
                if (node.wallNode != 0) return;

                switch (build)
                {
                    case Build.normal:

                        break;

                    case Build.Block:

                        if (overMoney >= 4 && node.wallNode != 2)
                        {
                            node.wallNode = 2;
                            GameObject pWall = GameObject.Instantiate<GameObject>(wall);
                            pWall.transform.position = new Vector3(node.posX + 0.5f, 0.5f, node.posY + 0.5f);
                            overMoney -= 4;
                        }

                        break;

                    case Build.Wood:

                        if (overMoney >= 2 && node.wallNode != 1)
                        {
                            node.wallNode = 1;
                            GameObject pWood = GameObject.Instantiate<GameObject>(wood);
                            pWood.transform.position = new Vector3(node.posX + 0.5f, 0.5f, node.posY + 0.5f);
                            overMoney -= 2;
                        }

                        break;

                    default:

                        break;
                }
            }
        }
    }

    void ReadMessage()
    {
        if (pathMessages.Count <= 0)
        {
            return;
        }

        PathMessage message = pathMessages.Dequeue();
        pathfinder.ReceiveOrder(message);
    }

    public void AddMessage(Transform _start, Transform _target, short _wallType)
    {
        PathMessage message = new PathMessage(_start, _target, _wallType);
        pathMessages.Enqueue(message);
    }

    public AstartNode PositionToNode(Vector3 _position)
    {
        //int x = Mathf.RoundToInt(_position.x);
        //int y = Mathf.RoundToInt(_position.z);

        int x = Mathf.FloorToInt(_position.x);
        int y = Mathf.FloorToInt(_position.z);

        return AstartGrid.GetInstance.nodes[x, y];
    }

    public void ClickWood()
    {
        build = Build.Wood;
    }

    public void ClickBlock()
    {
        build = Build.Block;
    }
}
