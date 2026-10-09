using UnityEngine;

// Mechanical pencil (Suneung, Graph1000). Every press past clickY clicks once;
// lifting the pointer above rearmY (or releasing) arms the next click.
public class SharpPen : MonoBehaviour
{
    [SerializeField] float pressTop = 3.4f;
    [SerializeField] float pressBottom = 2.8f;
    [SerializeField] float clickY = 2.9f;
    [SerializeField] float rearmY = 3.2f;
    [SerializeField] float pressOffset = 0.6f;
    [SerializeField] float restY = -0.45f;
    [SerializeField] GameManager gamemanager;

    const float GrabTolerance = 0.2f;

    AudioSource clickSound;
    bool armed = true;
    bool dragStarted;
    float grabOffset;

    void Awake() => clickSound = GetComponent<AudioSource>();

    void Update()
    {
        Vector3 p = Pointer.World;

        if (p.y < pressTop && Input.GetMouseButton(0))
        {
            if (!dragStarted)
            {
                grabOffset = pressTop - p.y;
                dragStarted = true;
            }
            if (pressBottom < p.y && p.y < pressTop - grabOffset)
            {
                SetY(p.y + grabOffset - pressTop - pressOffset);
                if (grabOffset < GrabTolerance && p.y < clickY && armed)
                {
                    clickSound.Play();
                    armed = false;
                    gamemanager.RecordKnock();
                }
            }
            if (p.y > rearmY) armed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragStarted = false;
            armed = true;
            SetY(restY);
        }
    }

    void SetY(float y) => transform.position = new Vector3(0, y, 0);
}
