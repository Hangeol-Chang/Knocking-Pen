using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// One per pen scene (part of UI.prefab). Persistent state lives in SaveService.
public class GameManager : MonoBehaviour
{
    // Field names are kept as-is: scenes override them through UI.prefab.
    public GameObject menuSet;
    public GameObject AchivementBox;
    public ObjID ObjID;

    Text achievementText;
    readonly Queue<(string text, float seconds)> messages = new Queue<(string, float)>();
    Coroutine messageRoutine;

    static SaveData Data => SaveService.Data;
    int CurrentPen => ObjID.PenNumber;

    void Awake()
    {
        menuSet.SetActive(false);
        AchivementBox.SetActive(false);
        achievementText = AchivementBox.GetComponentInChildren<Text>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Menu();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 p = Pointer.World;
            if (p.y < 3)
            {
                if (p.x > 2) GoToPen(+1);
                else if (p.x < -2) GoToPen(-1);
            }
        }
    }

    // Moves to the nearest unlocked pen in the given direction.
    void GoToPen(int step)
    {
        for (int i = CurrentPen + step; i >= 0 && i < PenCatalog.SceneCount; i += step)
        {
            if (!Data.unlocked[i]) continue;
            SaveService.Save();
            SceneManager.LoadScene("Pen" + i);
            return;
        }
    }

    public void Menu() => menuSet.SetActive(!menuSet.activeSelf);

    public void GameExit()
    {
        SaveService.Save();
        Application.Quit();
    }

    public void DevelopScene()
    {
        SaveService.Save();
        SceneManager.LoadScene("DeveloperScene");
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveService.Save();
    }

    void OnApplicationQuit() => SaveService.Save();

    public void RecordKnock() => Record(Data.knocks, PenCatalog.Counter.Knock);
    public void RecordMiss() => Record(Data.misses, PenCatalog.Counter.Miss);

    void Record(int[] counts, PenCatalog.Counter counter)
    {
        int count = ++counts[CurrentPen];
        foreach (var r in PenCatalog.Rewards)
        {
            if (r.pen != CurrentPen || r.counter != counter || count < r.count || Data.unlocked[r.unlock]) continue;
            Unlock(r.unlock);
            Notify(r.message);
        }
    }

    public void Unlock(int pen)
    {
        Data.unlocked[pen] = true;
        SaveService.Save();
    }

    // Shows messages one after another in the achievement box. Duplicates already queued are dropped.
    public void Notify(string text, float seconds = 5f)
    {
        if (messages.Contains((text, seconds))) return;
        messages.Enqueue((text, seconds));
        messageRoutine ??= StartCoroutine(ShowMessages());
    }

    IEnumerator ShowMessages()
    {
        AchivementBox.SetActive(true);
        while (messages.Count > 0)
        {
            var (text, seconds) = messages.Peek();
            achievementText.text = text;
            yield return new WaitForSeconds(seconds);
            messages.Dequeue();
        }
        AchivementBox.SetActive(false);
        messageRoutine = null;
    }
}
