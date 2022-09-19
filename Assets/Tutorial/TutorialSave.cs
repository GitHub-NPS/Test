using BayatGames.SaveGameFree;
using System.Collections.Generic;

[System.Serializable]
public class TutorialSave: IDataSave
{
    private string key = "Tutorial";

    public int CurTut = 0;
    public int CurStep = 0;

    public List<int> Complete = new List<int>();

    public void Fix()
    {
        
    }

    public void Save()
    {
        SaveGame.Save(key, this);
    }
}
