using UnityEngine;

// Click-type ballpoint (Jetstream, Mifi, white pen).
// Drag the button down past clickY to knock; while knocked, drag it down again to release.
// All positions are world-space y. Pen y while pressed = pointer y + grab offset - pressTop - pressOffset.
public class KnockPen : MonoBehaviour
{
    [Header("Press")]
    [SerializeField] float pressTop = 3.35f;
    [SerializeField] float pressBottom = 2.3f;
    [SerializeField] float clickY = 2.4f;
    [SerializeField] float pressOffset = 0.6f;

    [Header("Knocked")]
    [SerializeField] float tapTop = 2.4f;
    [SerializeField] float tapOffset = 3.85f;

    [Header("Rest")]
    [SerializeField] float restY = -0.5f;
    [SerializeField] float knockedY = -1.4f;

    [Header("Refs")]
    [SerializeField] AudioSource tapOffSound;
    [SerializeField] AudioSource releaseSound;
    [SerializeField] AudioSource knockSound;
    [SerializeField] GameManager gamemanager;

    // Grab must start this close to pressTop for the click to count.
    const float GrabTolerance = 0.2f;

    public bool IsKnocked { get; private set; }
    public bool ClickPassed { get; private set; }

    AudioSource clickSound;
    bool dragStarted;
    float grabOffset;

    void Awake() => clickSound = GetComponent<AudioSource>();

    void Update()
    {
        Vector3 p = Pointer.World;
        if (IsKnocked) UpdateKnocked(p);
        else UpdateRetracted(p);
    }

    void UpdateKnocked(Vector3 p)
    {
        if (pressBottom < p.y && p.y < tapTop && Input.GetMouseButton(0))
        {
            SetY(p.y - tapOffset);
            if (p.y < clickY)
            {
                if (ClickPassed) tapOffSound.Play();
                ClickPassed = false;
            }
        }

        if (!Input.GetMouseButtonUp(0)) return;

        if (ClickPassed)
        {
            SetY(knockedY);
            return;
        }

        IsKnocked = false;
        dragStarted = false;
        SetY(restY);
        releaseSound.volume = 1;
        releaseSound.Play();
    }

    void UpdateRetracted(Vector3 p)
    {
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
                if (grabOffset < GrabTolerance && p.y < clickY && !ClickPassed)
                {
                    clickSound.Play();
                    ClickPassed = true;
                }
            }
        }

        if (!Input.GetMouseButtonUp(0)) return;

        dragStarted = false;
        if (ClickPassed)
        {
            IsKnocked = true;
            SetY(knockedY);
            knockSound.Play();
            gamemanager.RecordKnock();
            return;
        }

        SetY(restY);
        float pull = grabOffset - Mathf.Max(p.y, pressBottom);
        if (pull < 0)
        {
            releaseSound.volume = 1 / (pull * pull);
            releaseSound.Play();
            gamemanager.RecordMiss();
        }
    }

    void SetY(float y) => transform.position = new Vector3(0, y, 0);
}
