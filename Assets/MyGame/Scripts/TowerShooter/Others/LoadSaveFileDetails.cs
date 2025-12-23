using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadSaveFileDetails : MonoBehaviour
{
    public TMP_Text[] saveFileNames;
    public TMP_Text[] waveNumbers,
                      towerHealth,
                      playerScore;

    public void Awake()
    {
        LoadTheDetails();
    }

    public void LoadTheDetails()
    {
        string[] folders = Directory.GetDirectories(Application.persistentDataPath).Select(Path.GetFileName).ToArray();

        foreach(string s in folders)
        {
            Debug.Log(s);
            string[] saveName = Directory.GetDirectories(Application.persistentDataPath + "/" + s).Select(Path.GetFileName).ToArray();
            int index = int.Parse(s);
            saveFileNames[index].text = saveName[0];
            LoadTheSaveFileDetails(Application.persistentDataPath + "/" + s + "/" + saveName[0], index);
        }
    }

    public void LoadTheSaveFileDetails(string path,int index)
    {
        if(Directory.Exists(path))
        {
            PlayerData data = PlayerDataSaver.LoadPlayerDataFromFile(path + "/playerData1.dat");
            if(data!=null)
            {
                waveNumbers[index].text = data.waveNumber.ToString();
                towerHealth[index].text = data.towerHealth.ToString();
                playerScore[index].text = data.playerScore.ToString();
            }
        }
    }
}
