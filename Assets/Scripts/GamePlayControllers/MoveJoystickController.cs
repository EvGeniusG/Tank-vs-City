using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystickController : FixedJoystick
{

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        GamePlayController.Instance.SetMove(Direction);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        GamePlayController.Instance.SetMove(Direction);
    }

    

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        GamePlayController.Instance.SetMove(Vector2.zero);
    }
}
