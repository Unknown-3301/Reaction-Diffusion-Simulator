using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FSP_Toggle : MonoBehaviour
{
    [SerializeField] private RectTransform toggleTransform;
    [SerializeField] private RectTransform bodyTransform;
    [SerializeField] private RectTransform showRegionTransform;
    [SerializeField] private Image image;
    [SerializeField] private Image arrowImage;

    private bool on;

    private void Update()
    {
        if (on)
            return;

        Rect showRect = GameManager.RectTransformToScreenSpace(showRegionTransform);
        bool mouseInside = showRect.Contains(Input.mousePosition);

        image.enabled = mouseInside;
        arrowImage.enabled = mouseInside;
    }

    public void OnClick()
    {
        float offset = (on ? -1 : 1) * 285;
        float dirOffset = (on ? 1 : 0) * 180;

        Quaternion q = toggleTransform.localRotation;
        q.eulerAngles = new Vector3(0, 0, dirOffset);
        toggleTransform.localRotation = q;

        Vector3 pos = bodyTransform.localPosition; 
        pos.y += offset;
        bodyTransform.localPosition = pos;

        on = !on;
    }

    public bool IsContainingMouse(Vector2 pos)
    {
        Rect rectBody = GameManager.RectTransformToScreenSpace(bodyTransform);
        Rect rectToggle = GameManager.RectTransformToScreenSpace(toggleTransform);

        return rectBody.Contains(pos) || rectToggle.Contains(pos);
    }
}
