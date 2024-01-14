using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Leaderborad : MonoBehaviour
{
    private List<TMP_Text> rankNames = new();
    private List<TMP_Text> rankPoints = new();

    private DataRecord record;
    private const int listSize = 10;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();

        for (int i = 0; i < listSize; i++)
        {
            rankNames.Add(transform.Find("N" + i.ToString()).gameObject.GetComponent<TMP_Text>());
            rankPoints.Add(transform.Find("P" + i.ToString()).gameObject.GetComponent<TMP_Text>());
        }
    }

    public void UpdateRank()
    {
        List<Tuple<string, int>> NamePoints = record.GetTopMemberNamePoint(listSize);

        for (int i = 0; i < listSize; i++)
        {
            bool nameChange = false;
            if (rankNames[i].text != NamePoints[i].Item1)
            {
                int index = i;
                string target = NamePoints[i].Item1;
                float timer = 0f;

                DOTween.To(() => timer, x => { timer = x; SetRandomName(index); }, 1f, 0.5f).OnComplete(() => rankNames[index].text = target);
                nameChange = true;
            }

            if (rankPoints[i].text != NamePoints[i].Item2.ToString() || nameChange)
            {
                int index = i;
                string target = NamePoints[i].Item2.ToString();
                float timer = 0f;

                DOTween.To(() => timer, x => { timer = x; SetRandomPoint(index); }, 1f, 0.5f).OnComplete(() => rankPoints[index].text = target);
            }
        }
    }

    private void SetRandomName(int index)
    {
        rankNames[index].text = record.GetRandomMemberName();
    }

    private void SetRandomPoint(int index)
    {
        rankPoints[index].text = UnityEngine.Random.Range(1, 100).ToString();
    }
}
