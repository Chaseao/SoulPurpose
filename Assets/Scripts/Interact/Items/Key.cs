using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject door;
    public bool ExecuteDialogue()
    {
        return false;
    }

    public void OpenDoor()
    {
        //Later we can change this to an animation that opens the door
        door.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
