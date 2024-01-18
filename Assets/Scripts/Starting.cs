using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System;

public class Starting : MonoBehaviour
{
    public class PointOP
    {
        public int OpCode; // 0 => Single / 1 => Team / 2 => Table / 3 => Donate
        public int ID;
        public string Account;
    }

    [Header("Team")]
    public GameObject teamPanel;
    public List<Button> teamBtns;
    public Piechart piechart;

    [Header("Single")]
    public GameObject singlePanel;
    public TMP_InputField accountInputField;
    public TMP_Text nameInputFieldPlaceholder;
    public Button addSingleBtn;

    [Header("Donate")]
    public GameObject donatePanel;
    public TMP_InputField donateAccountInputField;
    public TMP_InputField donateAmountInputField;
    public TMP_Text donateAccountInputFieldPlaceholder;
    public Button addDonateBtn;

    private int panelToggled = -1;
    private List<RectTransform> panelTransforms;
    private List<Tuple<float, float>> panelXPos;

    [Header("Misc")]
    public TMP_Text totalText;
    public TMP_Text announceText;
    public Leaderborad leaderboard;

    private DataRecord record;
    private Stack<PointOP> history;

    void Start()
    {
        record = FindObjectOfType<Manager>().GetDataRecord();
        history = new();

        panelToggled = -1;
        panelTransforms = new() {
            singlePanel.GetComponent<RectTransform>(),
            teamPanel.GetComponent<RectTransform>(),
            donatePanel.GetComponent<RectTransform>(),
        };
        panelXPos = new() { 
            new Tuple<float, float>(singlePanel.GetComponent<RectTransform>().position.x, -singlePanel.GetComponent<RectTransform>().position.x),
            new Tuple<float, float>(teamPanel.GetComponent<RectTransform>().position.x, -teamPanel.GetComponent<RectTransform>().position.x),
            new Tuple<float, float>(donatePanel.GetComponent<RectTransform>().position.x, -donatePanel.GetComponent<RectTransform>().position.x),
        };

        for (int i = 0; i < 5; i++)
        {
            int teamID = i;
            teamBtns[i].onClick.AddListener(() => AddPointToTeam(teamID));
        }
        addSingleBtn.onClick.AddListener(AddPointToMember);
        addDonateBtn.onClick.AddListener(AddDonate);

        announceText.gameObject.GetComponent<RectTransform>().DOAnchorPosX(-960f, 30f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        announceText.text = "Kenneth: 50000";
    }

    public void AddPointToMember()
    {
        if (record.AddPointToMember(accountInputField.text))
        {
            nameInputFieldPlaceholder.text = "<color=\"blue\">Add!</color>";

            history.Push(new PointOP() { OpCode = 0, Account = accountInputField.text });
            UpdateUI();
        }
        else
        {
            nameInputFieldPlaceholder.text = "<color=\"red\">WUT?</color>";
        }

        accountInputField.text = "";
    }

    public void AddPointToTeam(int teamID)
    {
        if (record.AddPointToTeam(teamID))
        {
            history.Push(new PointOP() { OpCode = 1, ID = teamID });
            UpdateUI();
        }
    }

    public void AddDonate()
    {
        if (record.HasMemberAccount(donateAccountInputField.text) && Int32.TryParse(donateAmountInputField.text, out _))
        {
            announceText.text += "      " + donateAccountInputField.text + ": " + donateAmountInputField.text;
            donateAccountInputFieldPlaceholder.text = "<color=\"blue\">Thank You!</color>";
        }
        else
        {
            donateAccountInputFieldPlaceholder.text = "<color=\"red\">WUT?</color>";
        }

        donateAccountInputField.text = "";
        donateAmountInputField.text = "";
    }

    public void RevertLastOP()
    {
        if (history.Count == 0) return;

        PointOP op = history.Pop();

        if (op.OpCode == 0)
            record.AddPointToMember(op.Account, true);
        else if (op.OpCode == 1)
            record.AddPointToTeam(op.ID, true);

        UpdateUI();
    }

    public void ToggleAddPointPanel(int opCode)
    {
        var seq = DOTween.Sequence();

        if (panelToggled != -1)
        {
            seq.Append(panelTransforms[panelToggled].DOAnchorPosX(panelXPos[panelToggled].Item1, 0.5f));
        }

        if (panelToggled != opCode)
        {
            seq.Append(panelTransforms[opCode].DOAnchorPosX(panelXPos[opCode].Item2, 0.5f));
            panelToggled = opCode;
        }
        else
        {
            panelToggled = -1;
        }
    }

    public void UpdateUI()
    {
        totalText.text = "Total : " + record.GetTotalPoint().ToString();
        piechart.UpdateChart();
        leaderboard.UpdateRank();
    }
}
