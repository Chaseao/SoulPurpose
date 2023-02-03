using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private float rangeOfInteract = 2;
    private IInteractable item;

    private void OnEnable()
    {
        Controller.OnInteract += Interact;
    }

    private void OnDisable()
    {
        Controller.OnInteract -= Interact;
    }

    void Update()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, rangeOfInteract, transform.forward, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Item"))
            {
                //Make object glow
            }
        }
    }

    void Interact()
    {
        RaycastHit[] hits;
        float minDistance = 1000f;
        hits = Physics.SphereCastAll(this.transform.position, rangeOfInteract, transform.forward, 0);
        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("Item"))
            {
                if(hit.distance < minDistance)
                {
                    item = hit.collider.gameObject.GetComponent<IInteractable>();
                    minDistance = hit.distance;
                }
            }
            else if(hit.collider.gameObject.CompareTag("Key"))
            {
                if(hit.distance < minDistance)
                {
                    item = hit.collider.gameObject.GetComponent<IInteractable>();
                    minDistance = hit.distance;
                }
            }
        }
        item.ExecuteDialogue();
        item.OpenDoor();
    }
}
