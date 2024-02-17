using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyPickUp : MonoBehaviour, IInteract
{
    // Start is called before the first frame update

    [SerializeField] string myName;
    [SerializeField] float rotSpeed = 90f;
    [SerializeField] float bounceHeight = 0.3f;
    [SerializeField] float bounceSpeed = 3f;

    [SerializeField] float viewRadius = 0.5f;
    [SerializeField] float viewDistance = 3f;

    [SerializeField] GameObject interactPreview;

    private Vector3 startPos;
    //bool isLooking;
    void Start()
    {
       gameManager.instance.updateKeyCount(1, myName);
       startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(transform.up, rotSpeed * Time.deltaTime);

        Vector3 bounceOffset = Vector3.up * Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + bounceOffset;
        
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.SphereCast(ray,viewRadius,out hit, viewDistance))
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
