using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class FordwardKinematics2 : MonoBehaviour
{
    public Joint[] joints;
    public float[] angles;
    public Transform target;
    public float tolerance = 0.01f;  // Tolerancia para el objetivo
    public float learningRate = 0.001f;

    private bool isMoving = false; // Bandera para controlar el movimiento
private Vector3 targetPosition; // Almacena la posici�n objetivo actual

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

        Vector3 targetPosition = target.position;  // La posici�n del objetivo (target)

        for (int iter = 0; iter < 100; iter++)
        {
            // Calculamos la posici�n actual del �ltimo joint
            Vector3 currentPosition = FordwardKin(angles, joints);

            // Calculamos la distancia entre el currentPosition y el target
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Si estamos suficientemente cerca del target, terminamos
            if (distance < tolerance)
            {
                Debug.Log("Posici�n alcanzada en " + iter + " iteraciones.");
                return;
            }

            // Ajuste de las articulaciones de atr�s hacia adelante
            for (int i = 1; i < joints.Length; i++)
            {

                // Vector de direcci�n hacia el objetivo desde el joint actual
                Vector3 directionToTarget = targetPosition - joints[i].transform.localPosition;
                // directionToTarget.Normalize();

                Debug.Log(directionToTarget);
                // Calculamos el �ngulo necesario para que la articulaci�n apunte hacia el target
                float adjustment = Vector3.Dot(directionToTarget, joints[i].axis) * learningRate;

                // Actualizamos el �ngulo de la articulaci�n
                angles[i] += adjustment;
                Debug.Log(adjustment);


                // Recalculamos la posici�n del brazo con el nuevo �ngulo
                FordwardKin(angles, joints);
            }
        }

        //Intento bugged
        /*
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
          }*/

    }

    public void StartMoving()
    {
        // Inicializamos el objetivo y verificamos el alcance
        Vector3 targetPosition = target.position;  // Posici�n objetivo

        float maxReach = 0f;
        foreach (var joint in joints)
            maxReach += joint.startOffset.magnitude;

        if (Vector3.Distance(joints[0].transform.position, targetPosition) > maxReach)
        {
            Debug.LogError("El objetivo est� fuera del alcance del brazo.");
            return;
        }

        isMoving = true; // Inicia el proceso de movimiento
    }

    private void Start()
    {
        InitializeOffsets();
    }

    private void Update()
    {
         MoveToTarget();
       /* if (!isMoving) return;

        // Calculamos la posici�n actual del extremo del brazo
        Vector3 currentPosition = FordwardKin(angles, joints);

        // Si estamos suficientemente cerca del objetivo, terminamos
        float distance = Vector3.Distance(currentPosition, targetPosition);
        if (distance < tolerance)
        {
            Debug.Log("Posici�n alcanzada.");
            isMoving = false; // Detenemos el proceso de movimiento
            return;
        }

        // Ajustamos las articulaciones desde el �ltimo al primero
        for (int i = joints.Length - 2; i >= 0; i--)
        {
            // Vector del joint actual al objetivo
            Vector3 toTarget = targetPosition - joints[i].transform.position;
            Vector3 toEndEffector = currentPosition - joints[i].transform.position;

            // Normalizamos los vectores para mayor estabilidad
            toTarget.Normalize();
            toEndEffector.Normalize();

            // Calculamos el �ngulo necesario para alinear toEndEffector con toTarget
            float angle = Vector3.SignedAngle(toEndEffector, toTarget, joints[i].axis);

            // Ajuste din�mico del �ngulo
            float adjustedAngle = Mathf.Clamp(angle * learningRate, -Mathf.Abs(angle) / 2, Mathf.Abs(angle) / 2);

            // Actualizamos el �ngulo de la articulaci�n
            angles[i] += adjustedAngle;
        }

        // Recalculamos la posici�n del brazo tras ajustar todos los �ngulos
        FordwardKin(angles, joints);*/
    }
}
