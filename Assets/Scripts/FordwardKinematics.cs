using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FordwardKinematics : MonoBehaviour
{
    public Joint[] joints;
    public float[] angles;

    public Vector3 FordwardKin(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            //Rotar alrededor del nuevo axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;

            prevPoint = nextPoint;
            Debug.Log(prevPoint);
        }
        return prevPoint;
    }


    public void FordwardKin2(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            //Rotar alrededor del nuevo axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;


            joints[i].transform.Rotate(nextPoint);

            prevPoint = nextPoint;
            Debug.Log(prevPoint);
        }
    }

    private void Start()
    {

        //FordwardKin2(angles);
        
    }

    private void Update()
    {
        //foreach (Joint joint in joints)
        //{
        //    joint.transform.Rotate(FordwardKin(angles));
        //}
        FordwardKin2(angles);
    }
}
