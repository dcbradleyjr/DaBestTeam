using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour, IInteract
{
    // Start is called before the first frame update

    [SerializeField] string myName;
    [SerializeField] float rotSpeed = 90f;
    [SerializeField] float bounceHeight = 0.3f;
    [SerializeField] float bounceSpeed = 3f;

    private Vector3 startPos;
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
    }


    public void interact()
    {
        gameManager.instance.updateKeyCount(-1, myName);
        Destroy(gameObject);
    }
}
