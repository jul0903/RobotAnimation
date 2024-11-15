using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    // Array de 5 articulaciones del brazo robótico
    public Transform[] joints = new Transform[5];

    // Ángulos actuales y objetivos de las articulaciones (en grados)
    public float[] currentJointAngles = new float[5];
    public float[] targetJointAngles = new float[5];

    // Velocidad de interpolación para el movimiento
    public float speed = 1.0f;

    void Start()
    {
        // Inicializar los ángulos actuales con los valores de las rotaciones iniciales
        for (int i = 0; i < joints.Length; i++)
        {
            // Asumimos que los joints están orientados inicialmente según sus rotaciones locales
            currentJointAngles[i] = joints[i].localRotation.eulerAngles.y;
        }
    }

    void Update()
    {
        // Iniciar el movimiento hacia los ángulos objetivo al presionar la tecla "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MoveToPosition(targetJointAngles));
        }

        // Ajustar los ángulos objetivo con teclas de prueba
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Presiona teclas 1-5 para incrementar el ángulo
            {
                targetJointAngles[i] += 10f; // Incrementa el ángulo objetivo
            }
        }
    }
    
    /// Mueve el brazo robótico hacia los ángulos objetivo especificados.
    /// <param name="targetAngles">Array con los ángulos objetivo para cada joint.</param>
    private IEnumerator MoveToPosition(float[] targetAngles)
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

    /// Aplica los ángulos actuales a las articulaciones del brazo robótico.
    /// <param name="angles">Array de ángulos en grados para cada joint.</param>
    private void SetJointAngles(float[] angles)
    {
        // Aplicar ángulos a cada joint:
        joints[0].localRotation = Quaternion.Euler(-90, 40, angles[0]); 
        joints[1].localRotation = Quaternion.Euler(angles[1], 0, -180); 
        joints[2].localRotation = Quaternion.Euler(angles[2], 0, 0);
        joints[3].localRotation = Quaternion.Euler(0, 0, angles[3]);
        joints[4].localRotation = Quaternion.Euler(angles[4], 0, 0);
    }
}