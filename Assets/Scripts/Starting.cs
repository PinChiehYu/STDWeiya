using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Starting : MonoBehaviour
{
    private List<string> teamNames = new() { "FST", "PTT", "S3T", "SET", "STT" };

    [Header("Teams")]
    public List<Button> teamBtns;
    public List<Image> teamPieCharts;

    [Header("Input Field")]
    public TMP_InputField accountInputField;
    public TMP_Text nameInputFieldPlaceholder;
    public Button addSingleBtn;

    [Header("Misc")]
    public TMP_Text totalText;
    public Button raffleBtn;

    private DataRecord record;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();

        for (int i = 0; i < 5; i++)
        {
            int teamID = i;
            teamBtns[i].onClick.AddListener(() => AddPointToTeam(teamID));
        }
        addSingleBtn.onClick.AddListener(AddPointToMember);

        UpdateUI();
    }

    public void AddPointToTeam(int teamID)
    {
        record.AddPointToTeam(teamID);

        UpdateUI();
    }

    public void AddPointToMember()
    {
        if (record.AddPointToMember(accountInputField.text))
        {
            nameInputFieldPlaceholder.text = "<color=\"blue\">Add!</color>";
            UpdateUI();
        }
        else
        {
            nameInputFieldPlaceholder.text = "<color=\"red\">WUT?</color>";
        }

        accountInputField.text = "";
    }

    private void UpdateUI()
    {
        int totalPoint = record.GetTotalPoint();
        float cumulate = 0f;

        for (int i = 4; i >= 0; i--)
        {
            float cur;
            int team = i;

            cumulate += (float)record.GetTeamPoint(i) / totalPoint;
            cur = cumulate;

            DOTween.To(() => teamPieCharts[team].fillAmount, x => teamPieCharts[team].fillAmount = x, cur, 0.5f).SetEase(Ease.OutQuint);
        }

        totalText.text = "Total : " + totalPoint.ToString();
    }
}
