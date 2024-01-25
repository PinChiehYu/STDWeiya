using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Raffle : MonoBehaviour
{
    private Button startBtn;
    private List<Button> prizeBtnLists = new();
    private List<TMP_Text> prizeTags = new();
    private Button prizeBtnThx;

    private DataRecord record;
    private List<Member> prizeOwners;
    private List<Member> prizeRemainMembers;
    private List<string> prizeNames;
    private List<string> bonusNames;
    private List<bool> bonusHasShown;

    private Image specialThanksBg;
    private TMP_Text specialThanksText;

    public PrizeShower prizeShower;

    private const int prizeSize = 20;
    private int stopSeq = 0;
    private bool isShowingSpecialThanks = false;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();
        prizeOwners = record.GetPrizeOwners();
        prizeRemainMembers = record.GetPrizeRemainMembers();

        prizeNames = new() { "10,000", "9,000", "8,000", "7,000", "6,000", "4,000", "3,600", "2,500" };
        bonusNames = new() { "20,000",      "",      "",      "",      "", "5,000", "4,000", "3,000" };
        bonusHasShown = new() { false,   false,   false,   false,   false,   false,   false,   false };

        startBtn = transform.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(() => StartRaffleProcess());

        Transform prizeListObj = transform.Find("PrizeList");
        for (int i = 0; i < prizeOwners.Count; i++)
        {
            Transform prizei = prizeListObj.Find("Prize" + i.ToString());
            Button prizeBtn = prizei.gameObject.GetComponent<Button>();

            prizeBtnLists.Add(prizeBtn);
            prizeTags.Add(prizei.Find("PrizeText").gameObject.GetComponent<TMP_Text>());

            int prizeID = i;
            prizeBtn.onClick.AddListener(() => ShowPrizeDetail(prizeID));
            prizeBtn.enabled = false;
        }

        Transform labelListObj = transform.Find("LabelList");
        for (int i = 0; i < prizeNames.Count; i++)
        {
            TMP_Text text = labelListObj.Find("Label" + i.ToString()).gameObject.GetComponent<TMP_Text>();
            RainbowEffect.Create(text, i + 1f);
        }

        prizeBtnThx = prizeListObj.Find("PrizeThx").gameObject.GetComponent<Button>();
        prizeBtnThx.onClick.AddListener(ShowSpecialThanks);

        specialThanksBg = transform.Find("SpecialThanks").gameObject.GetComponent<Image>();
        specialThanksText = transform.Find("SpecialThanks").Find("SpecialThanksText").gameObject.GetComponent<TMP_Text>();
    }

    public void StartRaffleProcess()
    {
        if (stopSeq == prizeSize) return;

        Action<int> updateShuffle = (int prizeID) =>
        {
            if (stopSeq != prizeID)
            {
                stopSeq = prizeID;
                prizeTags[prizeID-1].text = "Done!";
                prizeBtnLists[prizeID-1].enabled = true;
            }

            for (int i = stopSeq; i < 20; i++)
            {
                prizeTags[i].text = record.GetRandomMemberName();
            }
        };

        string thankTxt = specialThanksText.text;
        thankTxt += "<size=150%>~ Special Thanks ~\n\n<size=100%>";
        foreach (var member in prizeRemainMembers)
        {
            thankTxt += member.name + " : 2,000\n\n";
        }
        specialThanksText.text = thankTxt;

        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => stopSeq, prizeID => updateShuffle(prizeID), prizeSize, 10).SetEase(Ease.InCirc));
        seq.Append(prizeBtnThx.transform.DOScale(1f, 0.1f).SetDelay(0.5f));
        startBtn.transform.DOScale(0, 0.1f).SetEase(Ease.InBack).OnComplete(() => startBtn.gameObject.SetActive(false));
    }

    private void ShowPrizeDetail(int prizeID)
    {
        if (prizeShower.IsActivate()) return;
        if (isShowingSpecialThanks) return;

        prizeTags[prizeID].text = prizeOwners[prizeID].name;

        int idx = prizeID < 5 ? prizeID : 4 + prizeID / 5;
        string prizeText = prizeNames[idx];
        string bonusText = bonusNames[idx];
        bool hasShown = bonusHasShown[idx];

        if (bonusText.Equals(""))
        {
            prizeShower.UpdateInfo(prizeOwners[prizeID], prizeText);
        }
        else
        {
            prizeShower.UpdateInfoWithBonus(prizeOwners[prizeID], prizeText, bonusText, hasShown);
        }

        bonusHasShown[idx] = true;
        prizeShower.PopUp();
    }

    private void ShowSpecialThanks()
    {
        if (prizeShower.IsActivate()) return;
        if (isShowingSpecialThanks) return;

        isShowingSpecialThanks = true;
        specialThanksText.rectTransform.anchoredPosition = new Vector2(0, -2200);

        var seq = DOTween.Sequence();
        seq.Append(specialThanksBg.DOFade(1f, 0.75f));
        seq.Append(specialThanksText.rectTransform.DOAnchorPosY(2200, 20f).SetEase(Ease.Linear));
        seq.Append(specialThanksBg.DOFade(0f, 0.75f).OnComplete(() => isShowingSpecialThanks = false));
    }
}
