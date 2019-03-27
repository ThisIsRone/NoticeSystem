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

        internal Dictionary<NoticeType, BaseNotice> NoticeDic
        {
            get
            {
                return noticeDic;
            }
        }

        private Dictionary<NoticeType, BaseNotice> noticeDic = new Dictionary<NoticeType, BaseNotice>();

        public void InitSystem()
        {
            RegisterNotice(NoticeType.Main_Friend);
            RegisterNotice(NoticeType.Friend_MyFriend, NoticeType.Main_Friend);
            RegisterNotice(NoticeType.Root_MyFriend_Friend, NoticeType.Friend_MyFriend);
            //RegisterNotice(NoticeType.Root_Friend_Item, NoticeType.Root_MyFriend_Friend);

            RegisterNotice(NoticeType.Root_MyFriend_RecentMan, NoticeType.Friend_MyFriend);
            //RegisterNotice(NoticeType.Root_RecentMan_item, NoticeType.Root_MyFriend_RecentMan);

            RegisterNotice(NoticeType.Root_Friend_Apply, NoticeType.Main_Friend);
        }

        /// <summary>
        /// 注册Notice回调
        /// </summary>
        /// <param name="noticeType">Notice类型</param>
        /// <param name="root">Root节点</param>
        /// <param name="action">触发回调</param>
        /// <param name="immediateInvoke">触发当前新增的触发回调</param>
        public void RegisterCallBack(NoticeType noticeType, Transform root, Action<BaseNotice> action,bool immediateInvoke = true)
        {
            BaseNotice notice;
            if (noticeDic.TryGetValue(noticeType, out notice))
            {
                notice.RegisterCallBack(root,action);

                if (immediateInvoke)
                {
                    action.Invoke(notice);
                }
            }
        }

        /// <summary>
        /// 移除目标回调
        /// </summary>
        /// <param name="noticeType">Notice类型</param>
        /// <param name="root">Root节点</param>
        /// <param name="action">触发回调</param>
        /// <param name="immediateInvoke">触发当前移除的触发回调</param>
        public void UnRegiNotifyCallBack(NoticeType noticeType, Transform root, Action<BaseNotice> action, bool immediateInvoke = false)
        {
            BaseNotice notice;
            if (noticeDic.TryGetValue(noticeType, out notice))
            {
                notice.UnregisterCallBack(root,action);
                if (immediateInvoke)
                {
                    action.Invoke(notice);
                }
            }
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="isAlive"></param>
        public void Nodify(NoticeType noticeType, bool isAlive)
        {
            //Debug.LogErrorFormat("Nodify:: NoticeType = {0} isAlive = {1}", noticeType, isAlive);
            BaseNotice notice;
            if (noticeDic.TryGetValue(noticeType, out notice))
            {
                notice.Notify(notice, isAlive);

                if (notice.parent != null)
                {
                    checkNotice(notice.parent);
                }
            }
            else
            {
                Debug.LogError("【Nodify】Not exist target NoticeType : " + noticeType.ToString());
            }
        }

        public void RegisterNotice(NoticeType childType, NoticeType parentType = NoticeType.NONE)
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

        public void UnregisterNotice(NoticeType childType, NoticeType parentType = NoticeType.NONE)
        {
            BaseNotice child = null;
            if (!noticeDic.TryGetValue(childType, out child))
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
                parent.RemoveChild(child);

                if (child.parent == parent)
                {
                    child.SetParent(null);
                }
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
                //Debug.LogErrorFormat("checkNotice:: NoticeType = {0} isAlive = {1}", notice.noticeType, notice.isAlive);
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
                    //Debug.LogErrorFormat("checkNotice:: NoticeType = {0} isAlive = {1} Notify", notice.noticeType, notice.isAlive);
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
