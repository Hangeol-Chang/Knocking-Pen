using System;
using UnityEngine;

// Monami 153 on Pen0: no click. Push the button all the way down and let go to knock.
// While knocked: tap the button to wiggle it, press the tip to pull it back out.
// Raises events for the tutorial (GuideBehaviour).
public class MonamiPen : MonoBehaviour
{
    [SerializeField] float pressTop = 2.2f;
    [SerializeField] float pressBottom = 1.2f;
    [SerializeField] float knockMaxY = 1.3f;   // release must happen below this
    [SerializeField] float knockDepth = 1.0f;  // and at least this far below the grab point
    [SerializeField] float pressOffset = 3.2f;
    [SerializeField] float restY = -1f;
    [SerializeField] float knockedY = -1.9f;
    [SerializeField] Rect tapArea = new Rect(-0.3f, 1.2f, 0.6f, 0.1f);
    [SerializeField] Rect pullArea = new Rect(-0.2f, -1.3f, 0.4f, 0.4f);
    [SerializeField] AudioSource releaseSound;
    [SerializeField] GameManager gamemanager;

    public event Action Pressed;
    public event Action Missed;
    public event Action Knocked;
    public event Action Pulled;

    AudioSource knockSound;
    bool isKnocked;
    bool dragStarted;
    bool ignoreNextRelease;  // the release right after pulling out is not a knock attempt
    float grabOffset;

    void Awake() => knockSound = GetComponent<AudioSource>();

    void Update()
    {
        Vector3 p = Pointer.World;
        if (isKnocked) UpdateKnocked(p);
        else UpdateRetracted(p);
    }

    void UpdateRetracted(Vector3 p)
    {
        if (p.y < pressTop && Input.GetMouseButton(0))
        {
            Pressed?.Invoke();
            if (!dragStarted)
            {
                grabOffset = pressTop - p.y;
                dragStarted = true;
            }
            if (pressBottom < p.y) SetY(p.y + grabOffset - pressOffset);
        }

        if (!Input.GetMouseButtonUp(0)) return;

        dragStarted = false;
        float outY = Mathf.Max(p.y, pressBottom);
        float pull = grabOffset - outY;

        if (pull < -knockDepth && outY < knockMaxY && !ignoreNextRelease)
        {
            isKnocked = true;
            SetY(knockedY);
            knockSound.Play();
            gamemanager.RecordKnock();
            Knocked?.Invoke();
            return;
        }

        SetY(restY);
        if (!ignoreNextRelease && pull < 0)
        {
            Missed?.Invoke();
            releaseSound.volume = 1 / (pull * pull);
            releaseSound.Play();
        }
        ignoreNextRelease = false;
    }

    void UpdateKnocked(Vector3 p)
    {
        if (tapArea.Contains(p))
        {
            if (Input.GetMouseButton(0)) SetY(p.y - pressOffset);
        }
        else if (Input.GetMouseButtonUp(0)) SetY(knockedY);
        else if (pullArea.Contains(p) && Input.GetMouseButtonDown(0))
        {
            isKnocked = false;
            ignoreNextRelease = true;
            SetY(restY);
            releaseSound.volume = 1;
            releaseSound.Play();
            Pulled?.Invoke();
        }
    }

    void SetY(float y) => transform.position = new Vector3(0, y, 0);
}
