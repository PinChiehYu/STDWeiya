using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Raffle : MonoBehaviour
{
    private List<Button> prizeBtnLists = new();
    private List<TMP_Text> prizeTags = new();

    private DataRecord record;
    private List<string> memberNames;
    private List<Member> prizeOwners;

    private int stopSeq;

    // Start is called before the first frame update
    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();
        memberNames = record.GetAllMemberNames();
        prizeOwners = record.GetPrizeOwners();
        stopSeq = prizeOwners.Count;

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

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < prizeTags.Count; i++) 
        {
            if (i < stopSeq)
            {
                prizeTags[i].text = memberNames[UnityEngine.Random.Range(0, memberNames.Count)];
            }
        }
    }

    public void StartDisplay()
    {
        Debug.Log("Yea");

    }

    public void StopRandom()
    {
        if (stopSeq == 0) return;

        Action<int> updateShuffle = (int prizeID) =>
        {
            Debug.Log(prizeID);
            stopSeq = prizeID;
            prizeTags[prizeID].text = "Done!";
            prizeBtnLists[prizeID].enabled = true;
        };

        stopSeq -= 1;
        DOTween.To(() => stopSeq, priceID => updateShuffle(priceID), 0, 10);
    }

    public void ShowPrizeDetail(int prizeID)
    {
        Debug.Log(prizeID);

        prizeTags[prizeID].text = prizeOwners[prizeID].name;
    }
}
