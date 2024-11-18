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
private Vector3 targetPosition; // Almacena la posición objetivo actual

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

        Vector3 targetPosition = target.position;  // La posición del objetivo (target)

        for (int iter = 0; iter < 100; iter++)
        {
            // Calculamos la posición actual del último joint
            Vector3 currentPosition = FordwardKin(angles, joints);

            // Calculamos la distancia entre el currentPosition y el target
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Si estamos suficientemente cerca del target, terminamos
            if (distance < tolerance)
            {
                Debug.Log("Posición alcanzada en " + iter + " iteraciones.");
                return;
            }

            // Ajuste de las articulaciones de atrás hacia adelante
            for (int i = 1; i < joints.Length; i++)
            {

                // Vector de dirección hacia el objetivo desde el joint actual
                Vector3 directionToTarget = targetPosition - joints[i].transform.localPosition;
                // directionToTarget.Normalize();

                Debug.Log(directionToTarget);
                // Calculamos el ángulo necesario para que la articulación apunte hacia el target
                float adjustment = Vector3.Dot(directionToTarget, joints[i].axis) * learningRate;

                // Actualizamos el ángulo de la articulación
                angles[i] += adjustment;
                Debug.Log(adjustment);


                // Recalculamos la posición del brazo con el nuevo ángulo
                FordwardKin(angles, joints);
            }
        }

        //Intento bugged
        /*
         Vector3 targetPosition = target.position;  // Posición objetivo

          for (int iter = 0; iter < 100; iter++) // Máximo 100 iteraciones
          {
              // Calculamos la posición actual del último joint
              Vector3 currentPosition = FordwardKin(angles, joints);

              // Si estamos suficientemente cerca del objetivo, terminamos
              float distance = Vector3.Distance(currentPosition, targetPosition);

              if (distance < tolerance)
                  return; 

              // CCD: Ajustamos las articulaciones desde el último al primero
              for (int i = joints.Length - 2; i >= 0; i--)
              {
                  // Vector del joint actual al objetivo
                  Vector3 toTarget = targetPosition - joints[i].transform.position;

                  // Vector del joint actual al extremo del brazo
                  Vector3 toEndEffector = currentPosition - joints[i].transform.position;

                  // Calculamos el ángulo de rotación necesario
                  float angle = Vector3.SignedAngle(toEndEffector, toTarget, joints[i].axis);

                  // Ajustamos el ángulo de la articulación
                  angles[i] += angle * learningRate;

                  // Recalculamos la posición del brazo con el nuevo ángulo
                  currentPosition = FordwardKin(angles, joints);
              }
          }*/

    }

    public void StartMoving()
    {
        // Inicializamos el objetivo y verificamos el alcance
        Vector3 targetPosition = target.position;  // Posición objetivo

        float maxReach = 0f;
        foreach (var joint in joints)
            maxReach += joint.startOffset.magnitude;

        if (Vector3.Distance(joints[0].transform.position, targetPosition) > maxReach)
        {
            Debug.LogError("El objetivo está fuera del alcance del brazo.");
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

        // Calculamos la posición actual del extremo del brazo
        Vector3 currentPosition = FordwardKin(angles, joints);

        // Si estamos suficientemente cerca del objetivo, terminamos
        float distance = Vector3.Distance(currentPosition, targetPosition);
        if (distance < tolerance)
        {
            Debug.Log("Posición alcanzada.");
            isMoving = false; // Detenemos el proceso de movimiento
            return;
        }

        // Ajustamos las articulaciones desde el último al primero
        for (int i = joints.Length - 2; i >= 0; i--)
        {
            // Vector del joint actual al objetivo
            Vector3 toTarget = targetPosition - joints[i].transform.position;
            Vector3 toEndEffector = currentPosition - joints[i].transform.position;

            // Normalizamos los vectores para mayor estabilidad
            toTarget.Normalize();
            toEndEffector.Normalize();

            // Calculamos el ángulo necesario para alinear toEndEffector con toTarget
            float angle = Vector3.SignedAngle(toEndEffector, toTarget, joints[i].axis);

            // Ajuste dinámico del ángulo
            float adjustedAngle = Mathf.Clamp(angle * learningRate, -Mathf.Abs(angle) / 2, Mathf.Abs(angle) / 2);

            // Actualizamos el ángulo de la articulación
            angles[i] += adjustedAngle;
        }

        // Recalculamos la posición del brazo tras ajustar todos los ángulos
        FordwardKin(angles, joints);*/
    }
}
