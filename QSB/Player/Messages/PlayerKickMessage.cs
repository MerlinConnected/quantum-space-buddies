﻿using Mirror;
using QSB.Menus;
using QSB.Messaging;
using QSB.Utility;
using System.Linq;

namespace QSB.Player.Messages
{
	/// <summary>
	/// always sent by host
	/// </summary>
	internal class PlayerKickMessage : QSBMessage<KickReason>
	{
		private uint PlayerId;

		public PlayerKickMessage(uint playerId, KickReason reason)
		{
			PlayerId = playerId;
			Value = reason;
		}

		public override void Serialize(NetworkWriter writer)
		{
			base.Serialize(writer);
			writer.Write(PlayerId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			base.Deserialize(reader);
			PlayerId = reader.Read<uint>();
		}

		public override void OnReceiveLocal()
		{
			if (!QSBCore.IsHost)
			{
				return;
			}

			QSBCore.UnityEvents.FireInNUpdates(KickPlayer, 10);
		}

		private void KickPlayer()
			=> NetworkServer.connections.Values.First(x => PlayerId == x.GetPlayerId()).Disconnect();

		public override void OnReceiveRemote()
		{
			if (PlayerId != QSBPlayerManager.LocalPlayerId)
			{
				if (QSBPlayerManager.PlayerExists(PlayerId))
				{
					DebugLog.ToAll($"{QSBPlayerManager.GetPlayer(PlayerId).Name} was kicked.");
					return;
				}

				DebugLog.ToAll($"Player id:{PlayerId} was kicked.");
				return;
			}

			DebugLog.ToAll($"Kicked from server. Reason : {Value}");
			MenuManager.Instance.OnKicked(Value);
		}
	}
}
