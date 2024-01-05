using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IdleTimeNotification : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Setup(float time, float duration, float distance)
    {
        timeText.text = "+" + time;

        Vector3 currentPosition = rectTransform.anchoredPosition;
        Animations.Instance.PlayAnimation(this, newValue => rectTransform.anchoredPosition = newValue,  duration, currentPosition, currentPosition+new Vector3(0,distance,0), Easing.FindEaseType(Easing.EaseType.OutCubic), Animations.LoopType.Single, Destroy);
        Animations.Instance.PlayAnimation(this, newValue => canvasGroup.alpha = newValue,  duration, canvasGroup.alpha, 0.0f, Easing.FindEaseType(Easing.EaseType.OutQuad), Animations.LoopType.Single);
    }

    private void Destroy()
    {
       Destroy(gameObject); 
    }
}
