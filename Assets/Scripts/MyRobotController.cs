using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    // Array de 5 articulaciones del brazo rob�tico
    public Transform[] joints = new Transform[5];

    // Angulos actuales y objetivos de las articulaciones (en grados)
    public float[] currentJointAngles = new float[5];
    public float[] targetJointAngles = new float[5];
    public float[] ex1JointAngles = new float[5];
    public bool hasCube = false;

    // Velocidad de interpolaci�n para el movimiento
    public float speed = 1.0f;

    void Start()
    {
        // Inicializar los �ngulos actuales con los valores de las rotaciones iniciales
        for (int i = 0; i < joints.Length; i++)
        {
            // Asumimos que los joints est�n orientados inicialmente seg�n sus rotaciones locales
            currentJointAngles[i] = joints[i].localRotation.eulerAngles.y;
        }
    }

    void Update()
    {
        // Iniciar el movimiento hacia los �ngulos objetivo al presionar la tecla "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MoveToTargetPosition(targetJointAngles));
            hasCube = true;
        }
    }
    
    public IEnumerator MoveToTargetPosition(float[] targetAngles)
    {
        // Copia los �ngulos actuales del brazo para interpolar
        float[] startAngles = new float[5];
        for (int i = 0; i < 5; i++)
        {
            startAngles[i] = currentJointAngles[i];
        }

        float journey = 0f;

        // Interpolaci�n desde los �ngulos actuales hasta los �ngulos objetivo
        while (journey < 1f)
        {
            journey += Time.deltaTime * speed;
            for (int i = 0; i < 5; i++)
            {
                // Interpolaci�n lineal de cada �ngulo
                currentJointAngles[i] = Mathf.Lerp(startAngles[i], targetAngles[i], journey);
            }

            if(hasCube == false)
            {
                // Aplicar los �ngulos interpolados a cada joint
                SetJointAngles(currentJointAngles);
                yield return null;
            }
            else
            {
                SetEx1Angles(ex1JointAngles);
                yield return null;
            }
        }

        // Asegurarse de que los �ngulos finales sean exactos
        for (int i = 0; i < 5; i++)
        {
            currentJointAngles[i] = targetAngles[i];
        }
        if (hasCube == false)
        {
            // Aplicar los �ngulos interpolados a cada joint
            SetJointAngles(currentJointAngles);
        }
        else
        {
            // Aplicar los �ngulos interpolados a cada joint
            SetEx1Angles(ex1JointAngles);
        }
        SetJointAngles(currentJointAngles);
    }

    private void SetJointAngles(float[] angles)
    {
        // Aplicar �ngulos a cada joint:
        joints[0].localRotation = Quaternion.Euler(0, angles[0], 0); 
        joints[1].localRotation = Quaternion.Euler(angles[1], 0, 0); 
        joints[2].localRotation = Quaternion.Euler(angles[2], 0, 0);
        joints[3].localRotation = Quaternion.Euler(angles[3], 0, 0);
        joints[4].localRotation = Quaternion.Euler(0, 0, angles[4]);
    }

    private void SetEx1Angles(float[] angles)
    {
        joints[0].localRotation = Quaternion.Euler(0, angles[0], 0);
        joints[1].localRotation = Quaternion.Euler(angles[1], 0, 0);
        joints[2].localRotation = Quaternion.Euler(angles[2], 0, 0);
        joints[3].localRotation = Quaternion.Euler(angles[3], 0, 0);
        joints[4].localRotation = Quaternion.Euler(0, 0, angles[4]);
    }    
}