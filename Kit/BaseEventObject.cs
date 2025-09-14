
using System;

public class Event<T> where T : Event<T>
{
    private static Action onAction;

    public static void Register(Action onEvent)
    {
        onAction += onEvent;
    }

    public static void UnRegister(Action onEvent)
    {
        onAction -= onEvent;
    }

    public static void Trigger()
    {
        onAction?.Invoke();
    }
}

public class Event<T, TD> where T : Event<T, TD>
{
    private static Action<TD> onAction;
    public static void Register(Action<TD> onEvent)
    {
        onAction += onEvent;
    }

    public static void UnRegister(Action<TD> onEvent)
    {
        onAction -= onEvent;
    }
    public static void Trigger(TD data)
    {
        onAction?.Invoke(data);
    }
}