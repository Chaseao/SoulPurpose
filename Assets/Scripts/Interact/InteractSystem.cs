using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Controller.OnInteract += Interact;
    }

    private void OnDisable()
    {
        Controller.OnInteract -= Interact;
    }

    void Interact()
    {
        Debug.Log("interact");
        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position, 4 , transform.forward, out hit, 10))
        {
            if (hit.transform.CompareTag("Item"))
            {
                Debug.Log("HIT ITEM");
            }
        }
    }
}
