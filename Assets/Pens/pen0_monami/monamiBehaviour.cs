using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monamiBehaviour : MonoBehaviour
{
    private bool knocked = false;
    private bool knockstart = false;
    private bool offnow = false;

    public Vector3 startPos;
    public Vector3 knockPos;
    public Vector3 outPos;
    private float knockc = 0;

    static public float knockCount = 0;
    public GameObject AchivementBox;
    public GameObject AchivementText;
    public AudioSource monamiKnockoff;
    public GameManager gamemanager;
    public GuideBehaviour guidebehaviour;

    void Start()
    {
        //GameManager gamemanager = gameObject.AddComponent<GameManager>() as GameManager;
        //this.gamemanager = GameManager();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector3 startPos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(startPos);

            //Debug.Log(startPos.y);
        }*/

        Vector3 knockPos = Input.mousePosition;
        knockPos = Camera.main.ScreenToWorldPoint(knockPos);

        //볼펜누르기
        if (knocked == false)
        {
            if (/*-.3 < knockPos.x && knockPos.x < .3 &&*/ knockPos.y < 2.2)
            {
                if (Input.GetMouseButton(0))
                {
                    if (gamemanager.firstGame == 0) guidebehaviour.firstgame2();

                    knockPos = Input.mousePosition;
                    knockPos = Camera.main.ScreenToWorldPoint(knockPos);

                    if (knockstart == false)
                    {
                        knockc = 2.2f - knockPos.y;
                        knockstart = true;
                    }

                    if (1.2 < knockPos.y) transform.position = new Vector3(0, knockPos.y + knockc - 3.2f, 0);
                }
            }

            //knockon
            if (Input.GetMouseButtonUp(0))
            {
                knockstart = false;
                /*if (-.3 < knockPos.x && knockPos.x < .3)
                {*/
                    outPos = Input.mousePosition;
                    outPos = Camera.main.ScreenToWorldPoint(outPos);


                    //Debug.Log(outPos);

                    if (outPos.y < 1.2f) outPos.y = 1.2f;

                    if (knockc - outPos.y < -1.0f && outPos.y < 1.3f && offnow == false)
                    {
                        if (gamemanager.firstGame == 0) StartCoroutine(guidebehaviour.firstgame4());

                        knocked = true;
                        transform.position = new Vector3(0, -1.9f, 0);
                        knockCount += 1;
                        //Debug.Log(knockCount);
                        GetComponent<AudioSource>().Play();

                        if (knockCount == 10)
                        {
                            StartCoroutine(AchivePen1());
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(0, -1, 0);

                        if (offnow == false && knockc - outPos.y < 0)
                        {
                            if (gamemanager.firstGame == 0) StartCoroutine(guidebehaviour.firstgame3());
                            monamiKnockoff.volume = 1 / Mathf.Pow(knockc - outPos.y, 2);
                            monamiKnockoff.GetComponent<AudioSource>().Play();
                        }
                        offnow = false;
                    }
                //}
            }
        }

        //볼펜 빼기 및 탁탁
        if (knocked == true)
        {
            //탁탁
            if (-.3 < knockPos.x && knockPos.x < .3 && 1.2 < knockPos.y && knockPos.y < 1.3)
            {
                if (Input.GetMouseButton(0))
                {
                    knockPos = Input.mousePosition;
                    knockPos = Camera.main.ScreenToWorldPoint(knockPos);

                    transform.position = new Vector3(0, knockPos.y - 3.2f, 0);
                }
            }
            else if (Input.GetMouseButtonUp(0)) transform.position = new Vector3(0, -1.9f, 0);
            else if (-.2f < knockPos.x && knockPos.x < .2f && -1.3f < knockPos.y && knockPos.y < -.9f) //빼기
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (gamemanager.firstGame == 0) StartCoroutine(guidebehaviour.firstgame5());

                    transform.position = new Vector3(0, -1, 0);
                    knocked = false;
                    offnow = true;

                    monamiKnockoff.GetComponent<AudioSource>().volume = 1;
                    monamiKnockoff.GetComponent<AudioSource>().Play();
                }
            }


        }
    }
    IEnumerator AchivePen1()
    {
        yield return null;

        gamemanager.unlockpen(1);
        gamemanager.SaveGame();

        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "제*스트림을 획득하였습니다.";

        yield return new WaitForSeconds(5f);

        AchivementBox.SetActive(false);
    }
}
