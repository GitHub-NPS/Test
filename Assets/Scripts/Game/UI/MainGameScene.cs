using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
    public static MainGameScene S = null;

    [Header("UI")]
    public UIMainGame Main;
    public UIToast Toast;
    public UIPause Pause;
    public UIConfirm Confirm;

    private void Awake()
    {
        if (!S) S = this;
    }

    private void Start()
    {
        
    }
}
