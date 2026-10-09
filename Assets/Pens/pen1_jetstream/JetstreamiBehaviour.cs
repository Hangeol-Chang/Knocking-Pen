using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetstreamiBehaviour : MonoBehaviour
{
    public JetstreamBehaviour jetstreambutton;
    public GameObject jetstream;

    void Update()
    {
        if (jetstreambutton.knocked == false && jetstreambutton.Clicked == false)
        {
            transform.position = new Vector3(0, jetstream.transform.position.y, 0);
        }
        if (jetstreambutton.knocked == true)
        {
            if(jetstreambutton.Clicked == true && jetstream.transform.position.y < -1.7)
                this.transform.position = new Vector3(0, jetstream.transform.position.y, 0);
            else if (jetstreambutton.Clicked == false)
                transform.position = new Vector3(0, jetstream.transform.position.y, 0);
        }
    }
}
