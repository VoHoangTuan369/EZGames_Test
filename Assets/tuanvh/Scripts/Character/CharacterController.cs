using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance = 50f;

    public Action<int> OnAttacking;
    public Action OnDodging;

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
                OnAttacking?.Invoke(3);
            }
            else
            {
                Debug.Log("Swipe Left → Attack");
                OnAttacking?.Invoke(2);
            }
        }
        else
        {
            if (vertical > 0)
            {
                Debug.Log("Swipe Up ↑ Attack");
                OnAttacking?.Invoke(1);
            }
            else
            {
                Debug.Log("Swipe Down ↓ Defend");
                OnDodging?.Invoke();
            }
        }
    }
}
