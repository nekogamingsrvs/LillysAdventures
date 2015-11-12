using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using VoidInc;

[System.Serializable]
public class ConfigFileSetting
{
    public string settingName;
    public int settingValue;
    public bool useGMScore = false;
}

public class ConfigFileManager : MonoBehaviour
{
    public List<ConfigFileSetting> settingsToSet = new List<ConfigFileSetting>();

    void Start()
    {
        for (int a = 0; a < settingsToSet.Count; a++)
        {
            if (settingsToSet[a].useGMScore == true)
            {
                gameObject.GetComponent<GameManager>().Score = PlayerPrefs.GetInt(settingsToSet[a].settingName, 0);
            }
            else
            {
                settingsToSet[a].settingValue = PlayerPrefs.GetInt(settingsToSet[a].settingName, 0);
            }
        }
    }

    public void Update()
    {
        for (int a = 0; a < settingsToSet.Count; a++)
        {
            if (settingsToSet[a].useGMScore == true)
            {
                settingsToSet[a].settingValue = gameObject.GetComponent<GameManager>().Score;
            }
            if (PlayerPrefs.GetInt(settingsToSet[a].settingName, 0) == settingsToSet[a].settingValue)
            {

            }
            else
            {
                PlayerPrefs.SetInt(settingsToSet[a].settingName, settingsToSet[a].settingValue);
            }
            if (a >= settingsToSet.Count)
            {
                a = 0;
            }
        }
    }
}