using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class fanButton : MonoBehaviour, IInteract
{

    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem fanWindVis;
    [SerializeField] Collider fanWind;


    [SerializeField] GameObject interactPreview;    
    [SerializeField] Collider UIcollider;
    [SerializeField] float viewRadius;
    [SerializeField] float viewDistance;

    bool fanSpin;
    bool fanPart;
   

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        UIfan();
    }

    public void interact() 
    {
        Debug.Log("I am interacting");

        fanSpinning();
    }


    void fanSpinning()
    {

        if (fanSpin == false && fanPart == false)
        {
            Debug.Log("Spinning");
            anim.SetTrigger("FanSpin");//Animated Blades
            fanWindVis.Play();//Particles
            fanWind.enabled = true;//PushBack

            fanSpin = true;
            fanPart = true;
        }
        else if (fanSpin == true && fanPart == true)
        {
            Debug.Log("Not Spinning");
            anim.SetTrigger("FanSpin");
            fanWindVis.Stop();
            fanWind.enabled = false;

            fanSpin = false;
            fanPart = false;
        }
    }
    
    void UIfan()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (Physics.SphereCast(ray, viewRadius, out hit, viewDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                //isLooking = true;
                if (interactPreview)
                {
                    interactPreview.SetActive(true);
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
        else
        {
            //isLooking = false;
            if (interactPreview)
            {
                interactPreview.SetActive(false);
            }
        }
    }
}
