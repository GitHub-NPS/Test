using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public void Toggle()
    {
        this.gameObject.SetActive(gameObject.activeSelf);
    }
}
