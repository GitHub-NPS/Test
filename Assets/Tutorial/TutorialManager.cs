using Core;
using MEC;
using NPS;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager S = null;

    #region Properties
    [Header("UI")]
    [SerializeField] private GameObject blackLock;
    [SerializeField] private GameObject transparentLock;
    [SerializeField] private TextMeshProUGUI txt;

    [SerializeField] private HandTutorial handPrefab;

    [SerializeField] private GameObject objSkip;
    [SerializeField] private GameObject objTap2Continue;

    [SerializeField] private BoxChatTutorial boxChatPrefab;

    [Header("Properties")]
    private HandTutorial hand;
    private BoxChatTutorial boxChat;
    private CoroutineHandle handMove;

    private TutorialSave save;
    #endregion
   
    private void Awake()
    {
        if (S == null) S = this;

        save = DataManager.Save.Tutorial;
    }

    #region Handler 1

    public void StartTut(int tut)
    {
        //Debug.Log("Start: " + tut);
        save.CurTut = tut;
        save.CurStep = 0;

        IncreaseStep();
    }

    private void Complete(int tut)
    {
        if (!save.Complete.Contains(tut))
        {
            save.Complete.Add(tut);
            //Debug.Log("Complete: " + tut);
            this.PostEvent(EventID.CompleteTutorial, tut);

            save.Save();
        }
    }

    public void IncreaseStep()
    {
        save.CurStep++;
        //Debug.Log("Next Step: " + save.CurTut + " / " + save.CurStep);
        this.PostEvent(EventID.NextStepTutorial);
    }

    public void Skip()
    {
        HideHand();
        HideBoxChat();
    }

    public void Handler(int tut, List<int> steps, params object[] parameters)
    {
        if (save.Complete.Contains(tut)) return;

        if (save.CurTut == tut && steps.Contains(save.CurStep))
        {
            //Debug.Log("Handler: " + save.CurTut + " / " + save.CurStep);
            switch (save.CurTut)
            {
                case 1:
                    switch (save.CurStep)
                    {
                        case 1:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 2:
                            HideHand();
                            HideBoxChat();
                            Complete(tut);
                            break;
                    }
                    break;
                case 2:
                    switch (save.CurStep)
                    {
                        case 1:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 2:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 3:
                            HideHand();
                            HideBoxChat();
                            ShowLock(LockType.Transparent);
                            IncreaseStep();
                            break;
                        case 4:
                            ShowTap2Continue();
                            Complete(tut);
                            break;
                    }
                    break;
                case 3:
                    switch (save.CurStep)
                    {
                        case 1:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 2:
                            Complete(tut);
                            break;
                    }
                    break;
                case 4:
                    switch (save.CurStep)
                    {
                        case 1:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 2:
                            HideHand();
                            HideBoxChat();
                            IncreaseStep();
                            break;
                        case 3:
                            HideHand();
                            HideBoxChat();
                            Complete(tut);
                            break;
                    }
                    break;
            }
        }
    }
    #endregion

    #region Condition
    public bool ConditionStartTut1()
    {
        return true;
    }
    #endregion 

    #region Handler 2
    public void ShowLock(LockType type)
    {
        blackLock.SetActive(type == LockType.Black);
        transparentLock.SetActive(type == LockType.Transparent);
    }

    public void ShowHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent, params GameObject[] objs)
    {
        CreateHand(isUI, handT, parent.transform);
        ShowLock(lockT);
        ShowSkip(isSkip);
        if (objs.Length == 0) RayCast(parent);
        else
        {
            for (int i = 0; i < objs.Length; i++)
            {
                RayCast(objs[i]);
            }
        }
    }

    public void ShowHand(bool isUI, HandType handT, LockType lockT, bool isSkip, GameObject parent, Vector3 posStart, Vector3 posEnd, params GameObject[] objs)
    {
        CreateHand(isUI, handT, parent.transform);
        ShowLock(lockT);
        ShowSkip(isSkip);
        if (objs.Length == 0) RayCast(parent);
        else
        {
            for (int i = 0; i < objs.Length; i++)
            {
                RayCast(objs[i]);
            }
        }

        if (handT == HandType.Move && hand)
        {
            handMove = Timing.RunCoroutine(hand._Move(posEnd, posStart, true));
        }
    }

    private void ShowTap2Continue()
    {
        objTap2Continue.SetActive(true);
    }

    public void Tap2Continue()
    {
        objTap2Continue.SetActive(false);
        HideHand();
        HideBoxChat();

        this.PostEvent(EventID.Tap2ContinueTutorial);
    }

    public void ShowBoxChat(string message, Transform parent)
    {
        HideBoxChat();

        boxChat = Instantiate(boxChatPrefab, parent);

        boxChat.Set(message);
    }

    public void ShowText(string content)
    {
        txt.text = content;
        txt.gameObject.SetActive(true);
    }

    private void ShowSkip(bool isShow)
    {
        objSkip.SetActive(isShow);
    }

    public void HideText()
    {
        txt.gameObject.SetActive(false);
    }

    private void HideBoxChat()
    {
        if (boxChat) Destroy(boxChat.gameObject);
    }

    private void HideHand(bool isClear = true)
    {
        if (handMove.IsValid) Timing.KillCoroutines(handMove); ;

        if (hand) Destroy(hand.gameObject);

        if (isClear) Clear();
    }
    #endregion

    #region Handler 3
    List<GraphicRaycaster> lstRc = new List<GraphicRaycaster>();
    List<Tuple<Canvas, bool, int>> lstCv = new List<Tuple<Canvas, bool, int>>();
    List<Tuple<Canvas, bool, int>> lstOldCv = new List<Tuple<Canvas, bool, int>>();
    List<GameObjectLayer> lstOldLayer = new List<GameObjectLayer>();

    public void RayCast(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("[Tutorial]: Raycast null");
            return;
        }

        int layer = obj.layer;
        if (layer == 5)
        {
            Canvas cv = obj.GetComponent<Canvas>();
            if (cv == null)
            {
                cv = obj.AddComponent<Canvas>();
                lstCv.Add(new Tuple<Canvas, bool, int>(cv, cv.overrideSorting, cv.sortingOrder));
            }
            else
            {
                lstOldCv.Add(new Tuple<Canvas, bool, int>(cv, cv.overrideSorting, cv.sortingOrder));
            }

            cv.overrideSorting = true;
            cv.sortingOrder = 201;

            GraphicRaycaster rc = obj.GetComponent<GraphicRaycaster>();
            if (rc == null)
            {
                rc = obj.AddComponent<GraphicRaycaster>();
                lstRc.Add(rc);
            }
        }
        else
        {
            ChangeLayer(obj, true);
        }
    }

    public void ChangeLayer(GameObject obj, bool isSave = false)
    {
        iChangeLayer(obj, LayerMask.NameToLayer("UI"), isSave);

        SortingGroup sg = obj.GetComponent<SortingGroup>();
        if (sg) sg.sortingOrder = 201;
    }

    private void iChangeLayer(GameObject obj, int layer, bool isSave = false)
    {
        if (isSave)
        {
            lstOldLayer.Add(new GameObjectLayer()
            {
                obj = obj,
                layer = obj.layer
            });
        }

        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            iChangeLayer(child.gameObject, layer, isSave);
        }
    }

    private void Clear()
    {
        foreach (var item in lstRc)
        {
            if (item == null) continue;
            Destroy(item);
        }
        foreach (var item in lstCv)
        {
            if (item == null || item.Item1 == null) continue;
            item.Item1.overrideSorting = item.Item2;
            item.Item1.sortingOrder = item.Item3;

            Destroy(item.Item1);
        }
        foreach (var item in lstOldCv)
        {
            if (item == null || item.Item1 == null) continue;

            item.Item1.overrideSorting = item.Item2;
            item.Item1.sortingOrder = item.Item3;

            if (item.Item1.overrideSorting != item.Item2)
            {
                Debug.LogWarning("[Tutorial]: Don't change override sorting");
            }
        }
        foreach (var item in lstOldLayer)
        {
            if (item == null || item.obj == null) continue;
            item.obj.layer = item.layer;
        }

        ShowLock(LockType.None);
        ShowSkip(false);

        lstRc.Clear();
        lstCv.Clear();
        lstOldCv.Clear();
    }

    private class GameObjectLayer
    {
        public GameObject obj;
        public int layer;
    }

    private void CreateHand(bool isUI, HandType type, Transform parent, bool isClear = true)
    {
        HideHand(isClear);

        hand = Instantiate(handPrefab, parent);

        hand.Set(isUI, type);
    }
    #endregion
}
