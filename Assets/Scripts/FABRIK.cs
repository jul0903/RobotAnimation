using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FABRIK: MonoBehaviour
{
    public List<Transform> Joints;
    public DroneMovement droneTarget;
    public Transform astronautTarget;
    public float tolerance = 0.3f;
    public float maxIterations = 1e5f;
    private float lambda;
    private Vector3[] Links;
    private int countIterations;
    private int numberOfJoints;
    private Vector3 initialPosition;

    public bool canGrab = false;
    public bool grabbed = false;

    private float grabTimer=0f;

    // Start is called before the first frame update
    void Start()
    {
        numberOfJoints = Joints.Count;
        getLinks();
        initialPosition = Joints[0].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!grabbed)
        {
            if (countIterations < maxIterations &&
                Vector3.Distance(Joints[numberOfJoints - 1].position, droneTarget.gameObject.transform.position) > tolerance)

            {
                Forward();
                Backward();
                canGrab = false;
                countIterations++;
            }
            else
            {
                canGrab = true;
            }

            //ROTACION DE LA GARRA 
            //Si ponemos el endfactor como referencia, el final de la garra se superpone a la posicion del dron, por lo tanto se bugea la rotacion
            Vector3 directionToTarget = (droneTarget.transform.position - Joints[numberOfJoints - 2 ].position).normalized;

            Quaternion finalRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget); // el forward esta en el eje y

            Joints[numberOfJoints - 2].rotation = finalRotation;

        }
        else
        {
            if (countIterations < maxIterations &&
                Vector3.Distance(Joints[numberOfJoints - 1].position, astronautTarget.position) > tolerance)

            {
                FordwardAstronaut();
                Backward();
                countIterations++;
            }
        }

        Debug.Log(canGrab);
    
    }

    private void Update()
    {
        if (canGrab)
        {
            grabTimer += Time.deltaTime;
            if(grabTimer >= 2f)
            {
                GrabDrone();
                grabTimer = 0f;
            }
        }
    }

    void GrabDrone() {
        droneTarget.stopMovement = true;
        droneTarget.transform.SetParent(Joints[numberOfJoints - 1]);
        grabbed = true;
    }

    void getLinks() {
        Links = new Vector3[numberOfJoints - 1];
        for (int i = 0; i < numberOfJoints - 1; i++) {
            Links[i] = Joints[i + 1].position - Joints[i].position;
        }
    }

    void Forward()
    {
        Joints[numberOfJoints - 1].position = droneTarget.transform.position;
        for (int i = numberOfJoints - 2; i >= 0; i--)
        {

            float distance = Vector3.Magnitude(Links[i]);
            float denominator = Vector3.Distance(Joints[i].position, Joints[i + 1].position);
            lambda = distance / denominator;
            Vector3 temp = lambda * Joints[i].position + (1 - lambda) * Joints[i + 1].position;
            Joints[i].position = temp;
        }   
    }

    void FordwardAstronaut()
    {
        Joints[numberOfJoints - 1].position = astronautTarget.transform.position;
        for (int i = numberOfJoints - 2; i >= 0; i--)
        {
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



