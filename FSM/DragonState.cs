using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonState : MonoBehaviour
{
    virtual public void Ready(Dragon _Dragon) { }
    virtual public void Execute(Dragon _Dragon) { }
    virtual public void Finish(Dragon _Dragon) { }
}

public class DragonStateMove : DragonState
{
    public override void Ready(Dragon _Dragon)
    {
        GameManager.GetInstance.AddMessage(_Dragon.transform, _Dragon.target, 1);
        _Dragon.ani.SetBool("Fly Forward", true);
    }

    public override void Execute(Dragon _Dragon)
    {
        Move(_Dragon);
        RayForward(_Dragon);
    }

    public override void Finish(Dragon _Dragon)
    {
        _Dragon.path.Clear();
        _Dragon.ani.SetBool("Fly Forward", false);
    }

    private void Move(Dragon _Dragon)
    {
        if (_Dragon.path.Count <= 0)
        {
            return;
        }
        _Dragon.transform.LookAt(_Dragon.path[0]);
        _Dragon.transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);

        if (Vector3.Distance(_Dragon.transform.position, _Dragon.path[0]) < 0.25f)
        {
            _Dragon.path.RemoveAt(0);
        }
    }

    private void RayForward(Dragon _Dragon)
    {
        RaycastHit hit;

        Debug.DrawRay(_Dragon.transform.position, _Dragon.transform.forward, Color.blue);
        if (Physics.Raycast(_Dragon.transform.position, _Dragon.transform.forward, out hit, _Dragon.viewDistance))
        {
            _Dragon.fSM.enemy = hit.transform;

            switch (hit.transform.tag)
            {
                case "Wood":

                    GameManager.GetInstance.AddMessage(_Dragon.transform, _Dragon.target, 0);

                    break;

                case "Wall":

                    GameManager.GetInstance.AddMessage(_Dragon.transform, _Dragon.target, 1);

                    break;

                default:

                    break;
            }
        }
    }
}

public class DragonStateAttack : DragonState
{
    Wood wood;

    float attackTime = 3f;
    float currentTime;

    public override void Ready(Dragon _Dragon)
    {
        wood = _Dragon.fSM.enemy.GetComponent<Wood>();
        currentTime = attackTime;
        _Dragon.crashEffect.transform.position = wood.transform.position;
    }

    public override void Execute(Dragon _Dragon)
    {
        Attack(_Dragon);
    }

    public override void Finish(Dragon _Dragon)
    {

    }

    private void Attack(Dragon _Dragon)
    {
        if (currentTime <= 0)
        {
            _Dragon.ani.SetBool("Fly Fire Breath Attack High", true);
            _Dragon.attackSound.Play();
            _Dragon.attackEffect.Play();
            _Dragon.crashEffect.Play();
            if (wood.hp - 100 <= 0)
            {
                if (wood == null)
                {
                    _Dragon.fSM.ChangeState(new DragonStateMove());
                }

                GameManager.GetInstance.PositionToNode(wood.transform.position).wallNode = 0;
                //GameManager.GetInstance.AddMessage(_Dragon.transform, _Dragon.target.transform, 1);
                _Dragon.fSM.ChangeState(new DragonStateMove());
            }
            wood.hp -= 100;
            currentTime = attackTime;
        }
        currentTime -= Time.deltaTime;
    }
}
