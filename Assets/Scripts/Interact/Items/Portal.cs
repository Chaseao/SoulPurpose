using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            if (SceneTools.NextSceneExists)
            {
                StartCoroutine(SceneTools.TransitionToScene(SceneTools.NextSceneIndex));
            }
            else
            {
                Application.Quit();
            }
        }

    }

}
