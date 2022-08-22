﻿using Discord;
using DiscordMirror;
using Mirror;
using QSB.Utility;
using UnityEngine;

namespace QSB.RichPresence;

internal class DiscordJoinManager : MonoBehaviour, IAddComponentOnStart
{
	public void Start()
	{
		if (QSBCore.DebugSettings.UseKcpTransport)
		{
			Destroy(this);
			return;
		}

		var discordTransport = (DiscordTransport)Transport.activeTransport;
		var activityManager = discordTransport.discordClient.GetActivityManager();

		activityManager.OnActivityJoin += OnActivityJoin;
		activityManager.OnActivityJoinRequest += OnActivityJoinRequest;

	}

	private void OnActivityJoin(string secret)
	{
		// called when the LOCAL player clicks join
		DebugLog.DebugWrite($"OnActivityJoin {secret}");
	}

	private void OnActivityJoinRequest(ref User user)
	{
		// called when someone clicks "ask to join"
		DebugLog.DebugWrite($"OnActivityJoinRequest {user.Username} {user.Id}");
	}
}
