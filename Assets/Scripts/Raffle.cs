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

    private DataRecord record;
    private List<Member> prizeOwners;
    private List<string> prizeNames;
    private List<string> bonusNames;

    public PrizeShower prizeShower;

    private const int prizeSize = 20;
    private int stopSeq = 0;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();
        prizeOwners = record.GetPrizeOwners();

        prizeNames = new() { "10,000", "9,000", "8,000", "7,000", "6,000", "4,000", "3,600", "2,500" };
        bonusNames = new() { "20,000", "",      "",      "",      "",      "5,000", "4,000", "3,000" };

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
    }

    public void StartDisplay()
    {
        Debug.Log("Yea");
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

        DOTween.To(() => stopSeq, prizeID => updateShuffle(prizeID), prizeSize, 10).SetEase(Ease.InCirc);
        startBtn.transform.DOScale(0, 0.1f).SetEase(Ease.InBack).OnComplete(() => startBtn.gameObject.SetActive(false));
    }

    public void ShowPrizeDetail(int prizeID)
    {
        if (prizeShower.IsActivate()) return;

        prizeTags[prizeID].text = prizeOwners[prizeID].name;

        string prizeText = prizeID < 5 ? prizeNames[prizeID] : prizeNames[4 + prizeID / 5];
        string bonusText = prizeID < 5 ? bonusNames[prizeID] : bonusNames[4 + prizeID / 5];

        if (bonusText.Equals(""))
        {
            prizeShower.UpdateInfo(prizeOwners[prizeID], prizeText);
        }
        else
        {
            prizeShower.UpdateInfoWithBonus(prizeOwners[prizeID], prizeText, bonusText);
        }

        prizeShower.PopUp();
    }
}
