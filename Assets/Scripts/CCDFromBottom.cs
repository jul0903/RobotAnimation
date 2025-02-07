using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDFromBottom : MonoBehaviour
{
    public Transform Joint0;
    public Transform Joint1;
    public Transform Joint2;
    public Transform Joint3;
    public Transform Joint4;
    public Transform end;

    public Transform target;

    public float tolerance;
    public float maxIterations;
    private int iterationCount;

    private float rotation;
    private Vector3 axis;

    private int index;
    Vector3[] Joints = new Vector3[5];

    private Vector3[] Links;




    // Start is called before the first frame update
    void Start()
    {
        tolerance = 1.0f;
        maxIterations = 1e5f;
        iterationCount = 0;
        index = 0;
        Joints[0] = Joint0.position;
        Joints[1] = Joint1.position;
        Joints[2] = Joint2.position;
        Joints[3] = Joint3.position;
        Joints[4] = Joint4.position;

        getLinks();

    }

    // Update is called once per frame
    void Update()
    {
        if (iterationCount < maxIterations && Vector3.Distance(end.position, target.position) > tolerance)
        {
            Vector3 Pd = Joints[index];
            Vector3[] referenceVectors;
            referenceVectors = GetVectors(Pd);
            rotation = GetAngle(referenceVectors);
            axis = GetAxis(referenceVectors);

            UpdatePosition(index, rotation, axis);

            if (index == 4)
            {
                index = 0;
            }
            else { index++; }
            iterationCount++;
        }

    }

    void getLinks()
    {
        Links = new Vector3[5];
        for (int i = 0; i < 4; i++)
        {
            Links[i] = Joints[i + 1] - Joints[i];

        }

        Links[4] = end.position - Joints[4];
    }

    Vector3[] GetVectors(Vector3 Pd)
    {
        Vector3[] referenceVectors = new Vector3[2];

        referenceVectors[0] = Vector3.Normalize(end.position - Pd);
        referenceVectors[1] = Vector3.Normalize(target.position - Pd);

        return referenceVectors;

    }

    float GetAngle(Vector3[] referenceVectors)
    {

        float theta;
        theta = Mathf.Acos(Mathf.Clamp(Vector3.Dot(referenceVectors[0], referenceVectors[1]), -1.0f, 1.0f));
        return theta;
    }

    Vector3 GetAxis(Vector3[] referenceVectors)
    {

        Vector3 r;
        r = Vector3.Cross(referenceVectors[0], referenceVectors[1]);
        return r;
    }


    void UpdatePosition(int index, float rotation, Vector3 axis)
    {

        Quaternion q = Quaternion.AngleAxis(rotation * 180 / Mathf.PI, axis);

        if (index <= 3)
        {
            for (int i = index; i <= 3; i++)
            {
                Joints[i + 1] = Joints[i] + q * Links[i];
            }
        }

        end.position = Joints[4] + q * Links[4];


        Joint1.position = Joints[1];
        Joint2.position = Joints[2];
        Joint3.position = Joints[3];
        Joint4.position = Joints[4];

        getLinks();
    }




}
