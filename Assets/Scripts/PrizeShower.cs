using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeShower : MonoBehaviour
{
    private Image photo;
    private TMP_Text nameText;
    private TMP_Text prizeText;
    private Button prizeBtn;

    private bool showBonus;
    private string bonusText;

    private Tweener colorTween;

    void Start()
    {
        photo = transform.Find("Photo").GetComponent<Image>();
        prizeBtn = transform.Find("PrizeBtn").GetComponent<Button>();
        prizeText = prizeBtn.transform.Find("PrizeText").GetComponent<TMP_Text>();
        nameText = transform.Find("NameText").GetComponent<TMP_Text>();

        prizeBtn.onClick.AddListener(() => StartBonusProcess());
        showBonus = false;

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void UpdateInfo(Member member, string prize)
    {
        photo.sprite = Resources.Load<Sprite>("Textures/Photos/" + member.account);
        nameText.text = member.name;
        prizeText.text = prize;

        showBonus = false;
    }

    public void UpdateInfoWithBonus(Member member, string prize, string bonus)
    {
        UpdateInfo(member, prize);

        showBonus = true;
        bonusText = bonus;
    }

    public void PopUp()
    {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void Close()
    {
        transform.DOScale(0, 0.3f).OnComplete(() => { 
            gameObject.SetActive(false);
            prizeBtn.image.color = Color.white;
            colorTween.Kill(); 
        }).SetEase(Ease.InBack);
    }
    public bool IsActivate() { return gameObject.activeSelf; } 

    private void StartBonusProcess()
    {
        if (!showBonus) return;

        float timer = 0f;
        var seq = DOTween.Sequence();
        seq.Append(prizeBtn.transform.DOScale(0, 0.5f).OnComplete(() => prizeText.text = bonusText));
        seq.Append(prizeBtn.transform.DOScale(1, 0.5f));
        seq.Insert(0f, prizeBtn.transform.DORotate(new Vector3(360.0f, 0.0f), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear));

        colorTween = DOTween.To(() => timer, x =>
        {
            timer = x;
            if (timer <= 1f) prizeBtn.image.color = new Color(timer, 0f, 1f);
            else if (timer <= 2f) prizeBtn.image.color = new Color(1f, 0f, 1f - (timer - 1f));
            else if (timer <= 3f) prizeBtn.image.color = new Color(1f, timer - 2f, 0f);
            else if (timer <= 4f) prizeBtn.image.color = new Color(1f - (timer - 3f), 1f, 0f);
            else if (timer <= 5f) prizeBtn.image.color = new Color(0f, 1f, timer - 4f);
            else if (timer <= 6f) prizeBtn.image.color = new Color(0f, 1f - (timer - 5f), 1f);
        }, 6, 1f).SetDelay(0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
