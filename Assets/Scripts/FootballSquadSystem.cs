using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class FootballSquadMember
{
    public string id;
    public string displayName;
    public string position;
    [Range(1, 99)] public int overall = 70;
    [Range(0, 100)] public int fitness = 100;
    public bool isAvailable = true;
}

[Serializable]
public sealed class FootballFormationDefinition
{
    public string id = "4-3-3";
    public Vector2[] normalizedPositions;
}

/// <summary>Roster validation, formation selection, and legal substitution bookkeeping.</summary>
public sealed class FootballSquadSystem : MonoBehaviour
{
    [SerializeField] private List<FootballSquadMember> squad = new List<FootballSquadMember>();
    [SerializeField] private string activeFormation = "4-3-3";
    [SerializeField, Range(0, 5)] private int substitutionsRemaining = 5;
    private readonly List<string> startingEleven = new List<string>();

    public IReadOnlyList<FootballSquadMember> Squad => squad;
    public IReadOnlyList<string> StartingEleven => startingEleven;
    public int SubstitutionsRemaining => substitutionsRemaining;
    public string ActiveFormation => activeFormation;

    public bool SetStartingEleven(IList<string> memberIds)
    {
        if (memberIds == null || memberIds.Count != 11) return false;
        HashSet<string> unique = new HashSet<string>();
        for (int i = 0; i < memberIds.Count; i++)
        {
            FootballSquadMember member = Find(memberIds[i]);
            if (member == null || !member.isAvailable || !unique.Add(member.id)) return false;
        }
        startingEleven.Clear();
        for (int i = 0; i < memberIds.Count; i++) startingEleven.Add(memberIds[i]);
        return true;
    }

    public bool SetFormation(string formationId)
    {
        if (formationId != "4-4-2" && formationId != "4-3-3" &&
            formationId != "4-2-3-1" && formationId != "3-5-2") return false;
        activeFormation = formationId;
        return true;
    }

    public bool Substitute(string outgoingId, string incomingId)
    {
        if (substitutionsRemaining <= 0 || outgoingId == incomingId) return false;
        int index = startingEleven.IndexOf(outgoingId);
        FootballSquadMember outgoing = Find(outgoingId);
        FootballSquadMember incoming = Find(incomingId);
        if (index < 0 || outgoing == null || incoming == null || !incoming.isAvailable) return false;
        if (startingEleven.Contains(incomingId)) return false;
        startingEleven[index] = incomingId;
        outgoing.isAvailable = false;
        incoming.isAvailable = false;
        substitutionsRemaining--;
        return true;
    }

    public void ResetMatchSubstitutions(int allowed = 5)
    {
        substitutionsRemaining = Mathf.Clamp(allowed, 0, 5);
        for (int i = 0; i < squad.Count; i++) squad[i].isAvailable = true;
    }

    private FootballSquadMember Find(string memberId)
    {
        if (string.IsNullOrEmpty(memberId)) return null;
        return squad.Find(member => member != null && member.id == memberId);
    }
}
