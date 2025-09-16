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

目前做了一个更新，在分支中。 做了编辑器版本雏形。

2025-9-17
新功能演示：

【【Unity】模块的加载居然可以这么方便】 https://www.bilibili.com/video/BV1eapUzNEBk/?share_source=copy_web&vd_source=ec0f17224c8c59ef66de41f8c63386c8

略微完善了一下编辑面板。现在支持存储完文件后，放入LoadConfig/ReadyToLoad文件路径之后，自动加载模块只要被实例化了，就会把这些信息自动变成button（注意场景里面需要canvas）。然后直接测试你这个加载事件。

另外要注意的是面板的默认路径是loadConfig文件。

