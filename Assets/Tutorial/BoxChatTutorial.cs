using System;
using MEC;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxChatTutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtMessage = null;

    public void Set(string message)
    {
        txtMessage.text = message;
    }
}