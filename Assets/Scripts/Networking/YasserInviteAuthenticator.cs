using System;
using Mirror;
using UnityEngine;

namespace SanAndreasUnity.Net
{
    public sealed class YasserInviteAuthenticator : NetworkAuthenticator
    {
        [SerializeField, Tooltip("Share this private room code only with invited friends.")]
        private string serverInviteCode = "CHANGE-THIS-CODE";

        public static string ClientInviteCode = string.Empty;

        public struct InviteRequest : NetworkMessage
        {
            public string code;
        }

        public struct InviteResponse : NetworkMessage
        {
            public bool accepted;
            public string reason;
        }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<InviteRequest>(OnServerInviteRequest, false);
        }

        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<InviteResponse>(OnClientInviteResponse, false);
        }

        public override void OnServerAuthenticate(NetworkConnectionToClient conn)
        {
            // Authentication completes only after the invite request is checked.
        }

        public override void OnClientAuthenticate()
        {
            NetworkClient.Send(new InviteRequest
            {
                code = (ClientInviteCode ?? string.Empty).Trim()
            });
        }

        private void OnServerInviteRequest(NetworkConnectionToClient conn, InviteRequest message)
        {
            string configuredCode = Environment.GetEnvironmentVariable("YASSER_GTA_INVITE_CODE");
            string expected = (string.IsNullOrWhiteSpace(configuredCode) ? serverInviteCode : configuredCode).Trim();
            string supplied = (message.code ?? string.Empty).Trim();

            bool valid = expected.Length >= 6 &&
                         supplied.Length == expected.Length &&
                         string.Equals(supplied, expected, StringComparison.Ordinal);

            if (valid)
            {
                conn.Send(new InviteResponse { accepted = true, reason = string.Empty });
                ServerAccept(conn);
            }
            else
            {
                conn.Send(new InviteResponse { accepted = false, reason = "Invalid invite code" });
                ServerReject(conn);
            }
        }

        private void OnClientInviteResponse(InviteResponse message)
        {
            if (message.accepted)
                ClientAccept();
            else
                ClientReject();
        }
    }
}
