using System;
using UnityEngine;

// Since all data in this script is saved across scenes, almost all 
// variables are privitised and the getter/setter functions must be used.

public enum Character
{
    Anglerfish,
    Dolphin,
    Swordfish,
    Turtle
}

public enum Volume
{
    Master,
    BG,
    SFX
}

public class DataCarrier : MonoBehaviour
{
    public static DataCarrier Instance;

    [HideInInspector] public string nextRaceTag;
    [HideInInspector] public Character currentCharacter = Character.Anglerfish;

    private int currentSaveZoneID = 0;
    private int overworldProgress = 0;
    private int biomeNum = 1;
    private int discoveryID = 0;
    private int cutsceneID = 0;
    private int tutorialID = 0;
    private float masterVolume = 1f;
    private float bgVolume = 0.35f;
    private float sfxVolume = 0.75f;
    private float[] bestTimes = new float[10];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTag(string tag)
    {
        nextRaceTag = tag;
    }

    public void SetCharacter(Character character)
    {
        currentCharacter = character;
    }

    public Character GetCharacter()
    {
        return currentCharacter;
    }

    public void SetSaveZone(int id)
    {
        currentSaveZoneID = id;
    }

    public int GetSaveZone()
    {
        return currentSaveZoneID;
    }

    public void UnlockProgress(int id)
    {
        if (id > overworldProgress)
        {
            overworldProgress = id;
        }
    }

    public int GetProgress() 
    {
        return overworldProgress;
    }

    public void SetBiomeNum(int id)
    {
        biomeNum = id;
    }

    public int GetBiomeNum()
    {
        return biomeNum;
    }

    public void SetDiscoveryID(int id)
    {
        discoveryID = id;
    }

    public int GetDiscoveryID()
    {
        return discoveryID;
    }

    public void SetCutsceneID(int id)
    {
        cutsceneID = id;
    }

    public int GetCutsceneID()
    {
        return cutsceneID;
    }

    public void SetTutorialID(int id)
    {
        tutorialID = id;
    }

    public int GetTutorialID()
    {
        return tutorialID;
    }

    public void SetBestTime(int level, float time)
    {
        float targetTime = bestTimes[level-1];
        float roundedTime = (float)Math.Round(time, 1);
        if (targetTime == 0.0f || roundedTime < targetTime)
        {
            bestTimes[level-1] = roundedTime;
        }
    }

    public float GetBestTime(int level)
    {
        return bestTimes[level-1];
    }

    public void SetVolume(Volume volType, float newVol)
    {
        if (volType == Volume.Master)
        {
            masterVolume = newVol;
        }
        else if (volType == Volume.BG)
        {
            bgVolume = newVol;
        }
        else
        {
            sfxVolume = newVol;
        }
    }

    public float GetVolume(Volume volType)
    {
        if (volType == Volume.Master)
        {
            return masterVolume;
        }
        else if (volType == Volume.BG)
        {
            return bgVolume;
        }
        else
        {
            return sfxVolume;
        }
    }
}