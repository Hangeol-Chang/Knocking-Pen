using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animaltwopen3Behaviour : MonoBehaviour
{
    private bool knocked = false;
    private bool Clicked = false;
    private bool knockstart = false;
    private bool offnow = false;

    public Vector3 knockPos;

    private float knockc = 0;
    private float ylimit = 3.8f;
    private float ylimit2 = 1.8f;
    private float knockinyPos = .8f;
    private float const1 = 0.5f;
    private float knockedlength = 0.1f;
    private float knocklength = 0.4f;
    private float startyPos = -0.5f;

    public AudioSource off1;
    public AudioSource off2;
    public AudioSource on2;

    static public float knockCount = 0;
    static public int NonKnockCount = 0;
    public GameObject AchivementBox;
    public GameObject AchivementText;
    public GameManager gamemanager;

    // Update is called once per frame
    void Update()
    {
        Vector3 knockPos = Input.mousePosition;
        knockPos = Camera.main.ScreenToWorldPoint(knockPos);

        if (knocked == true)
        {
            //탁탁
            if (knockPos.y < ylimit - knockinyPos - startyPos)
            {
                if (Input.GetMouseButton(0))
                {
                    if (knockstart == false)
                    {
                        knockc = knockPos.y;
                        knockstart = true;
                    }
                    if (knockc - knockinyPos - knockedlength < knockPos.y)
                    {
                        if(knockc - knockPos.y > knockedlength) transform.position = new Vector3(0, - knockinyPos - knockedlength, 0);
                        else transform.position = new Vector3(0, knockPos.y - knockinyPos - knockc, 0);

                        if (knockc - knockPos.y > 0.07 && Clicked == true)
                        { off1.GetComponent<AudioSource>().Play();
                            Clicked = false;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                knockstart = false;

                if (Clicked == false)
                {
                    knocked = false;
                    offnow = true;
                    transform.position = new Vector3(0, 1f, 0);

                    off2.volume = 1;
                    off2.GetComponent<AudioSource>().Play();
                }
                else transform.position = new Vector3(0, -knockinyPos, 0);
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
                        knockc = knockPos.y;
                        knockstart = true;
                    }
                    if (knockc - knocklength < knockPos.y && knockPos.y < knockc)
                    {
                        transform.position = new Vector3(0, knockPos.y - knockc - const1, 0);
                        if (knockc - knockPos.y > .2 && Clicked == false)
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

                if (Clicked == true && offnow == false)
                {
                    knocked = true;
                    transform.position = new Vector3(0, -knockinyPos, 0);
                    knockCount += 1;
                    on2.GetComponent<AudioSource>().Play();

                    if (knockCount == 10)
                    {
                        //StartCoroutine(AchivePen(3));
                    }
                }
                else
                {
                    transform.position = new Vector3(0, startyPos, 0);

                    NonKnockCount++;
                    off2.volume = 1 / Mathf.Pow(knockc - knockPos.y, 2);
                    off2.GetComponent<AudioSource>().Play();

                    if (NonKnockCount == 50)
                    {
                        //StartCoroutine(AchivePen(10));
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
