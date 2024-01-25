using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RainbowEffect
{
    public static Tweener Create(Image image, float period)
    {
        float timer = 0f;
        float shift = Random.value * 6f;
        float color;
        Tweener colorTween = DOTween.To(() => timer, x =>
        {
            timer = x;
            color = (shift + timer) % 6;
            if (timer <= 1f) image.color = new Color(timer, 0f, 1f);
            else if (timer <= 2f) image.color = new Color(1f, 0f, 1f - (timer - 1f));
            else if (timer <= 3f) image.color = new Color(1f, timer - 2f, 0f);
            else if (timer <= 4f) image.color = new Color(1f - (timer - 3f), 1f, 0f);
            else if (timer <= 5f) image.color = new Color(0f, 1f, timer - 4f);
            else if (timer <= 6f) image.color = new Color(0f, 1f - (timer - 5f), 1f);
        }, 6f, period).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        return colorTween;
    }

    public static Tweener Create(TMP_Text text, float period)
    {
        float timer = 0f;
        float shift = Random.value * 6f;
        float color;
        Tweener colorTween = DOTween.To(() => timer, x =>
        {
            timer = x;
            color = (shift + timer) % 6;
            if (timer <= 1f) text.color = new Color(timer, 0f, 1f);
            else if (timer <= 2f) text.color = new Color(1f, 0f, 1f - (timer - 1f));
            else if (timer <= 3f) text.color = new Color(1f, timer - 2f, 0f);
            else if (timer <= 4f) text.color = new Color(1f - (timer - 3f), 1f, 0f);
            else if (timer <= 5f) text.color = new Color(0f, 1f, timer - 4f);
            else if (timer <= 6f) text.color = new Color(0f, 1f - (timer - 5f), 1f);
        }, 6f, period).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        return colorTween;
    }
}
