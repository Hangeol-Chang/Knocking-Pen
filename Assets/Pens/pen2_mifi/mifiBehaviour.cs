using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mifiBehaviour : MonoBehaviour
{
    private bool knocked = false;
    private bool Clicked = false;
    private bool knockstart = false;
    private bool offnow = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;

    private float knockc = 0;
    private float ylimit = 3.35f;
    private float ylimit2 = 2.3f;
    private float ClickPos = 2.4f;
    private float knockinyPos = -1.4f;
    private float const1 = 0.6f;
    private float startyPos = -0.5f;

    public AudioSource off1;
    public AudioSource off2;
    public AudioSource on2;

    static public float knockCount = 0;
    static public int NonKnockCount = 0;
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

        if (knocked == true)
        {
            //탁탁
            if (ylimit2 < knockPos.y && knockPos.y <ClickPos)
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position = new Vector3(0, knockPos.y-3.85f, 0);

                    if (knockPos.y < ClickPos)
                    {
                        if (Clicked == true) off1.GetComponent<AudioSource>().Play(); 
                        Clicked = false;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (Clicked == false)
                {
                    knocked = false;
                    offnow = true;
                    transform.position = new Vector3(0, 1f, 0);

                    off2.volume = 1;
                    off2.GetComponent<AudioSource>().Play();
                }
                else transform.position = new Vector3(0, knockinyPos, 0);
            }
        }

        //볼펜누르기
        if (knocked == false)
        {
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
                        transform.position = new Vector3(0, knockPos.y + knockc - ylimit -const1, 0);
                        if (knockc < 0.2 && knockPos.y < ClickPos && Clicked == false)
                        {
                            GetComponent<AudioSource>().Play();
                            Clicked = true;
                        }
                    }
                }
            }

            //knockon
            if (Input.GetMouseButtonUp(0))
            {
                knockstart = false;

                outPos = Input.mousePosition;
                outPos = Camera.main.ScreenToWorldPoint(outPos);

                if (outPos.y < ylimit2) outPos.y = ylimit2;

                if (Clicked == true && offnow == false)
                {
                    knocked = true;
                    transform.position = new Vector3(0, knockinyPos, 0);
                    knockCount += 1;
                    //Debug.Log(knockCount);
                    on2.GetComponent<AudioSource>().Play();

                    if (knockCount == 10)
                    {
                        //StartCoroutine(AchivePen(3));
                    }
                }
                else
                {
                    transform.position = new Vector3(0, startyPos, 0);
                    //Debug.Log(knockc - outPos.y);
                    if (offnow == false && knockc - outPos.y < 0)
                    {
                        NonKnockCount++;
                        off2.volume = 1 / Mathf.Pow(knockc - outPos.y, 2);
                        off2.GetComponent<AudioSource>().Play();
                        if (NonKnockCount == 50)
                        {
                            //StartCoroutine(AchivePen(10));
                        }
                    }
                    offnow = false;
                }
            }
        }

        //볼펜 빼기 및 탁탁

    }
    IEnumerator AchivePen(int num)
    {
        yield return null;

        gamemanager.unlockpen(num);
        AchivementBox.SetActive(true);
        if (num == 11) AchivementText.GetComponent<Text>().text = "아직 펜이 준비되지 않았습니다.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);


    }
}
