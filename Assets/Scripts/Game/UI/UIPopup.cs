using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    private GeneralSave general;

    private void OnEnable()
    {
        general = DataManager.Save.General;

        if (general.Ads && AppManager.Ads.IsShowBanner)
        {
            AppManager.Ads.HideBanner();
        }
    }

    private void OnDisable()
    {
        if (general.Ads && !AppManager.Ads.IsShowBanner)
        {
            AppManager.Ads.ShowBanner();
        }
    }
}
