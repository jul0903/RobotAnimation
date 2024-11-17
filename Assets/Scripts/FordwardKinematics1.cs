using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


public class FordwardKinematics1 : MonoBehaviour
{
    public Transform cube;
    public Joint[] joints;
    public float[] angles;
    public float tolerance = 0.01f;
    public float learningRate = 0.1f;
    public float rotationSpeed = 5f;
    public int maxIterations;

    private Quaternion[] jointRotations;


    private void Start()
    {
        jointRotations = new Quaternion[joints.Length];
    }

    public Vector3 ForwardKin(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            // Normalizar joints[i].axis
            joints[i - 1].axis = Vector3.Normalize(joints[i - 1].axis);

            //Rotar alrededor del nuevo axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);
            jointRotations[i] = rotation;
            Vector3 nextPoint = prevPoint + rotation * joints[i].startOffset;

            prevPoint = nextPoint;
            Debug.Log(prevPoint);
        }
        return prevPoint;
    }
    public void MoveArmToCube()
    {
        Vector3 targetPosition = cube.position;

        for (int iter = 0; iter < maxIterations; iter++)
        {
            Vector3 currentPosition = ForwardKin(angles);

            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Si hemos alcanzado el objetivo
            if (distance < tolerance)
            {
                Debug.Log("Posición alcanzada en " + iter + " iteraciones");
                return; // Salir si ya estamos lo suficientemente cerca
            }

            Vector3 directionToTarget = targetPosition - currentPosition;

            for (int i = 0; i < joints.Length; i++)
            {
                float originalAngle = angles[i];
                angles[i] += learningRate;

                Vector3 newPosition = ForwardKin(angles);

                float newDistance = Vector3.Distance(newPosition, targetPosition);

                if(newDistance < distance)
                {
                    distance = newDistance;
                }
                else
                {
                    angles[i] = originalAngle;
                }
            }
        }
        Debug.Log("No se alcanzó el objetivo después de " + maxIterations + " iteraciones.");
    }


    public void FordwardKin2()
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            Quaternion targetRotation = rotation * Quaternion.AngleAxis(angles[i - 1], joints[i - 1].axis);

            //joints[i - 1].transform.rotation = Quaternion.Slerp(joints[i - 1].transform.localRotation, jointRotations[i], Time.deltaTime * rotationSpeed);

            Vector3 nextPoint = prevPoint + targetRotation * joints[i].startOffset;
            joints[i].transform.position = nextPoint;

            prevPoint = nextPoint;
            rotation = targetRotation;
            Debug.Log(prevPoint);

        }
    }
    private void Update()
    {
        MoveArmToCube();
        FordwardKin2();
    }
}