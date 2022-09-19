using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirm : MonoBehaviour
{
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    [SerializeField] private TextMeshProUGUI txtTittle;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private GameObject objMask;
    [SerializeField] private GameObject content;
    private Action callBackYes;
    private Action callBackNo;

    [SerializeField] private Image imgBtnYes;
    [SerializeField] private Image imgBtnNo;
    [SerializeField] private Sprite spRed;
    [SerializeField] private Sprite spGreen;

    private ConfirmType type = ConfirmType.YesNo;

    public void OnShow(ConfirmType type, string tittle, string description, bool mask, Action _callBackYes = null, Action _callBackNo = null)
    {
        callBackYes = _callBackYes;
        callBackNo = _callBackNo;

        txtTittle.text = I2.Loc.LocalizationManager.GetTranslation(tittle);
        txtDescription.text = I2.Loc.LocalizationManager.GetTranslation(description);
        
        objMask.SetActive(mask);

        if (this.type != type)
        {
            this.type = type;
            switch (type)
            {
                case ConfirmType.YesNo:
                    imgBtnYes.sprite = spRed;
                    imgBtnNo.sprite = spGreen;
                    break;
                case ConfirmType.NoYes:
                    imgBtnYes.sprite = spGreen;
                    imgBtnNo.sprite = spRed;
                    break;
            }
        }

        content.SetActive(true);
    }

    public void OnClickYes()
    {
        Hide();

        if (callBackYes != null)
        {
            callBackYes();

            callBackYes = null;
        }
    }

    public void OnClickNo()
    {
        Hide();
        
        if (callBackNo != null)
        {
            callBackNo();

            callBackNo = null;
        }
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}
