using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public float hp = 5f;

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
