﻿using QSB.Events;
using QSB.Messaging;

namespace QSB.SatelliteSync.Events
{
	internal class SatelliteProjectorSnapshotEvent : QSBEvent<BoolMessage>
	{
		public override EventType Type => EventType.SatelliteProjectorSnapshot;

		public override void SetupListener() => GlobalMessenger<bool>.AddListener(EventNames.QSBSatelliteSnapshot, (bool forward) => Handler(forward));

		public override void CloseListener() => GlobalMessenger<bool>.RemoveListener(EventNames.QSBSatelliteSnapshot, (bool forward) => Handler(forward));

		private void Handler(bool forward) => SendEvent(CreateMessage(forward));

		private BoolMessage CreateMessage(bool forward) => new()
		{
			AboutId = LocalPlayerId,
			Value = forward
		};

		public override void OnReceiveRemote(bool isHost, BoolMessage message) => SatelliteProjectorManager.Instance.RemoteTakeSnapshot(message.Value);
	}
}
