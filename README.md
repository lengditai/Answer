# 作答
一、
工程在Move文件夹下，主要实现在MyTween.cs中

二、
大致分为三个部分：建立连接登录、发送消息、接收消息

1.建立连接登录：

建立socket连接：包括超时重试，失败提示等

根据协议进行登录：包括初次登录，平台登录，根据token登录，失败提示等

建立心跳定时器：和后端统一好心跳时间，定时发送心跳包

开启监听循环：对收到的包进行处理

2.发送消息：

发送消息类应包含协议号和一个自增的唯一id，当服务端回复这条消息时，应带回原id

应用层传入发送消息类，根据和后端提前定好的格式（json、protobuffer等）统一转换、加密发送到后端

发送消息会返回协议号和唯一id组成的句柄，应用层可以等待句柄来处理对应的返回消息

3.接收消息

采用广播分发消息，接受消息需要提前按消息类型注册回调

接收到消息后，统一解密，根据相应消息类型，利用反射查找注册回调的参数，进行统一数据转换，应用层直接拿到数据类

大部分消息放在unity线程处理

部分影响性能的消息可以在开启的异步线程池处理，但要注意线程安全问题以及不能处理unity物体，需要开发时特别注意

三、

整个场景数据化，gameobject仅用于显示。

提前建立对象池，视野外的gameobject回收到对象池，即将出现于视野内的gameobject从对象池取出复用

监控帧数，帧数过低时，考虑依据lod圈对远处的粒子系统，动画系统降频、关闭

缓存一些常用数据，保证cpu cache尽量命中

优化渲染，尽量进行动态静态合批，优化shader

animator过多时，可以考虑将动画渲染到贴图上，在shader中进行动画


