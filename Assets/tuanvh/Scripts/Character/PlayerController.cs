using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f;

    public Action<int> OnAttacking;
    public Action<int> OnDodging;
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

#if UNITY_EDITOR
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endTouchPosition = Input.mousePosition;
                DetectGesture();
            }
        }
        else
        {
            // Code gốc cho mobile
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startTouchPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        endTouchPosition = touch.position;
                        DetectGesture();
                        break;
                }
            }
        }
    }
#endif

    void DetectGesture()
    {
        Vector2 delta = endTouchPosition - startTouchPosition;

        if (delta.magnitude < minSwipeDistance)
        {
            // Tap
            Debug.Log("Tap detected");
            CurrentState = PlayerState.Attacking;
            OnAttacking?.Invoke(0);
            return;
        }

        float angle = Vector2.Angle(Vector2.right, delta);
        float vertical = delta.y;
        float horizontal = delta.x;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
            {
                Debug.Log("Swipe Right → Attack");
                CurrentState = PlayerState.Attacking;
                OnAttacking?.Invoke(2);
            }
            else
            {
                Debug.Log("Swipe Left → Attack");
                CurrentState = PlayerState.Attacking;
                OnAttacking?.Invoke(3);
            }
        }
        else
        {
            if (vertical > 0)
            {
                Debug.Log("Swipe Up ↑ Attack");
                CurrentState = PlayerState.Attacking;
                OnAttacking?.Invoke(1);
            }
            else
            {
                Debug.Log("Swipe Down ↓ Defend");
                CurrentState = PlayerState.Dodging;
                OnDodging?.Invoke(0);
            }
        }
    }
}
public enum PlayerState
{
    Idle,
    Attacking,
    Defending,
    Dodging
}
