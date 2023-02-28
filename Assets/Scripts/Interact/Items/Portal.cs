using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        print("Should get here");
        if (other.gameObject.transform.CompareTag("Player"))
        {
            print("Yo");
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
