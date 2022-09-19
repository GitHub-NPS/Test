using NPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFixMenu : MonoBehaviour
{
    [SerializeField] private GameObject content;

    [SerializeField] private List<UITabItem> tabItems = new List<UITabItem>();
    [SerializeField] private UIView viewMain;
    [SerializeField] private UIView viewShop;
    [SerializeField] private UIView viewHero;
    [SerializeField] private UIView viewArena;
    [SerializeField] private UIView viewEvent;
    private TabViewStatus status = TabViewStatus.None;
    private Dictionary<TabViewStatus, UIView> dicView = new Dictionary<TabViewStatus, UIView>();

    private void Awake()
    {
        for (int i = 0; i < tabItems.Count; i++)
        {
            tabItems[i].Set(this);
        }
    }

    private void Start()
    {
        ChangeStatus(TabViewStatus.Battle);
    }

    public void ChangeStatus(TabViewStatus status)
    {
        if (status == TabViewStatus.None)
        {
            MenuGameScene.S.Toast.Show(I2.Loc.LocalizationManager.GetTranslation("Coming soon"));
            return;
        }

        if (this.status == status) return;
        this.status = status;

        if (!dicView.ContainsKey(this.status))
        {
            dicView.Add(status, null);
        }

        switch (this.status)
        {
            case TabViewStatus.Battle:
                dicView[status] = viewMain;
                viewMain.Show();

                CloseView(status);
                break;
            case TabViewStatus.Hero:
                dicView[status] = viewHero;
                viewHero.Show();

                CloseView(status);
                break;
            case TabViewStatus.Shop:
                dicView[status] = viewShop;
                viewShop.Show();

                CloseView(status);
                break;
            case TabViewStatus.Arena:
                dicView[status] = viewArena;
                viewArena.Show();

                CloseView(status);
                break;
            case TabViewStatus.Event:
                dicView[status] = viewEvent;
                viewEvent.Show();

                CloseView(status);
                break;
            default:
                break;
        }

        for (int i = 0; i < tabItems.Count; i++)
        {
            tabItems[i].Active(this.status);
        }
    }

    private void CloseView(TabViewStatus self)
    {
        foreach (KeyValuePair<TabViewStatus, UIView> item in dicView)
        {
            if (item.Value != null && item.Key != self) item.Value.Hide();
        }
    }
}
