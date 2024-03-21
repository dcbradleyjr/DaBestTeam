using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Transform aimTransform;
    public Transform bone;
    public bool faceRight;

    public int iterations = 10;
    [Range(0, 1)] public float weight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward  * 50;

        for (int i = 0; i < iterations; i++)
        {
            AimAtTarget(bone, targetPosition, weight);
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition,float weight)
    {

        Vector3 aimDirection = aimTransform.forward;
        if (faceRight)
            aimDirection = aimTransform.right;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }
}
