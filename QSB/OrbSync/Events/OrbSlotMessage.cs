﻿using QSB.WorldSync.Events;
using QuantumUNET.Transport;

namespace QSB.OrbSync.Events
{
	public class OrbSlotMessage : WorldObjectMessage
	{
		public int SlotIndex;
		public bool PlayAudio;

		public override void Deserialize(QNetworkReader reader)
		{
			base.Deserialize(reader);
			SlotIndex = reader.ReadInt32();
			PlayAudio = reader.ReadBoolean();
		}

		public override void Serialize(QNetworkWriter writer)
		{
			base.Serialize(writer);
			writer.Write(SlotIndex);
			writer.Write(PlayAudio);
		}
	}
}
