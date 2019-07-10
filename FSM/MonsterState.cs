using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    virtual public void Ready(Monster _monster) { }
    virtual public void Execute(Monster _monster) { }
    virtual public void Finish(Monster _monster) { }
}

public class MonsterStateMove : MonsterState
{
    public override void Ready(Monster _monster)
    {
        GameManager.GetInstance.AddMessage(_monster.transform, _monster.target, 1);
        _monster.ani.SetBool("Move Forward", true);
    }

    public override void Execute(Monster _monster)
    {
        Move(_monster);
        RayForward(_monster);
    }

    public override void Finish(Monster _monster)
    {
        _monster.path.Clear();
        _monster.ani.SetBool("Move Forward", false);
    }

    private void Move(Monster _monster)
    {
        if (_monster.path.Count <= 0)
        {
            return;
        }

        Vector3 lookVec = _monster.path[0] - _monster.transform.position;

        _monster.transform.rotation = Quaternion.Lerp(_monster.transform.rotation,
            Quaternion.LookRotation(lookVec), 0.7f);

        _monster.transform.Translate(Vector3.forward * 1.5f * Time.deltaTime);

        if (Vector3.Distance(_monster.transform.position, _monster.path[0]) < 0.25f)
        {
            _monster.path.RemoveAt(0);
        }
    }

    private void RayForward(Monster _monster)
    {
        RaycastHit hit;

        Debug.DrawRay(_monster.transform.position, _monster.transform.forward, Color.blue);

        if (Physics.Raycast(_monster.transform.position, _monster.transform.forward, out hit, _monster.viewDistance))
        {
            _monster.fSM.enemy = hit.transform;

            switch (hit.transform.tag)
            {
                case "Wood":

                    GameManager.GetInstance.AddMessage(_monster.transform, _monster.target, 0);

                    break;

                case "Wall":

                    GameManager.GetInstance.AddMessage(_monster.transform, _monster.target, 1);

                    break;

                default:

                    break;
            }
        }
    }
}

public class MonsterStateAttack : MonsterState
{
    Wood wood;

    float attackTime = 1.5f;
    float currentTime;

    public override void Ready(Monster _monster)
    {
        wood = _monster.fSM.enemy.GetComponent<Wood>();
        currentTime = attackTime;
        _monster.crashEffect.transform.position = wood.transform.position;
    }

    public override void Execute(Monster _monster)
    {
        Attack(_monster);
    }

    public override void Finish(Monster _monster)
    {
        _monster.ani.SetBool("Swing Attack", false);
    }

    private void Attack(Monster _monster)
    {
        if (currentTime <= 0)
        {
            _monster.ani.SetBool("Swing Attack", true);
            _monster.attackSound.Play();
            _monster.attackEffect.Play();
            _monster.crashEffect.Play();
            if (wood.hp - 1 <= 0)
            {
                if (wood == null)
                {
                    _monster.fSM.ChangeState(new MonsterStateMove());
                }

                GameManager.GetInstance.PositionToNode(wood.transform.position).wallNode = 0;
                //GameManager.GetInstance.AddMessage(_monster.transform, _monster.target.transform, 1);
                _monster.fSM.ChangeState(new MonsterStateMove());
            }
            wood.hp -= 2;
            currentTime = attackTime;
        }
        currentTime -= Time.deltaTime;
    }
}
