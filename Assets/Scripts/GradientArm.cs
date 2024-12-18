using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientArm : MonoBehaviour
{

    public List<Transform> Joints;
    public Transform endFactor;

    public Transform target;
    public Transform newTarget;

    private float costFunction;
 
    private Vector3[] distances; //Links entre los Joints

    public float alpha = 1f;

    private Vector4 theta;

    public float tolerance = 0.2f;

    private Vector4 gradient;

    private int numberOfJoints;

  // [SerializeField] public List<GradientClaw> claws;

    void Start()
    {
        numberOfJoints = Joints.Count;
        endFactor = Joints[numberOfJoints - 1];
        distances = new Vector3[numberOfJoints - 1];

        for (int i = 0; i < numberOfJoints - 1; i++)
        {
            distances[i] = Joints[i + 1].position - Joints[i].position;
        }

        theta = Vector4.zero;
        costFunction = Vector3.SqrMagnitude(endFactor.position - target.position); 
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("cost function = " + costFunction);
        if (costFunction > tolerance) {

            gradient = GetGradient(theta);
            theta -= alpha * gradient;
            Vector3[] newPosition = endFactorFunction(theta);

            for (int i = 0;i < numberOfJoints; i++)
            {
                Joints[i].position = newPosition[i];
            }
          
            endFactor.position = newPosition[numberOfJoints - 1];

        }
 
        costFunction = lossCostFunction(theta);
        
    }

    //Calcula posiciones de los joints en funcion de theta y los joints anteriores, returnea las posiciones nuevas
    Vector3[] endFactorFunction(Vector4 theta)
    {
        Vector3[] result = new Vector3[numberOfJoints];

        Quaternion rotation = Quaternion.identity;

        Vector3 currentPosition = Joints[0].position;

        result[0] = currentPosition;

        for (int i = 0; i < distances.Length; i++)
        {
            // Aplicar rotaciones. Se van acumulando
            if (i == 0) rotation *= Quaternion.AngleAxis(theta.x, Vector3.up);
            if (i == 1) rotation *= Quaternion.AngleAxis(theta.y, Vector3.forward);
            if (i == 2) rotation *= Quaternion.AngleAxis(theta.z, Vector3.up);
            if (i == 3) rotation *= Quaternion.AngleAxis(theta.w, Vector3.forward);

            currentPosition += rotation * distances[i];
            result[i + 1] = currentPosition;
        }

        //Rotacion al endfactor para que apunte al target
        Vector3 endPosition = result[numberOfJoints - 1]; // Endfactor
        Vector3 directionToTarget = (target.position - endPosition).normalized;

        Quaternion finalRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget); // el forward esta en el eje y

        Joints[numberOfJoints - 1].rotation = finalRotation;

        return result;
    }

    //Actualiza la distancia de la pos final hasta el target
    float lossCostFunction(Vector4 theta) {

     
        Vector3 endPosition = endFactorFunction(theta)[numberOfJoints - 1]; //endfactor pos

        return Vector3.SqrMagnitude(endPosition - target.position); // + 0.5f * (1 - Vector3.Dot(endDirection, currentDirection));
    }

   //Calcula el gradient y el cost con la funcion de diferencias finitas
    Vector4 GetGradient(Vector4 theta)
    {
        Vector4 gradientVector = Vector4.zero;
        float step = 1e-2f;

        //x
        Vector4 thetaPlus = theta;
        thetaPlus.x = theta.x + step;
        gradientVector.x = (lossCostFunction(thetaPlus) - lossCostFunction(theta))/step;

        // y
        thetaPlus = theta;
        thetaPlus.y = theta.y + step;
        gradientVector.y = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // z
        thetaPlus = theta;
        thetaPlus.z = theta.z + step;
        gradientVector.z = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // w
        thetaPlus = theta;
        thetaPlus.w = theta.w + step;
        gradientVector.w = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;


        gradientVector.Normalize();

        return gradientVector;
     }

    public float GetCostFunc()
    {
        return costFunction;
    }


    public void SetNewtarget() {
        target = newTarget;
    }
}
