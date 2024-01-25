using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
public class Member
{
    public string name;
    public string account;
    public int level;
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
    private List<Member> prizeRemainMembers;
    private List<string> memberNames;
    private int totalPoint;

    public DataRecord()
    {
        InitCollect();

        TextAsset txt = Resources.Load<TextAsset>("member");
        string[] lines = txt.text.Split('\r', '\n');

        foreach (string line in lines)
        {
            string[] vals = line.TrimEnd('\r', '\n').Split(":");
            if (vals.Length != 4)
            {
                Debug.Log("Fail to split: " + line);
                continue;
            }

            if (!Int32.TryParse(vals[2], out int tid))
            {
                Debug.Log("Fail to parse team ID: " + line);
                continue;
            }

            if (!Int32.TryParse(vals[3], out int lv))
            {
                Debug.Log("Fail to parse team ID: " + lv);
                continue;
            }

            Member member = new() { name = vals[0], account = vals[1], level = lv, teamID = tid, tableID = -1, point = 1 };

            members.Add(member);
            memberNames.Add(member.name);
            accountToMembers[member.account] = member;
            teamToMembers[member.teamID].Add(member);
            teamPoint[member.teamID]++;
            totalPoint++;
        }

        members.Sort(SortMember);
    }

    private void InitCollect()
    {
        members = new();
        memberNames = new();
        teamPoint = new();
        teamToMembers = new();
        accountToMembers = new();
        prizeOwners = new();
        prizeRemainMembers = new();
        for (int i = 0; i < 5; i++)
        {
            teamToMembers.Add(new());
            teamPoint.Add(0);
        }
        totalPoint = 0;
    }

    private int SortMember(Member a, Member b)
    {
        if (a.point < b.point) return 1;
        if (a.point > b.point) return -1;
        if (a.level < b.level) return 1;
        if (a.level > b.level) return -1;
        if (a.teamID != 4 && b.teamID == 4) return 1;
        if (a.teamID == 4 && b.teamID != 4) return -1;

        return a.name.CompareTo(b.name);
    }

    public bool AddPointToTeam(int teamID, bool revert = false)
    {
        if (teamID < 0 || teamID > 4) return false;

        int point = revert ? -1 : 1;

        foreach (Member member in teamToMembers[teamID])
        {
            member.point += point;
            teamPoint[teamID] += point;
            totalPoint += point;
        }

        members.Sort(SortMember);
        return true;
    }

    public bool AddPointToMember(string account, bool revert = false)
    {
        if (!accountToMembers.ContainsKey(account)) return false;

        int point = revert ? -1 : 1;

        accountToMembers[account].point += point;
        teamPoint[accountToMembers[account].teamID] += point;
        totalPoint += point;

        members.Sort(SortMember);
        return true;
    }

    public List<string> GetAllMemberNames()
    {
        return memberNames;
    }

    public string GetRandomMemberName()
    {
        return memberNames[UnityEngine.Random.Range(0, memberNames.Count)];
    }

    public int GetTotalPoint() { return totalPoint; }
    
    public int GetTeamPoint(int teamID) { return teamPoint[teamID]; }

    public List<Tuple<string, int>> GetTopMemberNamePoint(int topN = 5)
    {
        List<Tuple<string, int>> result = new();

        for (int i = 0; i < topN; i++)
        {
            result.Add(Tuple.Create(members[i].account, members[i].point));
        }

        return result;
    }

    public List<Member> GetPrizeOwners() { return prizeOwners; }

    public List<Member> GetPrizeRemainMembers() { return prizeRemainMembers; }

    public bool HasMemberAccount(string account)
    {
        return accountToMembers.ContainsKey(account);
    }

    public void Raffle()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        prizeRemainMembers = members;
        int remainPoint = totalPoint;
        int poll, cumulate;

        for (int i = 0; i < 20; i++)
        {
            poll = UnityEngine.Random.Range(0, remainPoint);
            cumulate = 0;
            foreach (Member member in prizeRemainMembers)
            {
                cumulate += member.point;
                if (cumulate > poll)
                {
                    prizeOwners.Add(member);
                    remainPoint -= member.point;
                    prizeRemainMembers.Remove(member);

                    break;
                }
            }
        }

        SaveResult();
    }

    private void SaveResult()
    {
        string path = Directory.GetCurrentDirectory() + "/prize_result.txt";
        List<string> prize = new() { "20000", "9000", "8000", "7000", "6000", "5000", "4000", "3000" };
        List<string> result = new List<string>();

        for (int i = 0; i < prizeOwners.Count; i++)
        {
            int idx = i < 5 ? i : 4 + i / 5;
            result.Add(prizeOwners[i].account + ":" + prize[idx]);
        }

        foreach (var member in prizeRemainMembers)
        {
            result.Add(member.account + ":2000");
        }

        File.WriteAllLines(path, result);
    }
}
