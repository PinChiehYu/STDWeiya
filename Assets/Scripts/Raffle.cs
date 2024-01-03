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

    public GameObject detailPanel;
    private Image detailPhoto;
    private TMP_Text detailName;
    private TMP_Text detailPrizeName;

    private const int prizeSize = 20;
    private int stopSeq = 0;

    // Start is called before the first frame update
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

        detailPhoto = detailPanel.transform.Find("Photo").gameObject.GetComponent<Image>();
        detailName = detailPanel.transform.Find("Name").gameObject.GetComponent<TMP_Text>();
        detailPrizeName = detailPanel.transform.Find("PrizeName").gameObject.GetComponent<TMP_Text>();
        detailPanel.SetActive(false);
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
        if (detailPanel.activeSelf) return;

        prizeTags[prizeID].text = prizeOwners[prizeID].name;

        detailName.text = prizeOwners[prizeID].name;
        detailPrizeName.text = prizeID < 5 ? prizeNames[prizeID] : prizeNames[4 + prizeID / 5];

        detailPanel.SetActive(true);
        detailPanel.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void ClosePrizeDetail()
    {
        detailPanel.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => detailPanel.SetActive(false));
    }
}
