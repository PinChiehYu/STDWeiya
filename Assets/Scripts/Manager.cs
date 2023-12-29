using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public class Member
    {
        public string name;
        public int teamID;
        public int tableID;
        public int point;
    }

    private List<string> teamNames = new List<string> { "FST", "PTT", "S3T", "SET", "STT" };
    private Dictionary<string, Member> nameToMember = new Dictionary<string, Member>();
    private List<int> teamPoint;
    private int totalPoint = 0;

    [HeaderAttribute("Stage 1")]
    [HeaderAttribute("Main Obj")]
    public GameObject stageAUI;

    [HeaderAttribute("Teams")]
    public List<Button> teamBtns;
    public List<Image> teamPieCharts;

    [HeaderAttribute("Input Field")]
    public TMP_InputField nameInputField;
    public TMP_Text nameInputFieldPlaceholder;
    public Button addSingleBtn;

    [HeaderAttribute("Misc")]
    public TMP_Text totalText;
    public Button raffleBtn;

    [HeaderAttribute("Stage 2")]
    [HeaderAttribute("UI Element")]
    public GameObject stageBUI;

    void Start()
    {
        teamPoint = new List<int>{ 10, 10, 9, 9, 9 };
        for (int i = 0; i < 5; i++)
        {
            int teamID = i;

            totalPoint += teamPoint[i];
            teamBtns[i].onClick.AddListener(() => AddPointToTeam(teamID));
        }

        addSingleBtn.onClick.AddListener(AddPointToUser);
        raffleBtn.onClick.AddListener(startRaffle);

        UpdateUI();
    }

    void Update()
    {

    }

    public void AddPointToTeam(int team)
    {
        Debug.Log(team);

        teamPoint[team]++;
        totalPoint++;

        UpdateUI();
    }

    public void AddPointToUser()
    {
        string name = nameInputField.text;

        if (nameToMember.ContainsKey(name))
        {
            nameToMember[name].point++;
            nameInputFieldPlaceholder.text = "<color=\"blue\">Add!</color>";
        }
        else
        {
            nameInputFieldPlaceholder.text = "<color=\"red\">WUT?</color>";
        }

        nameInputField.text = "";
    }
    private void UpdateUI()
    {
        float cumulate = 0f;
        for (int i = 4; i >= 0; i--)
        {
            cumulate += (float)teamPoint[i] / totalPoint;
            teamPieCharts[i].fillAmount = cumulate;
        }

        totalText.text = "Total : " + totalPoint.ToString();
    }

    public void startRaffle()
    {
        Debug.Log("Start Raffle!");

        stageAUI.SetActive(false);
        stageBUI.SetActive(true);
    }
}
