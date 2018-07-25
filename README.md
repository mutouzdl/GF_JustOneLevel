![avatar](http://www.benmutou.com/wp-content/uploads/2018/07/Snipaste_2018-07-25_17-24-29.png)


# GF_JustOneLevel（只有一关）
基于[Game Framework](https://github.com/EllanJiang/GameFramework)框架的小游戏——《只有一关》

没错，内容只有一关，游戏名字也叫做只有一关。
其实这是我学习Unity3D后做的第一个游戏，现在用GameFramework重制一次。不过，由于时间关系，元素少了一些（比如没有了宠物功能）。

打个小广告，这是曾经发布《只有一关》的博客：http://www.benmutou.com/archives/1967 ，目前游戏已经被各种下架（版号原因，大家懂的）。
没想到距今已经三年了，而我的Unity3D水平依旧没有长进，十分对得起业余的称号。

# 版本
GameFramework版本：3.1.3

Unity3D版本：2018.1

# 主要玩法
玩法很简单，就是玩家在一块空地上，然后会有怪物不断产生，打死它们就可以了。
其中有三个魔力泉水，各有功效，需要合理利用，否则是无法生存下去的。

游戏并没有胜利机制，正常来说，怪物会越来越强，直到玩家无法战胜。

玩家每杀死一只怪物就会吸收它的部分属性（增加三围），所以想要生存下去的话，必须尽可能地杀死更多怪物。

目前只有五种笨笨的怪物，以及三个英雄（骑士、神射手、武斗家）。

操作: 触屏虚拟摇杆

# 主要技术
使用了GameFramework的以下功能：
 - 流程（Procedure）
 - 缓存（Object Pool）
 - 实体（Entity）
 - 资源（Resource），单机，非动态加载
 - 场景（Scene）
 - 界面（UI）
 - 本地化（Localization），英文版语言的翻译我是随便翻的，没有仔细检查
 - 数据表（DataTable）
 - 事件（Event）
 - 状态机（FSM）

使用了以下免费的Unity3D商店资源
 - [Pocket RPG Weapon Trails](https://assetstore.unity.com/packages/tools/particles-effects/pocket-rpg-weapon-trails-2458)
 - [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
 - [Ball Pack](https://assetstore.unity.com/packages/3d/props/ball-pack-446)
 - [Unity Samples: UI](https://assetstore.unity.com/packages/essentials/unity-samples-ui-25468)
 等，有点多，就不继续列出来了。
 
 # 主要功能
  - 场景切换
  - 从配置文件读取并加载实体
  - 状态机
  - 怪物生成器
  - 武器
  - 攻击和技能
  - 魔力泉
  - 移动控制器
  - 其它：对话确认框、死亡后花金币续命、杀死怪物后吸收怪物部分属性、怪物在短时间内受到三次攻击后会往后弹一段距离

 （详细描述请看[GF_JustOneLevel食用指南002：主要功能](http://www.benmutou.com/archives/2662)）

# 特别说明
由于是为了学习GameFramework框架，所以很多套路是参照StarForce项目来的，甚至有些类都是直接拿来改改就用。
以至于，可能看起来有点别扭（毕竟游戏类型不一样）。

另外，配置文件我是用CSV格式的（逗号分隔符），并不是框架作者使用的Tab分隔符。

UI很丑，嗯。

# 食用指南
[GF_JustOneLevel食用指南001：下载、运行](http://www.benmutou.com/archives/2660)

[GF_JustOneLevel食用指南002：主要功能](http://www.benmutou.com/archives/2662)

# 未完成的事情
目前只测试了PC版和Android版，iOS没有验证，因为木头个人是安卓粉，同时也因为精力不足，所以一直以来只关注Android，iOS没有环境验证。
期待大家能帮我验证打包iOS后是否有问题，欢迎Pull Request！

# License
Apache License 2.0
