﻿using Mirror;
using QSB.WorldSync;
using System;

namespace QSB.Utility;

public abstract class QSBNetworkBehaviour : NetworkBehaviour
{
	protected virtual float SendInterval => 0.1f;
	protected virtual bool UseReliableRpc => false;

	private double _lastSendTime;
	private byte[] _lastKnownData;

	protected abstract bool HasChanged();
	protected abstract void UpdatePrevData();
	protected abstract void Serialize(NetworkWriter writer);
	protected abstract void Deserialize(NetworkReader reader);

	protected virtual void Update()
	{
		if (!hasAuthority || !QSBWorldSync.AllObjectsReady)
		{
			return;
		}

		if (NetworkTime.localTime >= _lastSendTime + SendInterval)
		{
			_lastSendTime = NetworkTime.localTime;

			if (!HasChanged())
			{
				return;
			}

			using var writer = NetworkWriterPool.GetWriter();
			Serialize(writer);
			UpdatePrevData();

			var data = writer.ToArraySegment();
			if (UseReliableRpc)
			{
				CmdSendDataReliable(data);
			}
			else
			{
				CmdSendDataUnreliable(data);
			}

			if (QSBCore.IsHost)
			{
				_lastKnownData ??= new byte[data.Count];
				Array.Copy(data.Array!, data.Offset, _lastKnownData, 0, data.Count);
			}
		}
	}

	/// <summary>
	/// called on the host to send the last known data to a new client
	/// <para/>
	/// world objects will be ready on both sides at this point
	/// </summary>
	public void SendInitialState(NetworkConnectionToClient target)
	{
		if (_lastKnownData != null)
		{
			TargetSendInitialData(target, new ArraySegment<byte>(_lastKnownData));
		}
	}

	[Command(channel = Channels.Reliable, requiresAuthority = true)]
	private void CmdSendDataReliable(ArraySegment<byte> data) => RpcSendDataReliable(data);

	[ClientRpc(channel = Channels.Reliable, includeOwner = false)]
	private void RpcSendDataReliable(ArraySegment<byte> data) => OnData(data);

	[Command(channel = Channels.Unreliable, requiresAuthority = true)]
	private void CmdSendDataUnreliable(ArraySegment<byte> data) => RpcSendDataUnreliable(data);

	[ClientRpc(channel = Channels.Unreliable, includeOwner = false)]
	private void RpcSendDataUnreliable(ArraySegment<byte> data) => OnData(data);

	[TargetRpc(channel = Channels.Reliable)]
	private void TargetSendInitialData(NetworkConnection target, ArraySegment<byte> data) => OnData(data);

	private void OnData(ArraySegment<byte> data)
	{
		if (hasAuthority || !QSBWorldSync.AllObjectsReady)
		{
			return;
		}

		if (QSBCore.IsHost)
		{
			_lastKnownData ??= new byte[data.Count];
			Array.Copy(data.Array!, data.Offset, _lastKnownData, 0, data.Count);
		}

		using var reader = NetworkReaderPool.GetReader(data);
		UpdatePrevData();
		Deserialize(reader);
	}
}