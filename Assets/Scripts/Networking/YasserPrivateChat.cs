using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace SanAndreasUnity.Net
{
    public sealed class YasserPrivateChat : NetworkBehaviour
    {
        private const int MaxMessageLength = 120;
        private const int MaxVisibleMessages = 6;
        private static readonly List<string> Messages = new List<string>();

        private string m_draft = string.Empty;
        private Vector2 m_scroll;

        [Command]
        private void CmdSendMessage(string raw)
        {
            string message = Sanitize(raw);
            if (string.IsNullOrWhiteSpace(message))
                return;

            string sender = GetComponent<Player>() != null
                ? GetComponent<Player>().PlayerName
                : "Player";

            RpcReceiveMessage(sender, message);
        }

        [ClientRpc]
        private void RpcReceiveMessage(string sender, string message)
        {
            string line = Sanitize(sender) + ": " + Sanitize(message);
            Messages.Add(line);
            while (Messages.Count > 30)
                Messages.RemoveAt(0);
        }

        private static string Sanitize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Replace("<", "").Replace(">", "")
                         .Replace("\r", " ").Replace("\n", " ").Trim();

            return value.Length > MaxMessageLength
                ? value.Substring(0, MaxMessageLength)
                : value;
        }

        private void OnGUI()
        {
            if (!isLocalPlayer)
                return;

            float width = Mathf.Min(Screen.width * 0.48f, 430f);
            float height = Mathf.Min(Screen.height * 0.30f, 190f);
            var panel = new Rect(12f, 12f, width, height);

            GUI.Box(panel, "YASSER GTA  |  PRIVATE CHAT");
            GUILayout.BeginArea(new Rect(panel.x + 8f, panel.y + 25f, panel.width - 16f, panel.height - 32f));
            m_scroll = GUILayout.BeginScrollView(m_scroll, GUILayout.Height(panel.height - 65f));
            int start = Mathf.Max(0, Messages.Count - MaxVisibleMessages);
            for (int i = start; i < Messages.Count; i++)
                GUILayout.Label(Messages[i]);
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            m_draft = GUILayout.TextField(m_draft, GUILayout.MinWidth(80f));
            if (GUILayout.Button("Send", GUILayout.Width(58f)))
            {
                string outgoing = Sanitize(m_draft);
                if (!string.IsNullOrWhiteSpace(outgoing))
                {
                    CmdSendMessage(outgoing);
                    m_draft = string.Empty;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
