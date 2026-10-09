using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBehaviour : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject img1;
    public GameObject img2;
    public GameObject img3;
    public GameObject AchivementBox;
    public GameObject AchivementText;

    void Start()
    {
        img1.SetActive(false);
        img2.SetActive(false);
        img3.SetActive(false);

        if (gamemanager.firstGame == 0) StartCoroutine(firstgame());
        else Destroy(this.gameObject);
    }

    void Update()
    {
        if(gamemanager.firstGame ==1) Destroy(this.gameObject);
    }
    IEnumerator firstgame()
    {
        yield return null;
        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "버튼을 눌러보세세요.";

        img1.SetActive(true);

        yield return new WaitForSeconds(3f);
        AchivementBox.SetActive(false);

        yield return null;
    }

    public void firstgame2()
    {
        img1.SetActive(false);
    }
    public IEnumerator firstgame3()
    {
        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "버튼을 끝까지 눌러보세세요.";

        img1.SetActive(true);

        yield return new WaitForSeconds(3f);
        AchivementBox.SetActive(false);
    }
    public IEnumerator firstgame4()
    {
        img2.SetActive(true);
        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "버튼을 누르면 볼펜이 올라옵니다.";
        yield return new WaitForSeconds(3.0f);

        AchivementBox.SetActive(false);
        yield return null;
    }
    public IEnumerator firstgame5()
    {
        gamemanager.firstGame = 1;

        img2.SetActive(false);
        gamemanager.unlockpen(2);

        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "새로운 볼펜을 획득하였습니다.";
        yield return new WaitForSeconds(3.0f);

        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "다양한 볼펜이 준비되어 있습니다.";
        yield return new WaitForSeconds(3.0f);

        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "숨겨진 도전과제들을 완료하여";
        yield return new WaitForSeconds(3.0f);

        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "다양한 볼펜을 획득하세요.";
        yield return new WaitForSeconds(3.0f);

        img3.SetActive(true);
        AchivementBox.SetActive(true);
        AchivementText.GetComponent<Text>().text = "화면 우측을 눌러 다른 펜으로 이동합니다.";
        yield return new WaitForSeconds(3.0f);

        AchivementBox.SetActive(false);

        yield return null;
    }
}
