using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    //Array de todas las posiciones predefinidas del dron
    public Transform[] positions;
    int actualPos = 0;

    public float moveSpeed = 5f;  // Velocidad de movimiento
    public float ascendSpeed = 3f; // Velocidad de ascenso/descenso

    private bool manualMovement = true;

    public bool stopMovement = false;

    private void Update()
    {
        if (!stopMovement)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                manualMovement = !manualMovement;
            }

            if (manualMovement)
            {
                MoveDroneManual();
            }
            else
            {
                MoveDronePredefined();
            }
        }

    }
    void MoveDroneManual()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D o Flechas Izq/Der
        float vertical = Input.GetAxis("Vertical"); // W/S o Flechas Arriba/Abajo
        float ascend = 0f;

        // Subir con Q, bajar con E
        if (Input.GetKey(KeyCode.Q))
        {
            ascend = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            ascend = -1f;
        }

        // Calcular movimiento
        Vector3 moveDirection = new Vector3(horizontal, ascend * ascendSpeed, vertical).normalized;

        // Aplicar movimiento
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void MoveDronePredefined()
    {
        //Si la posicion es la ultima, vuelvo a la pos inicial. 
        if (actualPos < positions.Length)
        {
            //Si la distancia es mas pequeña que la tolerancia, paso a la siguiente posicion, sino lerp hacia la actual
            if (Vector3.Distance(positions[actualPos].position, this.transform.position) < 2f)
            {
                actualPos++;
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, positions[actualPos].position, Time.deltaTime * 2f);
            }
        }
        else
        {
            actualPos = 0; 
        }
    }
}
