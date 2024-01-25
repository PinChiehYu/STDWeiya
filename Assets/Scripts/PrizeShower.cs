using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeShower : MonoBehaviour
{
    public Image background;
    public Image photo;
    public TMP_Text nameText;
    public TMP_Text prizeText;
    public Button prizeBtn;

    [Header("Kenneth")]
    public RectTransform kennethFigureRect;
    public RectTransform dialogBackgroundRect;
    public Image dialogBackground;
    public TMP_Text dialogText;

    private bool showBonusAnimation;
    private bool showRainbowEffectOnly;

    private string bonusText;
    private List<Color> teamColors;

    private Tweener colorTween;

    void Start()
    {
        teamColors = new()
        {
            new Color(247f / 255, 168f / 255, 185f / 255),
            new Color( 17f / 255,  57f / 255, 252f / 255),
            new Color(252f / 255,  74f / 255,  31f / 255),
            new Color( 74f / 255, 150f / 255, 217f / 255),
            new Color(255f / 255, 213f / 255,   0f / 255),
        };

        prizeBtn.onClick.AddListener(() => StartBonusAnimation());
        showBonusAnimation = false;

        RainbowEffect.Create(transform.Find("Congrat").gameObject.GetComponent<Image>(), 0.5f);

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void UpdateInfo(Member member, string prize)
    {
        photo.sprite = Resources.Load<Sprite>("Textures/Photos/" + member.account);
        background.color = teamColors[member.teamID];
        nameText.text = member.name;
        prizeText.text = prize;

        showBonusAnimation = false;
        showRainbowEffectOnly = false;
    }

    public void UpdateInfoWithBonus(Member member, string prize, string bonus, bool hasShown)
    {
        UpdateInfo(member, prize);

        if (hasShown)
        {
            prizeText.text = bonus;
            showBonusAnimation = false;
            showRainbowEffectOnly = true;
        }
        else
        {
            bonusText = bonus;
            showBonusAnimation = true;
            showRainbowEffectOnly = false;
        }
    }

    public void PopUp()
    {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);

        if (showRainbowEffectOnly)
        {
            SetActivateRainbowEffect();
        }
    }

    public void Close()
    {
        transform.DOScale(0, 0.3f).OnComplete(() => {
            SetActivateRainbowEffect(false);
            prizeBtn.image.color = Color.white;
            kennethFigureRect.gameObject.SetActive(false);
            dialogText.text = "";
            gameObject.SetActive(false);
        }).SetEase(Ease.InBack);
    }

    public bool IsActivate() { return gameObject.activeSelf; }

    private void StartBonusAnimation()
    {
        if (!showBonusAnimation) return;

        // Vector2(805, -325);
        // Reset
        kennethFigureRect.gameObject.SetActive(true);
        kennethFigureRect.anchoredPosition = new Vector2(805, -755);
        dialogBackgroundRect.anchoredPosition = new Vector2(-225, 180);
        dialogBackground.color = new Color(1, 1, 1, 0);
        dialogText.color = background.color;

        var seq = DOTween.Sequence();
        string txt = "";

        // Kenneth
        seq.Append(kennethFigureRect.DOAnchorPosY(-325, 1.0f).SetEase(Ease.InOutBack));
        seq.AppendInterval(0.5f);
        seq.Append(dialogBackground.GetComponent<RectTransform>().DOAnchorPosY(230, 0.5f));
        seq.Join(dialogBackground.DOColor(Color.white, 0.5f));
        seq.Append(DOTween.To(() => txt, x => txt = x, "¡P¡P¡P", 1f).OnUpdate(() => dialogText.text = txt));

        // Check
        seq.Append(prizeBtn.transform.DORotate(new Vector3(720.0f, 0.0f), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        seq.Join(prizeBtn.transform.DOScale(0, 0.5f).OnComplete(() => { prizeText.text = bonusText; SetActivateRainbowEffect(); }));
        seq.Join(prizeBtn.transform.DOScale(1, 0.5f).SetDelay(0.5f).OnComplete(() => txt = ""));
        seq.Append(DOTween.To(() => txt, x => txt = x, "NICE", 0.2f).SetDelay(0.5f).OnUpdate(() => dialogText.text = txt));

        showBonusAnimation = false;
    }

    private void SetActivateRainbowEffect(bool activate = true)
    {
        if (colorTween != null && colorTween.IsPlaying())
        {
            colorTween.Kill();
            colorTween = null;
        }

        if (activate)
        {
            colorTween = RainbowEffect.Create(prizeBtn.image, 1f);
        }
    }
}
