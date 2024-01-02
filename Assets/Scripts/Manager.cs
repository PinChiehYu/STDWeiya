using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject startingObj;
    public GameObject raffleObj;
    public Image blackout;

    private Starting starting;
    private Raffle raffle;

    [HideInInspector]
    public DataRecord dataRecord;

    void Awake()
    {
        dataRecord = new DataRecord();
    }

    void Start()
    {
        starting = startingObj.GetComponent<Starting>();
        raffle = raffleObj.GetComponent<Raffle>();

        if (starting == null || raffle == null )
        {
            Debug.LogError("Fail to find components");
        }
    }

    public DataRecord GetDataRecord() {  return dataRecord; }
    public void StartRaffle()
    {
        Action ending = () =>
        {
            dataRecord.Raffle();
            raffleObj.SetActive(true);
            startingObj.SetActive(false);
        };

        var seq = DOTween.Sequence();
        seq.Append(DOTween.ToAlpha(() => blackout.color, x => blackout.color = x, 1, 0.5f).OnStepComplete(delegate { ending(); }));
        seq.AppendInterval(0.5f);
        seq.Append(DOTween.ToAlpha(() => blackout.color, x => blackout.color = x, 0, 0.5f).OnStepComplete(() => raffle.StartDisplay()));
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
