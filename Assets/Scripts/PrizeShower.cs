using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeShower : MonoBehaviour
{
    private Image photo;
    private TMP_Text nameText;
    private TMP_Text prizeText;

    private Button bonusBtn;
    private TMP_Text bonusText;

    private bool showBonus;

    void Start()
    {
        photo = transform.Find("Photo").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        prizeText = transform.Find("PrizeText").GetComponent<TMP_Text>();
        bonusBtn = transform.Find("BonusBtn").GetComponent<Button>();
        bonusText = bonusBtn.transform.Find("BonusText").GetComponent<TMP_Text>();
        bonusBtn.onClick.AddListener(() => StartBonusProcess());

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
        UpdateInfo(member, bonus);
        bonusText.text = prize;

        showBonus = true;
    }

    public void PopUp()
    {
        if (showBonus)
        {
            bonusBtn.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void Close()
    {
        transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => { gameObject.SetActive(false); bonusBtn.gameObject.SetActive(false); });
    }
    public bool IsActivate() { return gameObject.activeSelf; } 

    private void StartBonusProcess()
    {
        if (!showBonus) return;

        Debug.Log("ShowBonus!!!!");
    }
}
