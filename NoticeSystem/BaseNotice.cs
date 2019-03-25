using System;
using System.Collections.Generic;

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
        internal List<BaseNotice> childs;

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool isAlive { get; protected set; }

        /// <summary>
        /// 红点通知回调
        /// </summary>
        protected Action<BaseNotice, bool> notifyCallBack;

        internal virtual void AddNotifyCallBack(Action<BaseNotice, bool> action)
        {
            notifyCallBack += action;
        }

        internal virtual void Notify(BaseNotice notice,bool isAlive)
        {
            this.isAlive = isAlive;
            if (notifyCallBack != null)
            {
                notifyCallBack.Invoke(notice, isAlive);
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

        internal virtual void SetParent(BaseNotice parent)
        {
            this.parent = parent;
        }

        internal virtual void Reset()//子父级关系不重置
        {
            notifyCallBack = null;
            isAlive = false;
        }
    }
}

