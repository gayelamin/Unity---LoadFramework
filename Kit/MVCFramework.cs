using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Game;

namespace Framework
{
    #region Architecture
    public interface IArchitecture
    {
        void RegisterSystem<T>(T system) where T : ISystem;

        void RegisterModel<T>(T model) where T : IModel;

        void RegisterUtility<T>(T utility) where T : IUtility;

        T GetSystem<T>() where T : class, ISystem;

        T GetModel<T>() where T : class, IModel;

        T GetUtility<T>() where T : class, IUtility;

        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;

        TResult SendQuery<TResult>(IQuery<TResult> query);

        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        /// <summary>
        /// 是否初始化完成 
        /// </summary>
        private bool inited = false;

        private List<ISystem> systems = new List<ISystem>();

        private List<IModel> models = new List<IModel>();

        public static Action<T> OnRegisterPatch = architecture => { };

        private static T architecture;

        public static IArchitecture Interface
        {
            get
            {
                if (architecture == null)
                {
                    MakeSureArchitecture();
                }

                return architecture;
            }
        }


        static void MakeSureArchitecture()
        {
            if (architecture == null)
            {
                architecture = new T();
                architecture.Init();

                OnRegisterPatch?.Invoke(architecture);

                foreach (var architectureModel in architecture.models)
                {
                    architectureModel.Init();
                }

                architecture.models.Clear();

                foreach (var architectureSystem in architecture.systems)
                {
                    architectureSystem.Init();
                }

                architecture.systems.Clear();

                architecture.inited = true;
            }
        }

        protected abstract void Init();

        private IOCContainer container = new IOCContainer();

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            container.Register<TSystem>(system);

            if (!inited)
            {
                systems.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            container.Register<TModel>(model);

            if (!inited)
            {
                models.Add(model);
            }
            else
            {
                model.Init();
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            container.Register<TUtility>(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return container.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return container.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return container.Get<TUtility>();
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<TEvent>() where TEvent : new()
        {
            mTypeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            mTypeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }
    #endregion

    #region Controller

    public interface IController : IGetArchitecture, ISendCommand, IGetSystem, IGetModel, IRegisterEvent, ISendQuery
    {

    }

    public abstract class AbstractController : IController
    {
        IArchitecture IGetArchitecture.GetArchitecture()
        {
            return SystemCenter.Interface;
        }

    }
    #endregion

    #region System
    public interface ISystem : IGetArchitecture, ISetArchitecture, IGetModel, IGetUtility, IRegisterEvent, ISendEvent, IGetSystem
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture architecture;
        IArchitecture IGetArchitecture.GetArchitecture()
        {
            return architecture;
        }

        void ISetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }

        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }


    #endregion

    #region Model
    public interface IModel : IGetArchitecture, ISetArchitecture, IGetUtility, ISendEvent
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture architecturel;

        IArchitecture IGetArchitecture.GetArchitecture()
        {
            return architecturel;
        }

        void ISetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            architecturel = architecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }


    #endregion

    #region Utility
    public interface IUtility
    {
    }
    #endregion

    #region Command
    public interface ICommand : IGetArchitecture, ISetArchitecture, IGetSystem, IGetModel, IGetUtility, ISendEvent, ISendCommand, ISendQuery
    {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture architecture;
        IArchitecture IGetArchitecture.GetArchitecture()
        {
            return architecture;
        }

        void ISetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }


    #endregion

    #region Query

    public interface IQuery<TResult> : IGetArchitecture, ISetArchitecture, IGetModel, IGetSystem, ISendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }

        protected abstract T OnDo();


        private IArchitecture architecture;

        public IArchitecture GetArchitecture()
        {
            return architecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            this.architecture = architecture;
        }
    }

    #endregion

    #region Rule
    public interface IGetArchitecture
    {
        IArchitecture GetArchitecture();
    }

    public interface ISetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }

    public interface IGetModel : IGetArchitecture
    {
    }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this IGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }

    public interface IGetSystem : IGetArchitecture
    {

    }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this IGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }

    public interface IGetUtility : IGetArchitecture
    {

    }

    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this IGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }

    public interface IRegisterEvent : IGetArchitecture
    {
    }

    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this IRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }

        public static void UnRegisterEvent<T>(this IRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }

    public interface ISendCommand : IGetArchitecture
    {

    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ISendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ISendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }

    public interface ISendEvent : IGetArchitecture
    {
    }

    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ISendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        public static void SendEvent<T>(this ISendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }

    public interface ISendQuery : IGetArchitecture
    {

    }

    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ISendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
    #endregion

    #region TypeEventSystem

    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }

    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            TypeEventSystem.UnRegister<T>(OnEvent);

            TypeEventSystem = null;

            OnEvent = null;
        }
    }

    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    public class TypeEventSystem : ITypeEventSystem
    {
        public interface IRegistrations
        {

        }

        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        private Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();


        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {

            }
            else
            {
                registrations = new Registrations<T>();
                mEventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;

            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }

    #endregion

    #region IOC
    public class IOCContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }


    #endregion

    #region BindableProperty
    public class BindableProperty<T>
    {

        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }

        private T mValue = default(T);

        public T Value
        {
            get => mValue;
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                mOnValueChanged?.Invoke(value);
            }
        }

        private Action<T> mOnValueChanged = (v) => { };

        public IUnRegister Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }

        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void UnRegister(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }
    }

    public class BindablePropertyUnRegister<T> : IUnRegister
    {

        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);

            BindableProperty = null;
            OnValueChanged = null;
        }
    }

    public class BindablePropertyTwoArray<T>
    {
        private T[,] _value;
        public T[,] Value
        {
            get => _value;
            set
            {
                if (value == null && _value == null) return;
                if (value != null && value == _value) return;

                _value = value;
                mOnValueChanged?.Invoke(value);
            }
        }

        private Action<T[,]> mOnValueChanged = (v) => { };

        public BindablePropertyTwoArray(T[,] defaultValue)
        {
            _value = defaultValue;
        }
        public BindablePropertyTwoArray()
        {
            _value = new T[1, 1];
            _value[0, 0] = default(T);
        }


        public IUnRegister Register(Action<T[,]> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyTwoArrayUnRegister<T>()
            {
                BindablePropertyTwoArray = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnRegister RegisterWithInitValue(Action<T[,]> onValueChanged)
        {
            onValueChanged(_value);
            return Register(onValueChanged);
        }

        public static implicit operator T[,](BindablePropertyTwoArray<T> property)
        {
            return property.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void UnRegister(Action<T[,]> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }

    }

    public class BindablePropertyTwoArrayUnRegister<T> : IUnRegister
    {

        public BindablePropertyTwoArray<T> BindablePropertyTwoArray { get; set; }

        public Action<T[,]> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindablePropertyTwoArray.UnRegister(OnValueChanged);

            BindablePropertyTwoArray = null;
            OnValueChanged = null;
        }
    }
    #endregion

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

    abstract class Observer
    {
        public abstract void OnNotify();
    }

    abstract class Subject
    {
        private List<Observer> observers = new List<Observer>();

        public void AddObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            observers.ForEach(observer => observer.OnNotify());
        }

    }
}