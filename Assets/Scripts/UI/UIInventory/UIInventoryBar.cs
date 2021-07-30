using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool _isInventoryBarPositionBottom;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        SwitchInventoryBarPosition();
    }

    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition(); 
        if(playerViewportPosition.y > 0.3f && !_isInventoryBarPositionBottom)
        {
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0, 2.5f);

            _isInventoryBarPositionBottom = true;
        }
        else if(playerViewportPosition.y < 0.3f && _isInventoryBarPositionBottom)
        {
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -2.5f);

            _isInventoryBarPositionBottom = false;
        }
    }
}
