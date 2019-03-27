# NoticeSystem
这是一个轻量级的Notice(红点Tips)的管理方案，已经在商业（MMORPG）项目中得到应用。
我使用了递归函数去遍历设置Notice节点信息，因此也导致后来的一个比较难处理的问题，即当递归深度超过5的时候，在部分机型(低端机)上会引起闪退。
即便如此，我依旧认为这是一个非常适合管理和拓展的Notice管理方案。

2019.03.27 
修改内容
1.注册的变更回调事件 由单纯的Action管理 更改为 依赖于Mono的OnDestroy的管理方式
2.完善子父级动态更新的逻辑
3.回调的注册由ActionA += ActionB更改为 List<Action>.Add(ActionA) 的管理方式

