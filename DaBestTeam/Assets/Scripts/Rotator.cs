using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("--Attributes--")]
    [Range(1f, 90f)][SerializeField] float rotSpeedX;
    [Range(1f, 90f)][SerializeField] float rotSpeedY;
    [Range(1f, 90f)][SerializeField] float rotSpeedZ;
    [Range(0.01f,0.5f)][SerializeField] float bounceHeight;
    [Range(0.1f, 3f)][SerializeField] float bounceSpeed;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.right, rotSpeedX * Time.deltaTime);
        transform.Rotate(transform.up, rotSpeedY * Time.deltaTime);
        transform.Rotate(transform.forward, rotSpeedZ * Time.deltaTime);


        Vector3 bounceOffset = Vector3.up * Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + bounceOffset;
    }
}
