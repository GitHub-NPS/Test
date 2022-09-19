using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomJoystick : Joystick
{
    private Vector2 defaultPostion;

    private void Awake()
    {
        defaultPostion = background.anchoredPosition;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        background.anchoredPosition = defaultPostion;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}
