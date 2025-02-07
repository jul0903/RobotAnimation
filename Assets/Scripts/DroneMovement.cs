using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    //Array de todas las posiciones predefinidas del dron
    public Transform[] positions;
    int actualPos = 0;


    private void Update()
    {
        MoveDrone();
    }

    void MoveDrone()
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
                this.transform.position = Vector3.Lerp(this.transform.position, positions[actualPos].position, Time.deltaTime );
            }
        }
        else
        {
            actualPos = 0; 
        }
    }
}
