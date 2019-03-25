/**********************************************/
/*       红点枚举类型                         */
/*       Date:2019.03.25                      */
/*       功能类枚举 +1000                     */
 /*      页签类枚举 +100                      */
 /*      基础点枚举 前缀 Root_                */
/**********************************************/
namespace NoticeSystem
{
    public enum NoticeType
    {
        NONE = 0,
        Main_Friend = 1000,//主界面好友红点

        Friend_MyFriend = 1100, //好友页签红点

        Root_MyFriend_Friend = 1110,//好友列表
        //Root_Friend_Item = 1111,     //好友联系人

        Root_MyFriend_RecentMan = 1120,// 最近列表
        //Root_RecentMan_item = 1121,// 最近联系人


        Root_Friend_Apply = 1200, //申请页签红点
    }
}