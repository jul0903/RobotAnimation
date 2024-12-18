using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GradientClaw : MonoBehaviour
{

    public GradientArm arm;
    public List<Transform> ClawJoints;

    private int numberOfJoints;

    private Vector3 closingRotation = new Vector3(30f, 0f, 0f); //Rotacion que aplico a todos los dedos
    Quaternion[] initialRotations;
    Quaternion targetRotation; //Rotacion final

    public GameObject claw;

    public bool doNotOpenAgain = false;

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
            CloseClaw(1f);
        }
        else
        {
            if(!doNotOpenAgain)
            OpenClaw(1f);
        }
      
    }

    void CloseClaw(float speed)
    {
        // Interpola hacia la rotación objetivo
        for (int i = 0; i < numberOfJoints ; i++) {

            ClawJoints[i].localRotation = Quaternion.Slerp(ClawJoints[i].localRotation, initialRotations[i] * targetRotation, Time.deltaTime * speed);

        }

        //Si el target sigue dentro de la claw cuando esta se ha cerrado, pickeo el target
         if(ClawJoints[numberOfJoints - 1].localRotation == initialRotations[numberOfJoints - 1] * targetRotation)
         {
           Debug.Log("me cieerrooo");

           // GrabSpiderman();
           

                
         }
        
    }
     void OpenClaw(float speed)
    {
        // Interpola suavemente hacia la rotación inicial
        for (int i = 0; i < numberOfJoints; i++)
        {
            ClawJoints[i].localRotation = Quaternion.Slerp(ClawJoints[i].localRotation, initialRotations[i], Time.deltaTime * speed);
        }
    }

    void GrabSpiderman()
    {
        //Como cada brazo tiene un pivot distinto del spiderman como target, busco el parent del pivot hasta llegar al ultimo (el spiderman general)
        Transform masterParent = arm.target.gameObject.transform.parent;

        while (masterParent.parent != null)
        {

            masterParent = masterParent.parent;

        }

        masterParent.SetParent(claw.transform);
        arm.SetNewtarget();
        doNotOpenAgain = true;
    }
}
