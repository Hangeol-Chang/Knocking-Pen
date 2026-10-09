using System.Collections;
using UnityEngine;

// First-run tutorial on Pen0. Listens to MonamiPen events and removes itself when finished.
public class GuideBehaviour : MonoBehaviour
{
    public GameManager gamemanager;
    public MonamiPen pen;
    public GameObject img1;
    public GameObject img2;
    public GameObject img3;

    const float MessageSeconds = 3f;

    void Start()
    {
        img1.SetActive(false);
        img2.SetActive(false);
        img3.SetActive(false);

        if (SaveService.Data.tutorialDone)
        {
            Destroy(gameObject);
            return;
        }

        Subscribe(true);
        img1.SetActive(true);
        gamemanager.Notify("버튼을 눌러보세요.", MessageSeconds);
    }

    void OnDestroy() => Subscribe(false);

    void Subscribe(bool on)
    {
        if (pen == null) return;
        if (on)
        {
            pen.Pressed += OnPressed;
            pen.Missed += OnMissed;
            pen.Knocked += OnKnocked;
            pen.Pulled += OnPulled;
        }
        else
        {
            pen.Pressed -= OnPressed;
            pen.Missed -= OnMissed;
            pen.Knocked -= OnKnocked;
            pen.Pulled -= OnPulled;
        }
    }

    void OnPressed() => img1.SetActive(false);

    void OnMissed()
    {
        img1.SetActive(true);
        gamemanager.Notify("버튼을 끝까지 눌러보세요.", MessageSeconds);
    }

    void OnKnocked()
    {
        img2.SetActive(true);
        gamemanager.Notify("버튼을 누르면 볼펜이 올라옵니다.", MessageSeconds);
    }

    void OnPulled()
    {
        Subscribe(false);
        StartCoroutine(Finish());
    }

    IEnumerator Finish()
    {
        SaveService.Data.tutorialDone = true;
        img2.SetActive(false);
        gamemanager.Unlock(2);

        gamemanager.Notify("새로운 볼펜을 획득하였습니다.", MessageSeconds);
        gamemanager.Notify("다양한 볼펜이 준비되어 있습니다.", MessageSeconds);
        gamemanager.Notify("숨겨진 도전과제들을 완료하여", MessageSeconds);
        gamemanager.Notify("다양한 볼펜을 획득하세요.", MessageSeconds);
        yield return new WaitForSeconds(MessageSeconds * 4);

        img3.SetActive(true);
        gamemanager.Notify("화면 우측을 눌러 다른 펜으로 이동합니다.", MessageSeconds);
        yield return new WaitForSeconds(MessageSeconds);

        img3.SetActive(false);
        Destroy(gameObject);
    }
}
