using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFSM : MonoBehaviour
{
    public DragonState state;
    public Dragon Dragon;
    public Transform enemy;
    public bool isFailed = false;

    public DragonFSM(Dragon _Dragon)
    {
        state = new DragonStateMove();
        Dragon = _Dragon;
        _Dragon.ani.SetBool("Fly Forward", true);
    }

    public void Execute(Dragon _Dragon)
    {
        state.Execute(_Dragon);
        GetLost();
    }

    public void ChangeState(DragonState _state)
    {
        state.Finish(Dragon);
        state = _state;
        state.Ready(Dragon);
    }

    private void GetLost()
    {
        if (isFailed)
        {
            if (enemy == null)
            {
                return;
            }

            switch (enemy.tag)
            {
                case "Wall":

                    Destroy(Dragon.gameObject);

                    break;

                case "Wood":

                    isFailed = false;
                    ChangeState(new DragonStateAttack());

                    break;

                default:

                    break;
            }
        }
    }
}
