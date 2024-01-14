using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Raffle : MonoBehaviour
{
    private List<Button> prizeBtnLists = new();
    private List<TMP_Text> prizeTags = new();

    private DataRecord record;
    private List<Member> prizeOwners;
    private List<string> prizeNames;

    public PrizeShower prizeShower;

    private const int prizeSize = 20;
    private int stopSeq = 0;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();
        prizeOwners = record.GetPrizeOwners();

        prizeNames = new() { "First Prize", "Second Prize", "Third Prize", "Forth Prize", "Fifth Prize", "Sixth Prize", "Seventh Prize", "Eighth Prize" };

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
    }

    public void ShowPrizeDetail(int prizeID)
    {
        if (prizeShower.IsActivate()) return;

        prizeTags[prizeID].text = prizeOwners[prizeID].name;

        string prizeText = prizeID < 5 ? prizeNames[prizeID] : prizeNames[4 + prizeID / 5];

        prizeShower.UpdateInfoWithBonus(prizeOwners[prizeID], prizeText, "10000000");
        prizeShower.PopUp();
    }
}
