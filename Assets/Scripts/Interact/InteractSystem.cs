using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Sirenix.Utilities;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private float rangeOfInteract = 2;
    private IInteractable item;
    private Material glow;
    private Dictionary<GameObject, Boolean> objectsHit = new Dictionary<GameObject, bool>();
    private bool hasHitItem;

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
        float minDistance = 1000f;
        hits = Physics.SphereCastAll(this.transform.position, rangeOfInteract, transform.forward, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Item") || hit.collider.gameObject.CompareTag("Key"))
            {
                hasHitItem= true;
                if (!objectsHit.ContainsKey(hit.collider.gameObject)) objectsHit.Add(hit.collider.gameObject, false);
                if (hit.distance < minDistance)
                {
                    objectsHit[hit.collider.gameObject] = true;
                    minDistance = hit.distance;
                }
                else
                {
                    objectsHit[hit.collider.gameObject] = false;
                }
            }
        }
        if(!hasHitItem) foreach (GameObject key in objectsHit.Keys.ToList()) objectsHit[key] = false;
        hasHitItem= false;
        objectsHit.Where(x => x.Value).ToList().ForEach(x => Glow(x.Key));
        objectsHit.Where(x => !x.Value).ToList().ForEach(x => NotGlow(x.Key));
    }

    void Glow(GameObject objectToGlow)
    {
        if (objectToGlow.GetComponent<MeshRenderer>().material != null)
        {
            glow = objectToGlow.GetComponent<MeshRenderer>().material;
            glow.SetInteger("_GlowBool", 1);
        }
    }

    void NotGlow(GameObject objectToNotGlow)
    {
        if (objectToNotGlow.GetComponent<MeshRenderer>().material != null)
        {
            glow = objectToNotGlow.GetComponent<MeshRenderer>().material;
            glow.SetInteger("_GlowBool", 0);
        }
    }

    void Interact()
    {
        RaycastHit[] hits;
        float minDistance = 1000f;
        hits = Physics.SphereCastAll(this.transform.position, rangeOfInteract, transform.forward, 0);
        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.CompareTag("Item") || hit.collider.gameObject.CompareTag("Key"))
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
