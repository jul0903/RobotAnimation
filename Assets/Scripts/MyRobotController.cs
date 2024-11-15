using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    // Array de 5 articulaciones del brazo rob�tico
    public Transform[] joints = new Transform[5];

    // �ngulos actuales y objetivos de las articulaciones (en grados)
    public float[] currentJointAngles = new float[5];
    public float[] targetJointAngles = new float[5];

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
            StartCoroutine(MoveToPosition(targetJointAngles));
        }

        // Ajustar los �ngulos objetivo con teclas de prueba
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Presiona teclas 1-5 para incrementar el �ngulo
            {
                targetJointAngles[i] += 10f; // Incrementa el �ngulo objetivo
            }
        }
    }
    
    /// Mueve el brazo rob�tico hacia los �ngulos objetivo especificados.
    /// <param name="targetAngles">Array con los �ngulos objetivo para cada joint.</param>
    private IEnumerator MoveToPosition(float[] targetAngles)
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

            // Aplicar los �ngulos interpolados a cada joint
            SetJointAngles(currentJointAngles);
            yield return null;
        }

        // Asegurarse de que los �ngulos finales sean exactos
        for (int i = 0; i < 5; i++)
        {
            currentJointAngles[i] = targetAngles[i];
        }
        SetJointAngles(currentJointAngles);
    }

    /// Aplica los �ngulos actuales a las articulaciones del brazo rob�tico.
    /// <param name="angles">Array de �ngulos en grados para cada joint.</param>
    private void SetJointAngles(float[] angles)
    {
        // Aplicar �ngulos a cada joint:
        joints[0].localRotation = Quaternion.Euler(-90, 40, angles[0]); 
        joints[1].localRotation = Quaternion.Euler(angles[1], 0, -180); 
        joints[2].localRotation = Quaternion.Euler(angles[2], 0, 0);
        joints[3].localRotation = Quaternion.Euler(0, 0, angles[3]);
        joints[4].localRotation = Quaternion.Euler(angles[4], 0, 0);
    }
}