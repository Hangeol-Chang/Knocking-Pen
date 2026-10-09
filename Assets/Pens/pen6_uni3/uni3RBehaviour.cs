using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uni3RBehaviour : MonoBehaviour
{

    public GameObject penb;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = penb.transform.position;
    }
}
