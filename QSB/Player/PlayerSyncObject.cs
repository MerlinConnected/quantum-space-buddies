﻿using QuantumUNET;

namespace QSB.Player
{
	public abstract class PlayerSyncObject : QNetworkBehaviour
	{
		public uint PlayerId => NetId.Value;
		public PlayerInfo Player => QSBPlayerManager.GetPlayer(PlayerId);

		protected virtual void Start() => QSBPlayerManager.AddSyncObject(this);
		protected virtual void OnDestroy() => QSBPlayerManager.RemoveSyncObject(this);
	}
}