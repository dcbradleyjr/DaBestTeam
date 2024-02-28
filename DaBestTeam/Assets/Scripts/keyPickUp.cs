using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyPickUp : MonoBehaviour, IInteract
{
    [Header("--Attributes--")]
    [SerializeField] string myName;
    [SerializeField] float viewRadius;
    [SerializeField] float viewDistance;
    [Header("--UI--")]
    [SerializeField] GameObject interactPreview;

    void Start()
    {
       gameManager.instance.updateKeyCount(1, myName);
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, viewDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                //isLooking = true;
                if(interactPreview)
                {
                    interactPreview.SetActive(true);
                }
            }
            else
            {
                //isLooking = false;
                if(interactPreview)
                {
                    interactPreview.SetActive(false);
                }
            }
        }
        else
        {
            //isLooking = false;
            if (interactPreview)
            {
                interactPreview.SetActive(false);
            }
        }
        
    }

    public void interact()
    {
        gameManager.instance.updateKeyCount(-1, myName);
        Destroy(gameObject);
    }
}
