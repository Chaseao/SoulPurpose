using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject door;
    public void ExecuteDialogue()
    {
        //Should never activate this method
    }

    public void OpenDoor()
    {
        //Later we can change this to an animation that opens the door
        door.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
