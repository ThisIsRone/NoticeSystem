using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoticeSystem
{
    public class BaseNotice
    {
        internal BaseNotice(NoticeType noticeType)
        {
            this.noticeType = noticeType;
        }
        /// <summary>
        /// 红点类型
        /// </summary>
        public NoticeType noticeType { get;private set; }

        /// <summary>
        /// 父级Tip
        /// </summary>
        internal BaseNotice parent { get; private set; }

        /// <summary>
        /// 子级Tip
        /// </summary>
        internal List<BaseNotice> childs { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool isAlive { get; protected set; }

        /// <summary>
        /// 红点通知回调
        /// </summary>
        internal List<MonoNoticeAction> notifyCallBacks { get; private set; }

        internal virtual void Notify(BaseNotice notice,bool isAlive)
        {
            this.isAlive = isAlive;
            if (notifyCallBacks != null)
            {
                for (int i = 0; i < notifyCallBacks.Count; i++)
                {
                    notifyCallBacks[i].Invoke();
                }
            }
        }

        internal virtual void AddChild(BaseNotice child)
        {
            if (child != null)
            {
                if (childs == null)
                {
                    childs = new List<BaseNotice>();
                }

                if (!childs.Contains(child))
                {
                    childs.Add(child);
                }
            }
        }

        internal virtual void RemoveChild(BaseNotice child)
        {
            if (child != null)
            {
                if (childs != null)
                {
                    if (childs.Contains(child))
                    {
                        childs.Remove(child);
                    }
                }
            }
        }

        internal virtual void RegisterCallBack(Transform root, Action<BaseNotice> notifyCallBack)
        {
            MonoNoticeAction monoNotice = root.GetComponent<MonoNoticeAction>();
            if (monoNotice == null)
            {
                monoNotice = root.gameObject.AddComponent<MonoNoticeAction>();
                if (notifyCallBacks  == null)
                {
                    notifyCallBacks = new List<MonoNoticeAction>();
                }
                monoNotice.SetOwn(parent);
                notifyCallBacks.Add(monoNotice);
            }
            monoNotice.AddCallBack(notifyCallBack);
        }

        internal virtual void UnregisterCallBack(Transform root, Action<BaseNotice> notifyCallBack)
        {
            MonoNoticeAction monoNotice = root.GetComponent<MonoNoticeAction>();
            if (monoNotice != null)
            {
                monoNotice.SetOwn(parent);
                monoNotice.RemoveCallBack(notifyCallBack);
                if (monoNotice.CallCount == 0)
                {
                    GameObject.Destroy(monoNotice);
                }
            }
        }

        internal void RemoveMonoAction(MonoNoticeAction monoNotice)
        {
            if (notifyCallBacks != null)
            {
                if (notifyCallBacks.Contains(monoNotice))
                {
                    notifyCallBacks.Remove(monoNotice);
                }
            }
        }

        internal virtual void SetParent(BaseNotice parent)
        {
            this.parent = parent;
        }

        internal virtual void Clear()
        {
            notifyCallBacks.Clear();
        }

        internal virtual void Reset()//子父级关系不重置
        {
            for (int i = 0; i < notifyCallBacks.Count; i++)
            {
                GameObject.Destroy(notifyCallBacks[i]);
            }
            notifyCallBacks.Clear();
            isAlive = false;
        }
    }
}

