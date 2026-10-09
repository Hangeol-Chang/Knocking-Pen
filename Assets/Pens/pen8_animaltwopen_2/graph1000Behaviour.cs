using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class graph1000Behaviour : MonoBehaviour
{
    private bool canknock = true;
    private bool knockstart = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;

    private float knockc = 0;
    private float ylimit = 3.3f;
    private float ylimit2 = 3.05f;
    private float canknockPos = 3.25f;
    private float ClickPos = 3.1f;
    private float const1 = 0.5f;
    private float startyPos = -0.45f;

    static public float knockCount = 0;
    public GameObject AchivementBox;
    public GameObject AchivementText;
    public GameManager gamemanager;

    void Start()
    {
        //GameManager gamemanager = gameObject.AddComponent<GameManager>() as GameManager;
        //this.gamemanager = GameManager();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 knockPos = Input.mousePosition;
        knockPos = Camera.main.ScreenToWorldPoint(knockPos);

        //샤프누르기
        if (knockPos.y < ylimit)
        {
            if (Input.GetMouseButton(0))
            {
                if (knockstart == false)
                {
                    knockc = ylimit - knockPos.y;
                    knockstart = true;
                }

                if (ylimit2 < knockPos.y && knockPos.y < ylimit - knockc)
                {
                    transform.position = new Vector3(0, knockPos.y + knockc - ylimit - const1, 0);
                    if (knockc < 0.2 && knockPos.y < ClickPos && canknock == true)
                    {
                        GetComponent<AudioSource>().Play();
                        canknock = false;
                        knockCount++;
                        //if (knockCount == 30) StartCoroutine(AchivePen(5));
                        //if (knockCount == 77) StartCoroutine(AchivePen(7));
                    }
                }

                if (knockPos.y > canknockPos && canknock == false) canknock = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            knockstart = false;
            canknock = true;

            transform.position = new Vector3(0, startyPos, 0);
        }

    }
    IEnumerator AchivePen(int num)
    {
        yield return null;

        gamemanager.unlockpen(num);
        AchivementBox.SetActive(true);
        if (num == 7) AchivementText.GetComponent<Text>().text = "그래프1000을 획득했습니다.";
        if (num == 5) AchivementText.GetComponent<Text>().text = "토끼펜을 획득했습니다.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);


    }
}