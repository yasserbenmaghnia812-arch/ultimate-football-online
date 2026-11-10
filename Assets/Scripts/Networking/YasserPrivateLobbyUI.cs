using Mirror;
using UnityEngine;

namespace SanAndreasUnity.Net
{
    public sealed class YasserPrivateLobbyUI : MonoBehaviour
    {
        private string m_address = "127.0.0.1";
        private string m_inviteCode = "YASSER8";
        private string m_status = "ادخل عنوان السيرفر ورمز الدعوة";

        private void OnGUI()
        {
            if (NetworkClient.active || NetworkServer.active)
                return;

            float w = Mathf.Min(Screen.width - 24f, 420f);
            float h = 230f;
            var rect = new Rect((Screen.width - w) * 0.5f, (Screen.height - h) * 0.5f, w, h);

            GUI.Box(rect, "YASSER GTA - PRIVATE SERVER");
            GUILayout.BeginArea(new Rect(rect.x + 14f, rect.y + 30f, rect.width - 28f, rect.height - 40f));
            GUILayout.Label("Server IP");
            m_address = GUILayout.TextField(m_address);
            GUILayout.Label("Invite code");
            m_inviteCode = GUILayout.PasswordField(m_inviteCode, '*');
            GUILayout.Space(8f);

            if (GUILayout.Button("Connect to private server", GUILayout.Height(42f)))
            {
                if (string.IsNullOrWhiteSpace(m_address) || string.IsNullOrWhiteSpace(m_inviteCode))
                {
                    m_status = "IP and invite code are required.";
                }
                else
                {
                    YasserInviteAuthenticator.ClientInviteCode = m_inviteCode.Trim();
                    NetworkManager.singleton.networkAddress = m_address.Trim();
                    m_status = "Connecting...";
                    NetworkManager.singleton.StartClient();
                }
            }

            GUILayout.Label(m_status);
            GUILayout.EndArea();
        }
    }
}
