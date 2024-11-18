using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FordwardKinematics2 : MonoBehaviour
{
    public Joint[] joints;
    public float[] angles;
    public Transform target;
    public float tolerance = 0.01f;
    public float learningRate = 0.001f;

    private bool isMoving = false; 
    private Vector3 targetPosition; 

    public Joint[] GetJoints()
    {
        return joints;
    }

    private void InitializeOffsets()
    {
        for (int i = 1; i < joints.Length; i++)
        {
            joints[i].startOffset = joints[i].transform.localPosition;
        }
    }

    public Vector3 FordwardKin(float[] angles, Joint[] joints)
    {
        Vector3 prevPoint = joints[0].transform.localPosition;
        Quaternion rotation = Quaternion.identity;


        for (int i = 1; i < joints.Length; i++)
        {
            //Rotar alrededor del nuevo axis
            rotation *= Quaternion.AngleAxis(angles[i-1], joints[i-1].axis);

            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;

            joints[i-1].transform.localRotation = rotation;

            prevPoint = nextPoint;
        }
        //Tengo que conseguir que me devuelva la posicion del joint 4
        return prevPoint;
    }
    public void MoveToTarget()
    {        
         Vector3 targetPosition = target.position;  // Posici�n objetivo

          for (int iter = 0; iter < 100; iter++) // M�ximo 100 iteraciones
          {
              // Calculamos la posici�n actual del �ltimo joint
              Vector3 currentPosition = FordwardKin(angles, joints);

              // Si estamos suficientemente cerca del objetivo, terminamos
              float distance = Vector3.Distance(currentPosition, targetPosition);

              if (distance < tolerance)
                  return; 

              // CCD: Ajustamos las articulaciones desde el �ltimo al primero
              for (int i = joints.Length - 2; i >= 0; i--)
              {
                  // Vector del joint actual al objetivo
                  Vector3 toTarget = targetPosition - joints[i].transform.position;

                  // Vector del joint actual al extremo del brazo
                  Vector3 toEndEffector = currentPosition - joints[i].transform.position;

                  // Calculamos el �ngulo de rotaci�n necesario
                  float angle = Vector3.SignedAngle(toEndEffector, toTarget, joints[i].axis);

                  // Ajustamos el �ngulo de la articulaci�n
                  angles[i] += angle * learningRate;

                  // Recalculamos la posici�n del brazo con el nuevo �ngulo
                  currentPosition = FordwardKin(angles, joints);
              }
          }

    }

    private void Start()
    {
        InitializeOffsets();
    }

    private void Update()
    {
         MoveToTarget();
    }
}
