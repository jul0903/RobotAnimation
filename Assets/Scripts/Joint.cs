using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    public Vector3 axis = Vector3.zero; //tiene el eje por el cual va a girar ej:(0,1,0) mueve en y
    public Vector3 startOffset;

    private void Awake()
    {
        startOffset = this.transform.localPosition;
    }
}
