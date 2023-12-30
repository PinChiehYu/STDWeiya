using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
public class Member
{
    public string name;
    public string account;
    public int teamID;
    public int tableID;
    public int point;
}
public class DataRecord
{
    private List<Member> members;
    private List<List<Member>> teamToMembers;
    private List<int> teamPoint;
    private Dictionary<string, Member> accountToMembers;
    private List<Member> prizeOwners;
    private int totalPoint;
    public DataRecord()
    {
        InitCollect();

        TextAsset txt = Resources.Load<TextAsset>("member");
        string[] lines = txt.text.Split('\r', '\n');

        foreach (string line in lines)
        {
            string[] vals = line.TrimEnd('\r', '\n').Split(":");
            if (vals.Length != 3)
            {
                Debug.Log("Fail to split: " + line);
                continue;
            }

            if (!Int32.TryParse(vals[2], out int tid) || tid == 5)
            {
                Debug.Log("Fail to parse team ID: " + line);
                continue;
            }

            Member member = new() { name = vals[0], account = vals[1], teamID = Int32.Parse(vals[2]), tableID = -1, point = 1 };

            members.Add(member);
            accountToMembers[member.account] = member;
            teamToMembers[member.teamID].Add(member);
            teamPoint[member.teamID]++;
            totalPoint++;
        }
    }
    private void InitCollect()
    {
        members = new();
        teamPoint = new();
        teamToMembers = new();
        accountToMembers = new();
        prizeOwners = new();
        for (int i = 0; i < 5; i++)
        {
            teamToMembers.Add(new());
            teamPoint.Add(0);
        }
        totalPoint = 0;
    }

    public bool AddPointToTeam(int teamID)
    {
        if (teamID < 0 || teamID > 4) return false;

        foreach (Member member in teamToMembers[teamID])
        {
            member.point++;
            teamPoint[teamID]++;
            totalPoint++;
        }

        return true;
    }

    public bool AddPointToMember(string account)
    {
        if (!accountToMembers.ContainsKey(account)) return false;

        accountToMembers[account].point++;
        teamPoint[accountToMembers[account].teamID]++;
        totalPoint++;

        return true;
    }

    public List<string> GetAllMemberNames()
    {
        List<string> names = new();
        foreach (Member member in members)
        {
            names.Add(member.name);
        }

        return names;
    }
    public int GetTotalPoint() { return totalPoint; }
    
    public int GetTeamPoint(int teamID) { return teamPoint[teamID]; }

    public List<Member> GetPrizeOwners() { return prizeOwners; }

    public void StartRaffleOffPrice()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        List<Member> remainMembers = members;
        int remainPoint = totalPoint;
        int poll, cumulate;

        for (int i = 0; i < 20; i++)
        {
            poll = UnityEngine.Random.Range(0, remainPoint);
            cumulate = 0;
            foreach (Member member in remainMembers)
            {
                cumulate += member.point;
                if (cumulate > poll)
                {
                    Debug.Log(member.name);
                    prizeOwners.Add(member);
                    remainPoint -= member.point;
                    remainMembers.Remove(member);
                    break;
                }
            }
        }
    }
}