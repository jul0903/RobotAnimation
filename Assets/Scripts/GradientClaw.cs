using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GradientClaw : MonoBehaviour
{

    public GradientArm arm;
    public List<Transform> ClawJoints;
    public Transform endFactor;

    public Transform target;


    private int numberOfJoints;

    private Vector3 closingRotation = new Vector3(30f, 0f, 0f); //Rotacion que aplico a todos los dedos
    Quaternion[] initialRotations;
    Quaternion targetRotation; //Rotacion final



    private void Start()
    {
        numberOfJoints = ClawJoints.Count;
        //me guardo las rotaciones iniciales para multiplicarlas por las del target
        initialRotations = new Quaternion[numberOfJoints];
        for (int i = 0; i < numberOfJoints; i++)
        {
            initialRotations[i] = ClawJoints[i].localRotation;
        }

        targetRotation = Quaternion.Euler(closingRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (arm.GetCostFunc() <= arm.tolerance)
        {
            //Debug.Log(numberOfJoints);
            CloseClaw(0.5f);

        }

    }


    void CloseClaw(float speed)
    {

        for (int i = 0; i < numberOfJoints ; i++) {

            // Interpola suavemente hacia la rotación objetivo
            ClawJoints[i].localRotation = Quaternion.Slerp(ClawJoints[i].localRotation, initialRotations[i] * targetRotation, Time.deltaTime * speed);
           // Debug.Log("entras??????????");

        }
        
    }
}
