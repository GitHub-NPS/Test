using System;
using BayatGames.SaveGameFree;
using NPS;

[System.Serializable]
public class GeneralSave: IDataSave
{
    private string key = "General";

    public void Fix()
    {

    }

    public void Save()
    {
        SaveGame.Save(key, this);
    }

    public DateTime LastTimeOut = DateTime.UtcNow;
	
    public TimeSpan TimeOut
    {
        get
        {
            if ((UnbiasedTime.UtcNow - LastTimeOut).TotalMinutes <= 0)
            {
                return new TimeSpan(0, 0, 0, 0);
            }
            return UnbiasedTime.UtcNow - LastTimeOut;
        }
    }

    public float Sound = 1.0f;
    public float Music = 1.0f;

    public bool Ads = true;

    public int CountInterAds = 0;
    public int CountRewardAds = 0;

    public void SetLastTimeOut()
    {
        LastTimeOut = UnbiasedTime.S ? UnbiasedTime.UtcNow : DateTime.Now;
    }

    public void SetSound(float value)
    {
        Sound = value;
    }

    public void SetMusic(float value)
    {
        Music = value;
    }

    public void SetAds(bool value)
    {
        Ads = value;

        Observer.S?.PostEvent(EventID.ChangeAds, value);
    }
}
