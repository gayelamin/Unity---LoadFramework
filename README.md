QQ群：1060377215

项目还在早期阶段，快来加入QQ群反馈问题鸭。

框架设计思路：

【【unity】一个可高度自定义的加载框架设计思路（含代码）】 https://www.bilibili.com/video/BV1mtHCzzEuY/?share_source=copy_web&vd_source=ec0f17224c8c59ef66de41f8c63386c8

框架使用指南：

【【Unity】一个可自定义加载框架的使用指南（已开源）】 https://www.bilibili.com/video/BV1cdpcz5Eut/?share_source=copy_web&vd_source=ec0f17224c8c59ef66de41f8c63386c8

脚本运行方法：
场景中，创建一个物体，将SystemManager放上去，其他都不需要用。

实际上的加载在MonoTest中，这是为了测试框架，以及演示框架用的脚本。

模块在Game文件夹中，里面有5个模块，用于展示不同场景 模块1-3用于展示加载顺序。逻辑模块用于展示非Mono脚本的加载。场景加载就是场景加载。

2025-9-16

目前做了一个更新，在分支中。

准备去支持使用编辑器创建加载事件信息文件，并且存储到SO文件中。然后提供了一个文件可以把So文件直接读取成CommonLoadEventInfo对象，然后调用new LoadingCommand.execute就可以直接执行加载事件。

而且目前提供了一个自动加载模块，作为实验性功能。这个模块的作用是，在初始化的时候会自动读取Asset/LoadConfig/ReadyToLoad里面的SO文件，实例化成几个按钮，直接去进行加载。肥肠的方便！！！
