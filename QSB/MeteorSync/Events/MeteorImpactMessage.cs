﻿using QSB.WorldSync.Events;
using QuantumUNET.Transport;
using UnityEngine;

namespace QSB.MeteorSync.Events
{
	public class MeteorImpactMessage : WorldObjectMessage
	{
		public Vector3 ImpactPoint;
		public float Damage;

		public override void Deserialize(QNetworkReader reader)
		{
			base.Deserialize(reader);
			ImpactPoint = reader.ReadVector3();
			Damage = reader.ReadSingle();
		}

		public override void Serialize(QNetworkWriter writer)
		{
			base.Serialize(writer);
			writer.Write(ImpactPoint);
			writer.Write(Damage);
		}
	}
}
