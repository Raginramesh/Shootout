using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{

    public enum TouchStatus { None, Tap, Hold, Swipe };
    public TouchStatus currentTouchStatus;
    Touch playerTouch;

    Vector2 beginTouchPosition;
    Vector2 endTouchPosition;

    public float distance;
    public Vector2 direction;

    public float touchDeadZone;
    public float tapToHoldTimeLimit;


    void Start()
    {
        currentTouchStatus = TouchStatus.None;
    }

    
    void Update()
    {
        GetTouchInput();
    }



    public void GetTouchInput()
    {
        playerTouch = Input.GetTouch(0);

        if (playerTouch.tapCount > 0)
        {

            if (playerTouch.phase == TouchPhase.Began && currentTouchStatus != TouchStatus.Swipe)
            {
                currentTouchStatus = TouchStatus.Tap;
                Invoke("ChangeToHold", tapToHoldTimeLimit);
                beginTouchPosition = playerTouch.position;
            }
            else if (playerTouch.phase == TouchPhase.Moved && distance > touchDeadZone)
            {
                currentTouchStatus = TouchStatus.Swipe;
            }
            endTouchPosition = playerTouch.position;

            CalculateDistanceAndDirection(beginTouchPosition, endTouchPosition);

            if (playerTouch.phase == TouchPhase.Ended)
            {
                CancelInvoke("ChangeToHold");

                ResetAllData();
            }
        }
    }

    void ChangeToHold()
    {
        currentTouchStatus = TouchStatus.Hold;
    }

    private void CalculateDistanceAndDirection(Vector2 startpos, Vector2 endPos)
    {
        Vector2 heading = startpos - endPos;
        distance = heading.magnitude;

        if(distance > 0)
            direction = heading / distance;
    }

    private void ResetAllData()
    {
        distance = 0f;
        direction = new Vector2();
        playerTouch = new Touch();
        currentTouchStatus = TouchStatus.None;
    }
}
