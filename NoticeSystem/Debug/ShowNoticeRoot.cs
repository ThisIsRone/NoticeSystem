#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace NoticeSystem
{
    public class ShowNoticeRoot : MonoBehaviour
    {
        private Dictionary<NoticeType, BaseNotice> noticeDic = null;

        private void Awake()
        {
            noticeDic = NoticeSystemManager.Instance.NoticeDic;
        }

        public void RefreshRoot()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
                i--;
            }

            List<BaseNotice> rootNotice = new List<BaseNotice>();
            foreach (var item in noticeDic.Values)
            {
                if (item.parent == null)
                {
                    rootNotice.Add(item);
                }
            }
            for (int i = 0; i < rootNotice.Count; i++)
            {
                setChild(rootNotice[i], "");
            }
        }

        private void setChild(BaseNotice notice,string path)
        {
            GameObject obj = new GameObject(notice.noticeType.ToString());
            obj.AddComponent<WatchMonoAction>().noticeActions = notice.notifyCallBacks;
            Transform parent = transform.Find(path);
            if (parent == null)
            {
                parent = transform;
            }

            obj.transform.SetParent(parent);

            path = string.IsNullOrEmpty(path) ? obj.name : path + "/" + obj.name;
            if (notice.childs != null && notice.childs.Count > 0)
            {
                for (int i = 0; i < notice.childs.Count; i++)
                {
                    setChild(notice.childs[i], path);
                }
            }
        }
    }

    [CustomEditor(typeof(ShowNoticeRoot))]
    public class NoticeRootEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("刷新"))
            {
                ((ShowNoticeRoot)target).RefreshRoot();
            }
        }
    }

}
#endif