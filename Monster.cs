using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Enemy
{
    public MonsterFSM fSM = null;
    

    private void Awake()
    {
        target = GameObject.Find("Target").transform;
        ani = GetComponent<Animator>();
        attackSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        fSM = new MonsterFSM(this);
    }

    private void Update()
    {
        fSM.Execute(this);
    }

    public override void Faild(bool _isfaild)
    {
        fSM.isFailed = _isfaild;
    }
}
