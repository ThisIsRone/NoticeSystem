using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoticeSystem
{

    public class MonoNoticeAction : MonoBehaviour
    {
        internal BaseNotice ownNotice { get; private set; }

        internal int CallCount
        {
            get
            {
                return callCache.Count;
            }
        }

        private List<Action<BaseNotice>> callCache = new List<Action<BaseNotice>>();

        internal void SetOwn(BaseNotice ownNotice)
        {
            this.ownNotice = ownNotice;
        }

        internal void AddCallBack(Action<BaseNotice> notifyCallBack)
        {
            if (notifyCallBack != null)
            {
                if (!callCache.Contains(notifyCallBack))
                {
                    callCache.Add(notifyCallBack);
                }
            }
        }

        internal void RemoveCallBack(Action<BaseNotice> notifyCallBack)
        {
            if (notifyCallBack != null)
            {
                if (callCache.Contains(notifyCallBack))
                {
                    callCache.Remove(notifyCallBack);
                }
            }
        }

        internal void Invoke()
        {
            for (int i = 0; i < callCache.Count; i++)
            {
                callCache[i].Invoke(ownNotice);
            }
        }

        private void OnDestroy()
        {
            ownNotice.RemoveMonoAction(this);
        }
    }
}