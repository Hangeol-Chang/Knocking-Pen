using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuSet;
    public GameObject AchivementBox;
    public ObjID ObjID;
    //ObjID objID;

    int pennumber;
    public int firstGame = 0;

    int[] unlockedpen = new int[30];
    string unlockedpenSave = "";

    void Start()
    {
        menuSet.SetActive(false);
        AchivementBox.SetActive(false);
        //InvokeRepeating("SaveGame", 30f, 60f);

        unlockedpen[0] = 1;

        LoadGame();
        pennumber = ObjID.PenNumber;
        //monamiBehaviour.knockCount = 0;
        //SaveGame();

        //objID = GameObject.FindGameObjectWithTag("Pen").GetComponent<ObjID>();
        //pennumber = objID.PenNumber;

        //SaveReset();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (menuSet.activeSelf) menuSet.SetActive(false);
            else menuSet.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(worldPos);

            if (worldPos.x > 2 && worldPos.y <3)
            {
                int i = pennumber +1;
                while (i <= 28)
                {
                    //Debug.Log(unlockedpenSave);
                    //Debug.Log(unlockedpen[i]);
                    if (unlockedpen[i] == 1)
                    {
                        SaveGame();
                        SceneManager.LoadScene("Pen" + i);
                        pennumber++;
                        LoadGame();
                        Debug.Log(unlockedpenSave);
                        break;
                    }
                    else i++;
                }
            }

            //Debug.Log(worldPos.x + "," +  worldPos.y);
            if (worldPos.x < -2 && worldPos.y < 3)
            {
                int i = pennumber - 1;
                while (i >= 0)
                {
                    //Debug.Log(i);
                    if (unlockedpen[i] == 1)
                    {
                        SaveGame();
                        SceneManager.LoadScene("Pen" + i);
                        pennumber--;
                        LoadGame();
                        break;
                    }
                    else i--;
                }
            }
        }
    }
    public void Menu()
    {
        if (menuSet.activeSelf) menuSet.SetActive(false);
        else menuSet.SetActive(true);
    }

    public void GameExit()
    {
        SaveGame();
        Application.Quit();
    }
    public void SaveGame()
    {
        //저장하는 마크 등장
        PlayerPrefs.SetFloat("monamiknock", monamiBehaviour.knockCount);
        PlayerPrefs.SetFloat("jetstreamknock", JetstreamBehaviour.knockCount);
        //PlayerPrefs.SetInt("pennumber", pennumber);

        unlockedpenSave = "";
        for (int i= 0; i< unlockedpen.Length; i++)
        {
            unlockedpenSave = unlockedpenSave + unlockedpen[i];
            if (i < unlockedpen.Length - 1)
            {
                unlockedpenSave = unlockedpenSave + ",";
            }
        }
        PlayerPrefs.SetString("unlockedpen", unlockedpenSave);
        Debug.Log("저장한다~~");
        //PlayerPrefs.SetFloat("monamiclick")
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("monamiknock")) return;

        string[] dataArr = PlayerPrefs.GetString("unlockedpen").Split(','); //불러온 값을 ,에 대하여 잘라서 배열로 불러옴
        for(int i=0; i < 30; i++)
        {
            unlockedpen[i] = System.Convert.ToInt32(dataArr[i]);
            Debug.Log("Load" + unlockedpen[2]);
        }

        monamiBehaviour.knockCount = PlayerPrefs.GetFloat("monamiknock");
        JetstreamBehaviour.knockCount = PlayerPrefs.GetFloat("jetstreamknock");
        //pennumber = PlayerPrefs.GetInt("pennumber");
        firstGame = 1;

        //SceneManager.LoadScene("Pen" + pennumber);
    }

    public void unlockpen(int num)
    {
        unlockedpen[num] = 1;
        SaveGame();

        Debug.Log(unlockedpenSave);
    }
    
    public void DevelopScene()
    {
        SceneManager.LoadScene("DeveloperScene");
    }

    void SaveReset()
    {
        //개발 중 세이브 초기화용 코드. 건드리지 말것.
        PlayerPrefs.SetFloat("monamiknock", 0);
        PlayerPrefs.SetInt("pennumber", 0);

        unlockedpenSave = unlockedpenSave + 1;
        for (int i = 1; i < unlockedpen.Length; i++)
        {
            unlockedpenSave = unlockedpenSave + 0;
            if (i < unlockedpen.Length - 1)
            {
                unlockedpenSave = unlockedpenSave + ",";
            }
        }
        PlayerPrefs.SetString("unlockedpen", unlockedpenSave);
        Debug.Log("세이브 초기화");

        //PlayerPrefs.SetFloat("monamiclick")
    }
}
