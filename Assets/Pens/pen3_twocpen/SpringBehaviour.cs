using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehaviour : MonoBehaviour
{
    public GameObject pen;
    private float comp = 1;

    void Update()
    {
        comp = 1+(pen.transform.position.y /5f);
        this.transform.localScale = new Vector3(1, comp, 1);
    }
}
