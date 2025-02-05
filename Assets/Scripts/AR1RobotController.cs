using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AR1RobotController : MonoBehaviour
{
    // Array de 5 articulaciones del brazo robótico
    public Transform[] joints = new Transform[5];
    float[] initialPos = new float[5];
    public Transform endFactor;

    // Angulos actuales y objetivos de las articulaciones (en grados)
    public float[] currentJointAngles = new float[5];
    public float[] autoTargetJointAngles = new float[5];
    public float[] manualTargetJointAngles = new float[5];

    // Velocidad de interpolación para el movimiento
    public float speed = 1.0f;

    //movimiento manual o predefinido
    bool manualMovement = false;

    bool obstacle = false;
    bool onReset = false;
    float timerToReset = 0.0f;
    private void Start()
    {
        //Nos guardamos las posiciones iniciales del brazo
        /*initialPos[0] =  joints[0].localEulerAngles.y ;
        initialPos[1] = joints[1].localEulerAngles.x;
        initialPos[2] = joints[2].localEulerAngles.x;
        initialPos[3] = joints[3].localEulerAngles.x;
        initialPos[4] = joints[4].localEulerAngles.x ;*/
        initialPos[0] = 0;
        initialPos[1] = 0;
        initialPos[2] = -42.872f;
        initialPos[3] = -40.43f;
        initialPos[4] = -41.725f;
        for (int i = 0; i < 5; i++)
        {
            manualTargetJointAngles[i] = initialPos[i];
            currentJointAngles[i] = initialPos[i];
        }

    }
    void Update()
    {
        // Iniciar el movimiento automarico prefinido presionar la tecla "A"
        if (Input.GetKeyDown(KeyCode.A))
        {
            manualMovement = false;
            StopAllCoroutines();
            StartCoroutine(MoveToTargetPosition(autoTargetJointAngles));
        }

        //Movimiento en tiempo real
        MoveManually();

        if (manualMovement)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTargetPosition(manualTargetJointAngles));
        }

        //Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            manualMovement = false;
            ResetToStart();
        }

        if(!onReset)
            ObstacleAvoidance();
        else
        {
            timerToReset += Time.deltaTime;
            if(timerToReset > 2f)
            {
                onReset = false;
                timerToReset = 0f;
            }
        }

    }

    void ObstacleAvoidance()
    {
        obstacle = Physics.Raycast(endFactor.position, endFactor.up, 1f) ||
        Physics.Raycast(endFactor.position, endFactor.right, 1f) ||
        Physics.Raycast(endFactor.position, -endFactor.right, 1f) ||
        Physics.Raycast(endFactor.position, endFactor.forward, 1f) ||
        Physics.Raycast(endFactor.position, -endFactor.forward, 1f);

        if(obstacle)
        {
            StopAllCoroutines();
            onReset = true;
            manualMovement = false;
            ResetToStart();
        }

        Debug.Log(obstacle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(endFactor.position, endFactor.up * 1);
        Gizmos.DrawRay(endFactor.position, endFactor.right * 1);
        Gizmos.DrawRay(endFactor.position, -endFactor.right * 1);
        Gizmos.DrawRay(endFactor.position, endFactor.forward * 1);
        Gizmos.DrawRay(endFactor.position, -endFactor.forward * 1);
    }

    private void MoveManually()
    {
        if (obstacle)
            return;

        //Joint 0
        if (Input.GetKey(KeyCode.Alpha1))
        {
            manualMovement = true;
            manualTargetJointAngles[0] += 50f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            manualMovement = true;
            manualTargetJointAngles[0] -= 50f * Time.deltaTime;
        }

        //Joint 1
        if (Input.GetKey(KeyCode.Alpha3))
        {
            manualMovement = true;
            manualTargetJointAngles[1] += 50f * Time.deltaTime;
            manualTargetJointAngles[1] = Mathf.Clamp(manualTargetJointAngles[1], -90f, 90f);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            manualMovement = true;
            manualTargetJointAngles[1] -= 50f * Time.deltaTime;
            manualTargetJointAngles[1] = Mathf.Clamp(manualTargetJointAngles[1], -90f, 90f);
        }

        //Joint 2
        if (Input.GetKey(KeyCode.Alpha5))
        {
            manualMovement = true;
            manualTargetJointAngles[2] += 50f * Time.deltaTime;
            manualTargetJointAngles[2] = Mathf.Clamp(manualTargetJointAngles[2], -90f, 90f);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            manualMovement = true;
            manualTargetJointAngles[2] -= 50f * Time.deltaTime;
            manualTargetJointAngles[2] = Mathf.Clamp(manualTargetJointAngles[2], -90f, 90f);
        }

        //Joint 3
        if (Input.GetKey(KeyCode.Alpha7))
        {
            manualMovement = true;
            manualTargetJointAngles[3] += 50f * Time.deltaTime;
            manualTargetJointAngles[3] = Mathf.Clamp(manualTargetJointAngles[3], -90f, 90f);
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            manualMovement = true;
            manualTargetJointAngles[3] -= 50f * Time.deltaTime;
            manualTargetJointAngles[3] = Mathf.Clamp(manualTargetJointAngles[3], -90f, 90f);
        }

        //Joint 4
        if (Input.GetKey(KeyCode.Alpha9))
        {
            manualMovement = true;
            manualTargetJointAngles[4] += 50f * Time.deltaTime;
            manualTargetJointAngles[4] = Mathf.Clamp(manualTargetJointAngles[4], -90f, 90f);
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            manualMovement = true;
            manualTargetJointAngles[4] -= 50f * Time.deltaTime;
            manualTargetJointAngles[4] = Mathf.Clamp(manualTargetJointAngles[4], -90f, 90f);
        }
        
    }

    public IEnumerator MoveToTargetPosition(float[] targetAngles)
    {
        // Copia los ángulos actuales del brazo para interpolar
        float[] startAngles = new float[5];
        for (int i = 0; i < 5; i++)
        {
            startAngles[i] = currentJointAngles[i];
        }

        float interpolationStep = 0f;

        // Interpolación desde los ángulos actuales hasta los ángulos objetivo
        while (interpolationStep < 1f)
        {
            interpolationStep += Time.deltaTime * speed;
            for (int i = 0; i < 5; i++)
            {
                // Interpolación lineal de cada ángulo
                currentJointAngles[i] = Mathf.Lerp(startAngles[i], targetAngles[i], interpolationStep);
            }
            // Aplicar los ángulos interpolados a cada joint
            SetJointAngles(currentJointAngles);
            yield return new WaitForEndOfFrame();
        }

        // Asegurarse de que los ángulos finales sean exactos
        for (int i = 0; i < 5; i++)
        {
            currentJointAngles[i] = targetAngles[i];
        }
        SetJointAngles(currentJointAngles);
    }

    private void SetJointAngles(float[] angles)
    {
        // Aplicar ángulos a cada joint:
        joints[0].localRotation = Quaternion.Euler(0, angles[0], 0);
        joints[1].localRotation = Quaternion.Euler(angles[1], 0, 0);
        joints[2].localRotation = Quaternion.Euler(angles[2], 0, 0);
        joints[3].localRotation = Quaternion.Euler(angles[3], 0, 0);
        joints[4].localRotation = Quaternion.Euler(angles[4], 0, 0);
    }

    void ResetToStart()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToTargetPosition(initialPos));
        for (int i = 0; i < 5; i++)
        {
            manualTargetJointAngles[i] = initialPos[i];
            currentJointAngles[i] = initialPos[i];
        }
    }
     

}
