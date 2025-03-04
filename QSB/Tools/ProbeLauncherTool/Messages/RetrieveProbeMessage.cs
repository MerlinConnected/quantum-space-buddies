﻿using QSB.Messaging;
using QSB.Tools.ProbeLauncherTool.WorldObjects;

namespace QSB.Tools.ProbeLauncherTool.Messages;

internal class RetrieveProbeMessage : QSBWorldObjectMessage<QSBProbeLauncher, bool>
{
	public RetrieveProbeMessage(bool playEffects) : base(playEffects) { }

	public override void OnReceiveRemote() => WorldObject.RetrieveProbe(Data);
}