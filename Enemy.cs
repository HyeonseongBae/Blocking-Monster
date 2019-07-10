using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public List<Vector3> path = new List<Vector3>();
    public Transform target;
    public Animator ani;
    public float viewDistance = 3f;
    public float time = 2f;
    public ParticleSystem attackEffect;
    public ParticleSystem crashEffect;
    public AudioSource attackSound;
    public abstract void Faild(bool isfaild);
}
