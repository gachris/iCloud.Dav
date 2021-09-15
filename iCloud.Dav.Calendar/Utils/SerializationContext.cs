using Ical.Net.General;
using Ical.Net.Interfaces;
using Ical.Net.Interfaces.Components;
using Ical.Net.Interfaces.General;
using Ical.Net.Interfaces.Serialization;
using Ical.Net.Serialization;
using Ical.Net.Serialization.iCalendar.Factory;
using Ical.Net.Serialization.iCalendar.Processors;
using System;
using System.Collections.Generic;

namespace iCloud.Dav.Calendar.Utils
{
    public class SerializationContext : ISerializationContext, Ical.Net.Interfaces.General.IServiceProvider
    {
        private readonly Stack<WeakReference> _mStack = new Stack<WeakReference>();
        private ServiceProvider _mServiceProvider = new ServiceProvider();
        private static SerializationContext _default;

        /// <summary>
        /// Gets the Singleton instance of the SerializationContext class.
        /// </summary>
        public static ISerializationContext Default
        {
            get
            {
                if (SerializationContext._default == null)
                    SerializationContext._default = new SerializationContext();
                return new SerializationContext()
                {
                    _mServiceProvider = SerializationContext._default._mServiceProvider
                };
            }
        }

        public SerializationContext()
        {
            this.SetService(new SerializationSettings());
            this.SetService(new SerializerFactory());
            this.SetService(new ComponentFactory());
            this.SetService(new DataTypeMapper());
            this.SetService(new EncodingStack());
            this.SetService(new EncodingProvider(this));
            this.SetService(new CompositeProcessor<ICalendar>());
            this.SetService(new CompositeProcessor<ICalendarComponent>());
            this.SetService(new CompositeProcessor<ICalendarProperty>());
        }

        public virtual void Push(object item)
        {
            if (item == null)
                return;
            this._mStack.Push(new WeakReference(item));
        }

        public virtual object Pop()
        {
            if (this._mStack.Count > 0)
            {
                WeakReference weakReference = this._mStack.Pop();
                if (weakReference.IsAlive)
                    return weakReference.Target;
            }
            return null;
        }

        public virtual object Peek()
        {
            if (this._mStack.Count > 0)
            {
                WeakReference weakReference = this._mStack.Peek();
                if (weakReference.IsAlive)
                    return weakReference.Target;
            }
            return null;
        }

        public virtual object GetService(Type serviceType)
        {
            return this._mServiceProvider.GetService(serviceType);
        }

        public virtual object GetService(string name)
        {
            return this._mServiceProvider.GetService(name);
        }

        public virtual T GetService<T>()
        {
            return this._mServiceProvider.GetService<T>();
        }

        public virtual T GetService<T>(string name)
        {
            return this._mServiceProvider.GetService<T>(name);
        }

        public virtual void SetService(string name, object obj)
        {
            this._mServiceProvider.SetService(name, obj);
        }

        public virtual void SetService(object obj)
        {
            this._mServiceProvider.SetService(obj);
        }

        public virtual void RemoveService(Type type)
        {
            this._mServiceProvider.RemoveService(type);
        }

        public virtual void RemoveService(string name)
        {
            this._mServiceProvider.RemoveService(name);
        }
    }
}