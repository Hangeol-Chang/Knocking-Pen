using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public bool[] unlocked = new bool[PenCatalog.SlotCount];
    public int[] knocks = new int[PenCatalog.SlotCount];
    public int[] misses = new int[PenCatalog.SlotCount];
    public bool tutorialDone;
}

// One save shared by every scene, loaded on first access.
public static class SaveService
{
    const string Key = "save";
    static SaveData data;

    public static SaveData Data => data ??= Load();

    public static void Save()
    {
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    static SaveData Load()
    {
        SaveData d;
        try
        {
            d = PlayerPrefs.HasKey(Key) ? JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(Key)) : MigrateLegacy();
        }
        catch (ArgumentException e)
        {
            Debug.LogError("Save is corrupted, starting fresh: " + e.Message);
            d = new SaveData();
        }

        Array.Resize(ref d.unlocked, PenCatalog.SlotCount);
        Array.Resize(ref d.knocks, PenCatalog.SlotCount);
        Array.Resize(ref d.misses, PenCatalog.SlotCount);
        d.unlocked[0] = true;
        return d;
    }

    // Pre-refactor format: "unlockedpen" = "1,0,1,...", float knock counters for pen 0 and 1.
    static SaveData MigrateLegacy()
    {
        var d = new SaveData();
        if (!PlayerPrefs.HasKey("monamiknock")) return d;

        string[] flags = PlayerPrefs.GetString("unlockedpen").Split(',');
        for (int i = 0; i < flags.Length && i < d.unlocked.Length; i++)
            d.unlocked[i] = flags[i] == "1";

        d.knocks[0] = (int)PlayerPrefs.GetFloat("monamiknock");
        d.knocks[1] = (int)PlayerPrefs.GetFloat("jetstreamknock");
        d.tutorialDone = true;
        return d;
    }
}
