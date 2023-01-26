using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private float RangeOfInteract = 2;

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
        
    }

    void Interact()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, RangeOfInteract, transform.forward, 0);
        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("Item"))
            {
                Debug.Log("HIT ITEM");
            }
        }
    }
}
