using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonoBehaviour
{
    public MonsterState state;
    public Monster monster;
    public Transform enemy;
    public bool isFailed = false;

    public MonsterFSM(Monster _monster)
    {
        state = new MonsterStateMove();
        monster = _monster;
    }

    public void Execute(Monster _monster)
    {
        state.Execute(_monster);
        GetLost();
    }

    public void ChangeState(MonsterState _state)
    {
        state.Finish(monster);
        state = _state;
        state.Ready(monster);
    }

    private void GetLost()
    {
        if (isFailed)
        {
            if(enemy == null)
            {
                return;
            }

            switch (enemy.tag)
            {
                case "Wall":

                    Destroy(monster.gameObject);

                    break;

                case "Wood":

                    isFailed = false;
                    ChangeState(new MonsterStateAttack());

                    break;

                default:

                    break;
            }
        }
    }
}