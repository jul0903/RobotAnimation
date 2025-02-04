using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AR1RobotController : MonoBehaviour
{
    // Array de 5 articulaciones del brazo robótico
    public Transform[] joints = new Transform[5];
    float[] initialPos = new float[5];

    // Angulos actuales y objetivos de las articulaciones (en grados)
    public float[] currentJointAngles = new float[5];
    public float[] autoTargetJointAngles = new float[5];
    public float[] manualTargetJointAngles = new float[5];

    // Velocidad de interpolación para el movimiento
    public float speed = 1.0f;


    //movimiento manual o predefinido
    bool manualMovement = false;

    private void Start()
    {
        //Nos guardamos las posiciones iniciales del brazo
        /*initialPos[0] = joints[0].localEulerAngles.y ;
        initialPos[1] = joints[1].localEulerAngles.x;
        initialPos[2] = joints[2].localEulerAngles.x;
        initialPos[3] = joints[3].localEulerAngles.x;
        initialPos[4] = joints[4].localEulerAngles.x ;*/
        initialPos[0] = 0;
        initialPos[1] = 0;
        initialPos[2] = -42.872f;
        initialPos[3] = -40.43f;
        initialPos[4] = -41.725f;

    }
    void Update()
    {
        // Iniciar el movimiento hacia los ángulos objetivo al presionar la tecla "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            manualMovement = false;
            StartCoroutine(MoveToTargetPosition(autoTargetJointAngles));
        }

        //Movimiento en tiempo real

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            manualMovement = true;
            manualTargetJointAngles[0] -= 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            manualMovement = true;
            manualTargetJointAngles[0] += 5;
        }

        if(manualMovement)
         StartCoroutine(MoveToTargetPosition(manualTargetJointAngles));

        //Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            manualMovement = false;
            ResetToStart();
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

        float journey = 0f;

        // Interpolación desde los ángulos actuales hasta los ángulos objetivo
        while (journey < 1f)
        {
            journey += Time.deltaTime * speed;
            for (int i = 0; i < 5; i++)
            {
                // Interpolación lineal de cada ángulo
                currentJointAngles[i] = Mathf.Lerp(startAngles[i], targetAngles[i], journey);
            }
            // Aplicar los ángulos interpolados a cada joint
            SetJointAngles(currentJointAngles);
            yield return null;
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
        StartCoroutine(MoveToTargetPosition(initialPos));
    }
     

}
