using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoticeSystem
{
    public class NoticeSystemManager
    {
        private NoticeSystemManager()
        {

        }

        private static NoticeSystemManager instance = null;

        public static NoticeSystemManager Instance
        {
            get
            {
                if (instance ==null)
                {
                    instance = new NoticeSystemManager();
                }
                return instance;
            }
        }

        private Dictionary<NoticeType, BaseNotice> noticeDic = new Dictionary<NoticeType, BaseNotice>();

        public void InitSystem()
        {
            registerNotice(NoticeType.Main_Friend);
            registerNotice(NoticeType.Friend_MyFriend, NoticeType.Main_Friend);
            registerNotice(NoticeType.Root_MyFriend_Friend, NoticeType.Friend_MyFriend);
            //registerNotice(NoticeType.Root_Friend_Item, NoticeType.Root_MyFriend_Friend);

            registerNotice(NoticeType.Root_MyFriend_RecentMan, NoticeType.Friend_MyFriend);
            //registerNotice(NoticeType.Root_RecentMan_item, NoticeType.Root_MyFriend_RecentMan);

            registerNotice(NoticeType.Root_Friend_Apply, NoticeType.Main_Friend);
        }

        /// <summary>
        /// 注册红点变更回调
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="isAlive"></param>
        public void RegiNotifyCallBack(NoticeType noticeType, Action<BaseNotice, bool> action,bool immediateInvoke = true)
        {
            BaseNotice notice;
            if (noticeDic.TryGetValue(noticeType, out notice))
            {
                notice.AddNotifyCallBack(action);
                if (immediateInvoke)
                {
                    action.Invoke(notice, notice.isAlive);
                }
            }
        }

        /// <summary>
        /// 红点状态变更通知
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="isAlive"></param>
        public void Nodify(NoticeType noticeType, bool isAlive)
        {
            BaseNotice notice;
            if (noticeDic.TryGetValue(noticeType, out notice))
            {
                if (notice.isAlive != isAlive)
                {
                    notice.Notify(notice, isAlive);

                    if (notice.parent != null)
                    {
                        checkNotice(notice.parent);
                    }
                }
            }
        }

        private void registerNotice(NoticeType childType, NoticeType parentType = NoticeType.NONE)
        {
            BaseNotice child = null;
            if(!noticeDic.TryGetValue(childType, out child))
            {
                if (child == null)
                {
                    child = new BaseNotice(childType);
                    noticeDic[childType] = child;
                }
            }

            if (parentType != NoticeType.NONE)
            {
                BaseNotice parent = null;
                if (!noticeDic.TryGetValue(parentType, out parent))
                {
                    if (parent == null)
                    {
                        parent = new BaseNotice(parentType);
                        noticeDic[parentType] = parent;
                    }
                }
                parent.AddChild(child);
                child.SetParent(parent);
            }
        }

        /// <summary>
        /// 递归检查设置父级notice
        /// </summary>
        /// <param name="notice"></param>
        private void checkNotice(BaseNotice notice)
        {
            if (notice != null)
            {
                bool isAlive = false;
                if (notice.childs != null)
                {
                    for (int i = 0; i < notice.childs.Count; i++)
                    {
                        if(notice.childs[i].isAlive)
                        {
                            isAlive = true;
                            break;
                        }
                    }
                }
                if (isAlive != notice.isAlive)
                {
                    notice.Notify(notice, isAlive);
                }

                if (notice.parent != null)
                {
                    checkNotice(notice.parent);
                }
            }
        }

        public void Reset()
        {
            foreach (var item in noticeDic.Values)
            {
                item.Notify(item, false);
            }
        }
    }
}
