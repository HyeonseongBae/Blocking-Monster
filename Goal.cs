using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.transform.tag;
        if(tag == "Monster" || tag == "Dragon")
        {
            SceneManager.LoadScene(GameManager.GetInstance.level);
        }
    }
}
