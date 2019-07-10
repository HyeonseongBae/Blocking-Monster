using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    GameObject[] monsters;
    GameObject[] dragons;
    Transform target;

    bool isStart = false;
    bool isFast = false;

    private void Awake()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        dragons = GameObject.FindGameObjectsWithTag("Dragon");
        target = GameObject.Find("Target").transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(isStart)
        {
            foreach(GameObject go in monsters)
            {
                go.GetComponent<Monster>().enabled = true;
                enabled = false;
            }

            foreach (GameObject go in dragons)
            {
                go.GetComponent<Dragon>().enabled = true;
                enabled = false;
            }
        }
    }

    public void FastForward()
    {
        if (!isStart)
        {
            isStart = true;
            isFast = false;
            Time.timeScale = 1f;
            GameManager.GetInstance.start = true;

            foreach (var i in monsters)
            {
                GameManager.GetInstance.AddMessage(i.transform, target, 1);
            }

            foreach (var i in dragons)
            {
                GameManager.GetInstance.AddMessage(i.transform, target, 1);
            }
            return;
        }
        isFast = !isFast;

        if (isFast)
        {
            Time.timeScale = 3f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
