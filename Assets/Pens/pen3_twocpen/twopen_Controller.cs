using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class twopen_Controller : MonoBehaviour
{
    private bool knockedL = false;
    private bool knockedR = false;
    private bool Clicked = false;
    private bool knockstart = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;

    private float knockc = 0;
    private float ylimit = 3.1f;
    private float ylimit2 = 2.0f;
    private float ClickPos = 2.1f;
    private float knockinyPos = -1.0f;
    private float const1 = 0f;
    private float startyPos = 0f;

    public AudioSource off1;

    public GameObject Right;
    public GameObject Left;
    public GameObject empty;
    private GameObject pen;

    static public float knockCount = 0;
    static public float nonknockCount = 0;
    public GameObject AchivementBox;
    public GameObject AchivementText;
    public GameManager gamemanager;


    void Update()
    {
        Vector3 knockPos = Input.mousePosition;
        knockPos = Camera.main.ScreenToWorldPoint(knockPos);

        if (knockPos.x > 0.2 && knockedR == false) pen = Right;
        else if (knockPos.x < -0.2 && knockedL == false) pen = Left;
        else pen = empty;

        //볼펜누르기
        if (knockPos.y < ylimit)
        {
            if (Input.GetMouseButton(0))
            {
                if (knockstart == false)
                {
                    knockc = ylimit - knockPos.y;
                    knockstart = true;
                }

                if (Clicked == false)
                {
                    if (ylimit2 < knockPos.y && knockPos.y < ylimit - knockc)
                    {
                        pen.transform.position = new Vector3(0, knockPos.y + knockc - ylimit - const1, 0);

                        if (knockedL == true && pen == Right && pen.transform.position.y < -.25f)
                        {
                            knockedL = false;
                            Left.transform.position = new Vector3(0, startyPos, 0);
                            off1.volume = 1;
                            off1.GetComponent<AudioSource>().Play();
                        }
                        if (knockedR == true && pen == Left && pen.transform.position.y < -.25f)
                        {
                            knockedR = false;
                            Right.transform.position = new Vector3(0, startyPos, 0);
                            off1.volume = 1;
                            off1.GetComponent<AudioSource>().Play();
                        }

                        if (knockc < 0.2 && knockPos.y < ClickPos && Clicked == false)
                        {
                            GetComponent<AudioSource>().Play();
                            Clicked = true;
                        }
                    }
                }
                else if (ylimit2 < knockPos.y && knockPos.y < ClickPos)
                    pen.transform.position = new Vector3(0, knockPos.y + knockc - ylimit - const1, 0);
            }
        }

        //knockon
        if (Input.GetMouseButtonUp(0))
        {
            knockstart = false;

            outPos = Input.mousePosition;
            outPos = Camera.main.ScreenToWorldPoint(outPos);

            if (outPos.y < ylimit2) outPos.y = ylimit2;

            if (Clicked == true)
            {
                if (pen == Right) knockedR = true;
                else knockedL = true;

                pen.transform.position = new Vector3(0, knockinyPos, 0);
                knockCount ++;
                Clicked = false;

                if (knockCount == 10)
                {
                    StartCoroutine(AchivePen(4));
                }
            }
            else if (knockedL == false && knockedR == false)
            {
                pen.transform.position = new Vector3(0, startyPos, 0);
                //Debug.Log(knockc - outPos.y);

                     off1.volume = 1 / Mathf.Pow(knockc - outPos.y, 2);
                     off1.GetComponent<AudioSource>().Play();

                    nonknockCount++;
                    Debug.Log(nonknockCount);
                    if (nonknockCount == 20) StartCoroutine(AchivePen(6));
            }
        }
    }
    IEnumerator AchivePen(int num)
    {
        yield return null;

        gamemanager.unlockpen(num);
        AchivementBox.SetActive(true);
        if (num == 4) AchivementText.GetComponent<Text>().text = "수능샤프를 획득하였습니다.";
        if (num == 6) AchivementText.GetComponent<Text>().text = "개발자가 고딩때 쓰던 볼펜을 획득하였습니다.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);
    }


}