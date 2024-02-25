using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    [SerializeField] float windSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        IPushBack pushBack = other.GetComponent<IPushBack>();

        if (pushBack != null)// is not null means IPushback is present
        {
            pushBack.pushBackDir(transform.forward * windSpeed * Time.deltaTime); //Z Axis is forward
        }
    }
}
