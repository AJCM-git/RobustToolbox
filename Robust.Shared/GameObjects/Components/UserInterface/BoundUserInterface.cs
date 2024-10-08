using System;
using System.Collections.Generic;
using Robust.Shared.IoC;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Robust.Shared.GameObjects
{
    /// <summary>
    ///     An abstract class to override to implement bound user interfaces.
    /// </summary>
    public abstract class BoundUserInterface : IDisposable
    {
        [Dependency] protected readonly IEntityManager EntMan = default!;
        [Dependency] private readonly ISharedPlayerManager _playerManager = default!;
        protected readonly SharedUserInterfaceSystem UiSystem;

        public readonly Enum UiKey;
        public EntityUid Owner { get; }

        /// <summary>
        /// Additional controls to be disposed when this BUI is disposed.
        /// </summary>
        internal List<IDisposable>? Disposals;

        /// <summary>
        ///     The last received state object sent from the server.
        /// </summary>
        protected internal BoundUserInterfaceState? State { get; internal set; }

        // Bandaid just for storage :)
        /// <summary>
        /// Defers state handling
        /// </summary>
        [Obsolete]
        public virtual bool DeferredClose { get; } = true;

        protected BoundUserInterface(EntityUid owner, Enum uiKey)
        {
            IoCManager.InjectDependencies(this);
            UiSystem = EntMan.System<SharedUserInterfaceSystem>();

            Owner = owner;
            UiKey = uiKey;
        }

        /// <summary>
        ///     Invoked when the UI is opened.
        ///     Do all creation and opening of things like windows in here.
        /// </summary>
        protected internal virtual void Open()
        {
        }

        /// <summary>
        ///     Invoked when the server uses <c>SetState</c>.
        /// </summary>
        protected internal virtual void UpdateState(BoundUserInterfaceState state)
        {
        }

        /// <summary>
        /// Helper method that gets called upon prototype reload.
        /// </summary>
        public virtual void OnProtoReload(PrototypesReloadedEventArgs args)
        {

        }

        /// <summary>
        ///     Invoked when the server sends an arbitrary message.
        /// </summary>
        protected internal virtual void ReceiveMessage(BoundUserInterfaceMessage message)
        {
        }

        /// <summary>
        ///     Invoked to close the UI.
        /// </summary>
        public void Close()
        {
            UiSystem.CloseUi(Owner, UiKey, _playerManager.LocalEntity, predicted: true);
        }

        /// <summary>
        ///     Sends a message to the server-side UI.
        /// </summary>
        public void SendMessage(BoundUserInterfaceMessage message)
        {
            UiSystem.ClientSendUiMessage(Owner, UiKey, message);
        }

        public void SendPredictedMessage(BoundUserInterfaceMessage message)
        {
            UiSystem.SendPredictedUiMessage(this, message);
        }

        ~BoundUserInterface()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Disposals != null)
                {
                    foreach (var control in Disposals)
                    {
                        control.Dispose();
                    }

                    Disposals = null;
                }
            }
        }
    }
}
