using Framework;

namespace LoadFramework {
    /// <summary>
    /// 广播加载信息用的事件
    /// </summary>
    public class LoadingEvent : Event<LoadingEvent, ILoadEventInfo> { }
    /// <summary>
    /// 加载器开始加载的时候广播这个事件（这个事件可以用来控制加载UI的显示）
    /// </summary>
    public class StartLoadingEvent : Event<StartLoadingEvent> { }
    /// <summary>
    /// 加载器全部加载完成之后广播这个事件（这个事件可以用来控制加载UI的关闭）
    /// </summary>
    public class LoadingCompletedEvent : Event<LoadingCompletedEvent> { }
    /// <summary>
    /// 用来播报当前加载进度的事件，根据需要可以订阅这个事件来更新加载进度UI
    /// </summary>
    public class CurrentLoadingStepEvent : Event<CurrentLoadingStepEvent, string> { }
}