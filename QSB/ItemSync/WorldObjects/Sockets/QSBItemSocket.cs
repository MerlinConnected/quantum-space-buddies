﻿using QSB.ItemSync.WorldObjects.Items;
using QSB.WorldSync;

namespace QSB.ItemSync.WorldObjects.Sockets
{
	internal class QSBItemSocket : WorldObject<OWItemSocket>
	{
		public override void SendInitialState(uint to)
		{
			// todo SendResyncInfo
		}

		public bool IsSocketOccupied()
			=> AttachedObject.IsSocketOccupied();

		public void PlaceIntoSocket(IQSBItem item)
			=> AttachedObject.PlaceIntoSocket((OWItem)item.ReturnObject());

		public void RemoveFromSocket()
			=> AttachedObject.RemoveFromSocket();
	}
}
