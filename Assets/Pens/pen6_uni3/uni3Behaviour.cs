using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uni3Behaviour : MonoBehaviour
{
    private bool knockedL = false;
    private bool knockedR = false;
    private bool knockedM = false;
    private bool Clicked = false;
    private bool knockstart = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;

    private float knockc = 0;
    private float ylimit = 2.9f;
    private float ylimit2 = 1.8f;
    private float ClickPos = 2.2f;
    private float knockinyPos = -1.4f;
    private float const1 = .5f;
    private float startyPos = -.5f;

    public AudioSource off1;

    public GameObject Right;
    public GameObject Left;
    public GameObject Middle;
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
        else if (knockPos.x > -0.2 && knockPos.x <0.2 && knockedM == false) pen = Middle;
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

                        if (pen.transform.position.y < -.25f && pen != empty)
                        {
                            if (knockedL == true)
                            {
                                knockedL = false;
                                Left.transform.position = new Vector3(0, startyPos, 0);
                                off1.volume = 1;
                                off1.GetComponent<AudioSource>().Play();
                            }else if (knockedR == true)
                            {
                                knockedR = false;
                                Right.transform.position = new Vector3(0, startyPos, 0);
                                off1.volume = 1;
                                off1.GetComponent<AudioSource>().Play();
                            }else if(knockedM == true){
                                knockedM = false;
                                Middle.transform.position = new Vector3(0, startyPos, 0);
                                off1.volume = 1;
                                off1.GetComponent<AudioSource>().Play();
                            }
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
                else if (pen == Left) knockedL = true;
                else knockedM = true;

                pen.transform.position = new Vector3(0, knockinyPos, 0);
                knockCount++;
                Clicked = false;
            }
            else if (knockedL == false && knockedR == false && knockedM == false)
            {
                pen.transform.position = new Vector3(0, startyPos, 0);
                //Debug.Log(knockc - outPos.y);

                off1.volume = 1 / Mathf.Pow(knockc - outPos.y, 2);
                off1.GetComponent<AudioSource>().Play();

                nonknockCount++;
                //Debug.Log(nonknockCount);
                if (nonknockCount == 200) StartCoroutine(AchivePen(29));
            }
        }
    }
    IEnumerator AchivePen(int num)
    {
        yield return null;

        gamemanager.unlockpen(num);
        AchivementBox.SetActive(true);
        if (num == 29) AchivementText.GetComponent<Text>().text = "업데이트를 기대해주세요ㅎㅎ.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);
    }


}