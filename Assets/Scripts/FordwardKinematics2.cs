using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class FordwardKinematics2 : MonoBehaviour
{
    public Joint[] joints;
    public float[] angles;
    public Transform target;
    public float tolerance = 0.01f;  // Tolerancia para el objetivo
    public float learningRate = 0.1f;

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
        Debug.Log(prevPoint);
        //Tengo que conseguir que me devuelva la posicion del joint 4
        return prevPoint;
    }
    public void MoveToTarget()
    {

        Vector3 targetPosition = target.position;  // La posici�n del objetivo (target)

        for (int iter = 0; iter < 10; iter++)
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
            for (int i = joints.Length - 1; i >= 0; i--)
            {
                // Vector de direcci�n hacia el objetivo desde el joint actual
                Vector3 directionToTarget = targetPosition - joints[i].transform.position;

                // Calculamos el �ngulo necesario para que la articulaci�n apunte hacia el target
                float adjustment = Vector3.Dot(directionToTarget, joints[i].axis) * learningRate;

                // Actualizamos el �ngulo de la articulaci�n
                angles[i] += adjustment;

                // Recalculamos la posici�n del brazo con el nuevo �ngulo
                FordwardKin(angles, joints);
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
