using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JetstreamBehaviour : MonoBehaviour
{
    public bool knocked = false;
    public bool Clicked = false;
    private bool knockstart = false;
    private bool offnow = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;

    private float knockc = 0;
    private float ylimit = 2f;
    private float knockinyPos = -1.7f;

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
            if (/*-.3 < knockPos.x && knockPos.x < .3 &&*/ 1.15 < knockPos.y && ylimit > knockPos.y)
            {
                if (Input.GetMouseButton(0))
                {
                    knockPos = Input.mousePosition;
                    knockPos = Camera.main.ScreenToWorldPoint(knockPos);

                    transform.position = new Vector3(0, knockPos.y - 3f, 0);

                    if (knockPos.y < 1.2)
                    {
                        if(Clicked == true) off1.GetComponent<AudioSource>().Play();
                        Clicked = false;

                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if(Clicked == false)
                {
                    knocked = false;
                    offnow = true;
                    transform.position = new Vector3(0, -1, 0);

                    off2.volume = 1;
                    off2.GetComponent<AudioSource>().Play();
                }else transform.position = new Vector3(0, knockinyPos, 0);
            }
        }

        //볼펜누르기
        if (knocked == false)
        {
            if (/*-.3 < knockPos.x && knockPos.x < .3 &&*/ knockPos.y < ylimit)
            {
                if (Input.GetMouseButton(0))
                {
                    if (knockstart == false)
                    {
                        knockc = ylimit - knockPos.y;
                        knockstart = true;
                    }
                    if (1.15 < knockPos.y && knockPos.y < ylimit -  knockc)
                    {
                        transform.position = new Vector3(0, knockPos.y + knockc - ylimit - 1f, 0);
                        if (knockc < 0.2 && knockPos.y < 1.2 && Clicked == false)
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

                if (outPos.y < 1.1f) outPos.y = 1.1f;

                    if (/*knockc - outPos.y < -.8f && outPos.y < 1.2f*/ Clicked == true && offnow == false)
                    {
                        knocked = true;
                        transform.position = new Vector3(0, knockinyPos, 0);
                        knockCount += 1;
                        //Debug.Log(knockCount);
                        on2.GetComponent<AudioSource>().Play();

                        if (knockCount == 10)
                        {
                            StartCoroutine(AchivePen(3));
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(0, -1f, 0);
                        //Debug.Log(knockc - outPos.y);
                        if (offnow == false && knockc - outPos.y < 0)
                        {
                            NonKnockCount++;
                        Debug.Log(NonKnockCount);
                            off2.volume = 1 / Mathf.Pow(knockc - outPos.y, 2);
                            off2.GetComponent<AudioSource>().Play();
                        if (NonKnockCount == 30)
                        {
                            StartCoroutine(AchivePen(10));
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
        if(num == 3) AchivementText.GetComponent<Text>().text = "2색펜을 획득하였습니다.";
        if (num == 10) AchivementText.GetComponent<Text>().text = "화이트펜을 획득하였습니다.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);


    }
}
