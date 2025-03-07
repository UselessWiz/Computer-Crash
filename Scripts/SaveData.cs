using System.IO;

namespace pm_march_jamgame;

public class SaveData
{
    string SaveDataPath;

    public int GameBootCount;
    public int LastGameState;

    public SaveData(string saveDataPath = "GameData/SAVEDATA.SAV")
    {
        SaveDataPath = saveDataPath;
        if (File.Exists(saveDataPath)) { 
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(saveDataPath); 
  
            for (int i = 0; i < 2; i++) 
            {
                string[] split = lines[i].Split('=');
                if (i == 0) GameBootCount = int.Parse(split[1]);
                if (i == 1) LastGameState = int.Parse(split[1]);
            }
        } 
    }

    public void WriteSaveData()
    {        
        File.WriteAllText(SaveDataPath, ToString());
    }

    public new string ToString()
    {
        return $"GameBootCount={GameBootCount}\nLastGameState={LastGameState}";
    }
}