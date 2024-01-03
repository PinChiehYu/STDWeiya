using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Piechart : MonoBehaviour
{
    public float textDistance = 150f;
    public float showTextPercent = 0.2f;
    public int maxTextSize = 100;

    private List<Image> teamPieCharts = new();
    private List<TMP_Text> teamChartTexts = new();

    private List<string> teamNames = new() { "FST", "PTT", "S3T", "SET", "STT" };
    private List<Tuple<float, float>> angleBaseDeltas = new();

    private DataRecord record;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();

        foreach (var name in teamNames)
        {
            teamPieCharts.Add(transform.Find(name).GetComponent<Image>());
            teamChartTexts.Add(transform.Find(name + "t").GetComponent<TMP_Text>());
            angleBaseDeltas.Add(Tuple.Create(0f, 0f));
        }
    }

    public void UpdateChart()
    {
        float deltaTime = 0f;
        float cumulate = 0f;
        int totalPoint = record.GetTotalPoint();

        for (int i = teamNames.Count - 1; i >= 0; i--)
        {
            cumulate += (float)record.GetTeamPoint(i) / totalPoint;
            angleBaseDeltas[i] = Tuple.Create(teamPieCharts[i].fillAmount, cumulate - teamPieCharts[i].fillAmount);
        }

        DOTween.To(() => deltaTime, x => { deltaTime = x; UpdateChartComponent(deltaTime); }, 1f, 0.5f).SetEase(Ease.OutQuint);
    }

    private void UpdateChartComponent(float deltaTime)
    {
        float prevTeamAngle = 0f, deltaAngle, angle;
        double alignRadian;

        for (int i = teamNames.Count - 1; i >= 0; i--)
        {
            angle = angleBaseDeltas[i].Item1 + angleBaseDeltas[i].Item2 * deltaTime;
            deltaAngle = angle - prevTeamAngle;

            alignRadian = (0.5 - angle - prevTeamAngle) * Math.PI;

            teamChartTexts[i].gameObject.GetComponent<RectTransform>().anchoredPosition
                = new Vector2(-(float)Math.Cos(alignRadian) * textDistance, (float)Math.Sin(alignRadian) * textDistance);
            teamChartTexts[i].fontSize = Math.Min(deltaAngle * maxTextSize * 5, maxTextSize);
            teamChartTexts[i].text = teamNames[i] + "\n" + ((int)Math.Round(deltaAngle * 100)).ToString() + "%";

            teamPieCharts[i].fillAmount = angle;
            prevTeamAngle = angle;
        }
    }
}
