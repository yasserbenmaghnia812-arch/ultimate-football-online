using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

/// <summary>
/// Creates Unity Multiplayer Services rooms with Relay and a 22-player capacity.
/// This provides room creation/join codes; it does not yet replicate the football simulation.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballOnlineRooms : MonoBehaviour
{
    private ISession currentSession;
    private string joinCodeInput = "";
    private string status = "Online rooms: not connected";
    private bool busy;
    private bool visible;
    private Rect panel = new Rect(18, 56, 430, 270);

    public string CurrentJoinCode => currentSession != null ? currentSession.Code : "";

    private void Awake()
    {
        EnsureNetworkManager();
    }

    private void EnsureNetworkManager()
    {
        if (NetworkManager.Singleton != null) return;
        GameObject networkObject = new GameObject("FootballNetworkManager");
        UnityTransport transport = networkObject.AddComponent<UnityTransport>();
        NetworkManager manager = networkObject.AddComponent<NetworkManager>();
        manager.NetworkConfig = new NetworkConfig
        {
            NetworkTransport = transport,
            EnableSceneManagement = false
        };
        DontDestroyOnLoad(networkObject);
    }

    private async Task EnsureServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
            await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRoom()
    {
        if (busy || currentSession != null) return;
        busy = true;
        status = "Connecting to Unity services...";
        try
        {
            await EnsureServices();
            SessionOptions options = new SessionOptions
            {
                Name = "Ultimate Football 11v11",
                MaxPlayers = 22,
                IsPrivate = false
            }.WithRelayNetwork();
            currentSession = await MultiplayerService.Instance.CreateSessionAsync(options);
            status = "Room created. Share the join code.";
        }
        catch (Exception exception)
        {
            status = "Create room failed: " + exception.Message;
            Debug.LogException(exception);
        }
        finally { busy = false; }
    }

    public async void JoinRoom()
    {
        if (busy || currentSession != null || string.IsNullOrWhiteSpace(joinCodeInput)) return;
        busy = true;
        status = "Joining room...";
        try
        {
            await EnsureServices();
            currentSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCodeInput.Trim());
            status = "Joined room.";
        }
        catch (Exception exception)
        {
            status = "Join failed: " + exception.Message;
            Debug.LogException(exception);
        }
        finally { busy = false; }
    }

    public async void LeaveRoom()
    {
        if (busy || currentSession == null) return;
        busy = true;
        try
        {
            await currentSession.LeaveAsync();
            currentSession = null;
            status = "Left room.";
        }
        catch (Exception exception)
        {
            status = "Leave failed: " + exception.Message;
            Debug.LogException(exception);
        }
        finally { busy = false; }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(18, 12, 145, 40), "ONLINE ROOM")) visible = !visible;
        if (visible) panel = GUI.Window(GetInstanceID(), panel, DrawPanel, "ONLINE MULTIPLAYER");
    }

    private void DrawPanel(int id)
    {
        GUILayout.Label(status, GUILayout.MinHeight(38));
        if (currentSession == null)
        {
            GUI.enabled = !busy;
            if (GUILayout.Button("CREATE 22-PLAYER ROOM", GUILayout.Height(40))) CreateRoom();
            GUILayout.Space(8);
            joinCodeInput = GUILayout.TextField(joinCodeInput, 16);
            if (GUILayout.Button("JOIN WITH CODE", GUILayout.Height(38))) JoinRoom();
        }
        else
        {
            GUILayout.Label("Room code: " + currentSession.Code);
            GUILayout.Label("Members: " + currentSession.Players.Count + " / 22");
            if (GUILayout.Button("COPY INVITE CODE", GUILayout.Height(38)))
                GUIUtility.systemCopyBuffer = currentSession.Code;
            if (GUILayout.Button("LEAVE ROOM", GUILayout.Height(38))) LeaveRoom();
        }
        GUI.enabled = true;
        if (GUILayout.Button("CLOSE")) visible = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 24));
    }
}
