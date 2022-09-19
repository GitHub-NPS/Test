using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameScene : MonoBehaviour
{
    public static MenuGameScene S = null;

    [Header("UI")]
    public UIMainMenu Main;
    public UIToast Toast;
    public UIFixMenu Fix;

    private void Awake()
    {
        if (!S) S = this;
    }

    private void Start()
    {
        
    }
}
