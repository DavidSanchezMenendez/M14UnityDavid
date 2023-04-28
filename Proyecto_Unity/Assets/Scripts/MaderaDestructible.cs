using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaderaDestructible : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
       

        if (other.tag == "NoInteractuarCamera")
        {
            rb.isKinematic = false;
            Debug.Log("HOLA");
        }
    }
}
