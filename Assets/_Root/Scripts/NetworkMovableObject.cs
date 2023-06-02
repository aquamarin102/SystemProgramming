using Mirror;
using System;
using UnityEngine;
namespace Network
{
    public abstract class NetworkMovableObject : NetworkBehaviour
    {
        protected abstract float _speed { get; }
        protected Action _onUpdateAction { get; set; }
        protected Action _onFixedUpdateAction { get; set; }
        protected Action _onLateUpdateAction { get; set; }
        protected Action _onPreRenderActionAction { get; set; }
        protected Action _onPostRenderAction { get; set; }

        [SyncVar] protected Vector3 _serverPosition;
        [SyncVar] protected Vector3 _serverEuler;

        public override void OnStartAuthority()
        {
            //Initiate();
        }
        protected virtual void Initiate(UpdatePhase updatePhase =
        UpdatePhase.Update)
        {
            switch (updatePhase)
            {
                case UpdatePhase.Update:
                    _onUpdateAction += Movement;
                    break;
                case UpdatePhase.FixedUpdate:
                    _onFixedUpdateAction += Movement;
                    break;
                case UpdatePhase.LateUpdate:
                    _onLateUpdateAction += Movement;
                    break;
                case UpdatePhase.PostRender:
                    _onPostRenderAction += Movement;
                    break;
                case UpdatePhase.PreRender:
                    _onPreRenderActionAction += Movement;
                    break;
            }
        }
        private void Update()
        {
            _onUpdateAction?.Invoke();
        }
        private void LateUpdate()
        {
            _onLateUpdateAction?.Invoke();
        }
        private void FixedUpdate()
        {
            _onFixedUpdateAction?.Invoke();
        }
        private void OnPreRender()
        {
            _onPreRenderActionAction?.Invoke();
        }
        private void OnPostRender()
        {
            _onPostRenderAction?.Invoke();
        }
        protected virtual void Movement()
        {
            if (isServer || isOwned)
            {
                HasAuthorityMovement();
            }
            else
            {
                FromServerUpdate();
            }
        }
        protected abstract void HasAuthorityMovement();
        protected abstract void FromServerUpdate();
        protected abstract void SendToServer();
    }

    public enum UpdatePhase
    {
        Update,
        FixedUpdate,
        LateUpdate,
        PostRender,
        PreRender
    }
}
