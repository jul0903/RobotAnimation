using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FABRIK: MonoBehaviour
{
    public List<Transform> Joints;
    public Transform target;
    public float tolerance = 1.0f;
    public float maxIterations = 1e5f;
    private float lambda;
    private Vector3[] Links;
    private int countIterations;
    private int numberOfJoints;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        numberOfJoints = Joints.Count;
        getLinks();
        initialPosition = Joints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (countIterations < maxIterations &&
            Vector3.Distance(Joints[numberOfJoints - 1].position, target.position) > tolerance)

        {
            Forward();
            Backward();

            countIterations++;
        }
    }
    void getLinks() {
        Links = new Vector3[numberOfJoints - 1];
        for (int i = 0; i < numberOfJoints - 1; i++) {
            Links[i] = Joints[i + 1].position - Joints[i].position;
        }
    }

    void Forward()
    {
        Joints[numberOfJoints - 1].position = target.position;
        for (int i = numberOfJoints - 2; i >= 0; i--) {

            float distance = Vector3.Magnitude(Links[i]);
            float denominator = Vector3.Distance(Joints[i].position, Joints[i + 1].position);
            lambda = distance / denominator;
            Vector3 temp = lambda * Joints[i].position + (1 - lambda) * Joints[i + 1].position;
            Joints[i].position = temp;

        }
    }

    void Backward()
    {
        Joints[0].position = initialPosition;

        for (int i = 1; i < numberOfJoints; i++) {

            float distance = Vector3.Magnitude(Links[i - 1]);
            float denominator = Vector3.Distance(Joints[i - 1].position, Joints[i].position);
            lambda = distance / denominator;
            Vector3 temp = lambda * Joints[i].position + (1 - lambda) * Joints[i - 1].position;
            Joints[i].position = temp;
        }
    
    }


}



