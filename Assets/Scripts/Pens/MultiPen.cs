using UnityEngine;

// Multi-color pen (2-color, animal pens, uni 3-color). The pointer x picks the color:
// x > sideX -> right, x < -sideX -> left, otherwise middle (if any). One color is knocked at a time;
// pressing another color down past releaseOthersBelow releases it.
public class MultiPen : MonoBehaviour
{
    [Header("Press")]
    [SerializeField] float pressTop = 3.1f;
    [SerializeField] float pressBottom = 2.0f;
    [SerializeField] float clickY = 2.1f;
    [SerializeField] float pressOffset = 0f;
    [SerializeField] float sideX = 0.2f;
    [SerializeField] float releaseOthersBelow = -0.25f;

    [Header("Rest")]
    [SerializeField] float restY = 0f;
    [SerializeField] float knockedY = -1.0f;

    [Header("Refs")]
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
    [SerializeField] GameObject middle;
    [SerializeField] AudioSource releaseSound;
    [SerializeField] GameManager gamemanager;

    const float GrabTolerance = 0.2f;

    AudioSource clickSound;
    GameObject knocked;
    GameObject clicked;
    bool dragStarted;
    float grabOffset;

    void Awake() => clickSound = GetComponent<AudioSource>();

    GameObject PenAt(float x)
    {
        GameObject pen = x > sideX ? right : x < -sideX ? left : middle;
        return pen == knocked ? null : pen;
    }

    void Update()
    {
        Vector3 p = Pointer.World;
        GameObject pen = PenAt(p.x);

        if (p.y < pressTop && Input.GetMouseButton(0))
        {
            if (!dragStarted)
            {
                grabOffset = pressTop - p.y;
                dragStarted = true;
            }
            float pressedY = p.y + grabOffset - pressTop - pressOffset;

            if (clicked == null)
            {
                if (pen != null && pressBottom < p.y && p.y < pressTop - grabOffset)
                {
                    SetY(pen, pressedY);
                    if (knocked != null && pressedY < releaseOthersBelow)
                    {
                        SetY(knocked, restY);
                        knocked = null;
                        releaseSound.volume = 1;
                        releaseSound.Play();
                    }
                    if (grabOffset < GrabTolerance && p.y < clickY)
                    {
                        clickSound.Play();
                        clicked = pen;
                    }
                }
            }
            else if (pressBottom < p.y && p.y < clickY) SetY(clicked, pressedY);
        }

        if (!Input.GetMouseButtonUp(0)) return;

        dragStarted = false;
        if (clicked != null)
        {
            knocked = clicked;
            clicked = null;
            SetY(knocked, knockedY);
            gamemanager.RecordKnock();
        }
        else if (knocked == null)
        {
            if (pen != null) SetY(pen, restY);
            float pull = grabOffset - Mathf.Max(p.y, pressBottom);
            releaseSound.volume = 1 / (pull * pull);
            releaseSound.Play();
            gamemanager.RecordMiss();
        }
    }

    static void SetY(GameObject pen, float y) => pen.transform.position = new Vector3(0, y, 0);
}
