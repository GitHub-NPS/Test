using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
	public static MenuGameManager S;
	
	private void Awake()
    {
        if (!S) S = this;
    }
	
    private void Start()
    {
        AppManager.Ads?.HideBanner();
    }
}
