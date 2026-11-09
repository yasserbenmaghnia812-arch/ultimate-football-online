using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class FootballTeamManagementUI : MonoBehaviour
{
    private FootballSquadSystem squad;
    private readonly Dictionary<string, FootballPlayerCard> cards = new Dictionary<string, FootballPlayerCard>();
    private bool visible;
    private int tab;
    private Rect panel = new Rect(20, 60, 540, 590);
    private GUIStyle label;

    private void Awake()
    {
        squad = GetComponent<FootballSquadSystem>();
        if (squad == null) squad = gameObject.AddComponent<FootballSquadSystem>();
        squad.InitializeDefaultRoster();
        foreach (FootballSquadMember member in squad.Squad)
        {
            FootballPlayerCard card = ScriptableObject.CreateInstance<FootballPlayerCard>();
            card.playerName = member.displayName;
            card.position = member.position;
            card.overall = member.overall;
            card.pace = Mathf.Clamp(member.overall + 2, 1, 99);
            card.shooting = Mathf.Clamp(member.overall + (member.position == "ST" ? 8 : -3), 1, 99);
            card.passing = member.overall;
            card.dribbling = Mathf.Clamp(member.overall + 1, 1, 99);
            card.defending = Mathf.Clamp(member.overall + (member.position == "CB" ? 8 : -8), 1, 99);
            card.physical = member.overall;
            cards[member.id] = card;
        }
    }

    private void OnGUI()
    {
        if (label == null) { label = new GUIStyle(GUI.skin.label); label.fontSize = 16; label.normal.textColor = Color.white; }
        if (GUI.Button(new Rect(Screen.width - 154, 12, 140, 42), "TEAM / CARDS")) visible = !visible;
        if (!visible) return;
        panel.width = Mathf.Min(540, Screen.width - 24);
        panel.height = Mathf.Min(590, Screen.height - 72);
        panel = GUI.Window(GetInstanceID(), panel, DrawPanel, "ULTIMATE FOOTBALL");
    }

    private void DrawPanel(int id)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(tab == 0, "SQUAD", "Button")) tab = 0;
        if (GUILayout.Toggle(tab == 1, "PLAYER CARDS", "Button")) tab = 1;
        GUILayout.EndHorizontal();
        if (tab == 0) DrawSquad();
        else DrawCards();
        if (GUILayout.Button("CLOSE", GUILayout.Height(38))) visible = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 25));
    }

    private void DrawSquad()
    {
        GUILayout.Label("Formation: " + squad.ActiveFormation + " | Substitutions: " + squad.SubstitutionsRemaining, label);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("4-3-3")) squad.SetFormation("4-3-3");
        if (GUILayout.Button("4-4-2")) squad.SetFormation("4-4-2");
        if (GUILayout.Button("4-2-3-1")) squad.SetFormation("4-2-3-1");
        if (GUILayout.Button("3-5-2")) squad.SetFormation("3-5-2");
        GUILayout.EndHorizontal();
        foreach (FootballSquadMember member in squad.Squad)
        {
            bool starter = squad.StartingEleven.Contains(member.id);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label((starter ? "XI  " : "BENCH ") + member.displayName + " | " + member.position + " | " + member.overall, label);
            GUILayout.EndHorizontal();
        }
        GUILayout.Label("Roster data is fictional prototype content.", label);
    }

    private void DrawCards()
    {
        foreach (FootballSquadMember member in squad.Squad)
        {
            FootballPlayerCard card = cards[member.id];
            GUILayout.BeginVertical("box");
            GUILayout.Label(card.playerName + "  |  " + card.position + "  |  " + card.tier + "  |  " + card.overall + " OVR", label);
            GUILayout.Label("PAC " + card.pace + "   SHO " + card.shooting + "   PAS " + card.passing + "   DRI " + card.dribbling, label);
            GUILayout.Label("DEF " + card.defending + "   PHY " + card.physical + "   STA " + card.stamina, label);
            GUILayout.EndVertical();
        }
    }
}
