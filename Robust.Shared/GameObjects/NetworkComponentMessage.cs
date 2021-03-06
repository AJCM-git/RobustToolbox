﻿using Robust.Shared.Interfaces.Network;
using Robust.Shared.Network.Messages;
using Robust.Shared.Utility;

namespace Robust.Shared.GameObjects
{
#nullable enable

    /// <summary>
    /// A container that holds a component message and network info.
    /// </summary>
    public readonly struct NetworkComponentMessage
    {
        /// <summary>
        /// Network channel this message came from.
        /// </summary>
        public readonly INetChannel Channel;

        /// <summary>
        /// Entity Uid this message is associated with.
        /// </summary>
        public readonly EntityUid EntityUid;

        /// <summary>
        /// If the Message is Directed, Component net Uid this message is being sent to.
        /// </summary>
        public readonly uint NetId;

        /// <summary>
        /// The message payload.
        /// </summary>
        public readonly ComponentMessage Message;

        /// <summary>
        /// Constructs a new instance of <see cref="NetworkComponentMessage"/>.
        /// </summary>
        /// <param name="netMsg">Raw network message containing the component message.</param>
        public NetworkComponentMessage(MsgEntity netMsg)
        {
            DebugTools.Assert(netMsg.Type == EntityMessageType.ComponentMessage);

            Channel = netMsg.MsgChannel;
            EntityUid = netMsg.EntityUid;
            NetId = netMsg.NetId;
            Message = netMsg.ComponentMessage;
        }
    }

#nullable restore
}
