using UnityEngine;

// Animal pen 3: the knock is relative to where the drag starts, so it can be pressed anywhere below pressTop.
public class AnimalTwoPen3 : MonoBehaviour
{
    [SerializeField] float pressTop = 3.8f;
    [SerializeField] float pressDepth = 0.4f;     // how far the button travels
    [SerializeField] float clickDepth = 0.2f;     // depth that clicks it
    [SerializeField] float pressOffset = 0.5f;
    [SerializeField] float tapDepth = 0.1f;       // travel while knocked
    [SerializeField] float tapClickDepth = 0.07f; // depth that releases it while knocked
    [SerializeField] float restY = -0.5f;
    [SerializeField] float knockedY = -0.8f;
    [SerializeField] AudioSource tapOffSound;
    [SerializeField] AudioSource releaseSound;
    [SerializeField] AudioSource knockSound;
    [SerializeField] GameManager gamemanager;

    AudioSource clickSound;
    bool isKnocked;
    bool clickPassed;
    bool dragStarted;
    float anchorY;

    void Awake() => clickSound = GetComponent<AudioSource>();

    void Update()
    {
        Vector3 p = Pointer.World;
        if (isKnocked) UpdateKnocked(p);
        else UpdateRetracted(p);
    }

    // Depth below the point where the current drag started.
    float DragDepth(float y)
    {
        if (!dragStarted)
        {
            anchorY = y;
            dragStarted = true;
        }
        return anchorY - y;
    }

    void UpdateKnocked(Vector3 p)
    {
        if (p.y < pressTop + knockedY - restY && Input.GetMouseButton(0))
        {
            float depth = DragDepth(p.y);
            if (depth < tapDepth - knockedY)
            {
                SetY(knockedY - Mathf.Min(depth, tapDepth));
                if (depth > tapClickDepth && clickPassed)
                {
                    tapOffSound.Play();
                    clickPassed = false;
                }
            }
        }

        if (!Input.GetMouseButtonUp(0)) return;

        dragStarted = false;
        if (clickPassed)
        {
            SetY(knockedY);
            return;
        }

        isKnocked = false;
        SetY(restY);
        releaseSound.volume = 1;
        releaseSound.Play();
    }

    void UpdateRetracted(Vector3 p)
    {
        if (p.y < pressTop && Input.GetMouseButton(0))
        {
            float depth = DragDepth(p.y);
            if (0 < depth && depth < pressDepth)
            {
                SetY(-depth - pressOffset);
                if (depth > clickDepth && !clickPassed)
                {
                    clickSound.Play();
                    clickPassed = true;
                }
            }
        }

        if (!Input.GetMouseButtonUp(0)) return;

        dragStarted = false;
        if (clickPassed)
        {
            isKnocked = true;
            SetY(knockedY);
            knockSound.Play();
            gamemanager.RecordKnock();
            return;
        }

        SetY(restY);
        float pull = anchorY - p.y;
        releaseSound.volume = 1 / (pull * pull);
        releaseSound.Play();
        gamemanager.RecordMiss();
    }

    void SetY(float y) => transform.position = new Vector3(0, y, 0);
}
