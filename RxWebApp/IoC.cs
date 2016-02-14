using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RxWebApp
{
    public enum IocScope
    {
        Shared,
        PerRequest
    }

    public sealed class IoC : IDisposable
    {
        private readonly Dictionary<Type, Tuple<Type, IocScope>> _dependencyMap;
        private readonly Dictionary<Type, Tuple<object, IocScope>> _factoryMap;
        private readonly ConcurrentDictionary<Type, Object> _instanceCache;

        public IoC()
        {
            _dependencyMap = new Dictionary<Type, Tuple<Type, IocScope>>();
            _factoryMap = new Dictionary<Type, Tuple<object, IocScope>>();
            _instanceCache = new ConcurrentDictionary<Type, object>();
        }

        ~IoC()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // managed types
                var disposableTypes = _instanceCache.Where(x => x.Value is IDisposable).Select(x => x.Value).Cast<IDisposable>().ToList();
                foreach (var disposable in disposableTypes)
                {
                    disposable.Dispose();
                }
            }
            // unmanaged types
            _instanceCache.Clear();
            _factoryMap.Clear();
            _dependencyMap.Clear();
        }

        private static IoC instance;

        public static IoC Instance => instance ?? (instance = new IoC());

        public T Resolve<T>(Type t)
        {
            return (T)Resolve(t);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public void Register<TFrom, TTo>(IocScope iocScope = IocScope.Shared)
        {
            var type = typeof(TFrom);
            Tuple<Type, IocScope> existing;
            if (_dependencyMap.TryGetValue(type, out existing))
            {
                _dependencyMap.Remove(type);
            }
            _dependencyMap.Add(typeof(TFrom), Tuple.Create(typeof(TTo), iocScope));
        }

        public void Register<TFrom>(Func<TFrom> factory, IocScope iocScope = IocScope.Shared)
        {
            Type type = typeof(TFrom);
            Tuple<object, IocScope> existing;
            if (_factoryMap.TryGetValue(type, out existing))
            {
                _factoryMap.Remove(type);
            }
            _factoryMap.Add(typeof(TFrom), Tuple.Create((object)factory, iocScope));
        }

        public void RegisterInstance<TFrom>(TFrom instance)
        {
            Type type = typeof(TFrom);
            if (_instanceCache.ContainsKey(type))
            {
                object dummy;
                _instanceCache.TryRemove(type, out dummy);
            }
            _instanceCache[type] = instance;
            if (_dependencyMap.ContainsKey(type))
            {
                _dependencyMap.Remove(type);
            }
            _dependencyMap.Add(type, Tuple.Create(typeof(TFrom), IocScope.Shared));
        }

        public void UnregisterInstance<TFrom>(TFrom instance)
        {
            var key = typeof(TFrom);
            object existing;
            if (_instanceCache.TryGetValue(key, out existing) && ReferenceEquals(existing, instance))
            {
                object dummy;
                _instanceCache.TryRemove(key, out dummy);
                _dependencyMap.Remove(key);
            }
        }

        private object Resolve(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            Tuple<Type, IocScope> resolvedType = LookUpDependency(type);

            object instance;
            if (resolvedType == null)
            {
                instance = ResolveFactory(type);
                if (instance == null)
                {
                    string typeName = type.FullName;
                    throw new ArgumentOutOfRangeException("type", type, typeName + " could not be resolved by " + GetType().Name);
                }
            }
            else if (resolvedType.Item2 == IocScope.PerRequest || !_instanceCache.TryGetValue(type, out instance))
            {
                // Check instance cache            
                object createInstance;
                ConstructorInfo constructor = resolvedType.Item1.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => !x.IsStatic);
                if (constructor == null)
                {
                    createInstance = Activator.CreateInstance(resolvedType.Item1);
                }
                else
                {
                    ParameterInfo[] parameters = constructor.GetParameters();

                    if (!parameters.Any())
                    {
                        createInstance = Activator.CreateInstance(resolvedType.Item1);
                    }
                    else
                    {
                        createInstance = constructor.Invoke(ResolveParameters(parameters).ToArray());
                    }
                }

                if (resolvedType.Item2 == IocScope.Shared)
                {
                    instance = _instanceCache.GetOrAdd(type, createInstance);
                }
                else
                {
                    instance = createInstance;
                }
            }

            return instance;
        }

        private object ResolveFactory(Type type)
        {
            object instance = null;
            Tuple<object, IocScope> factory;
            if (_factoryMap.TryGetValue(type, out factory))
            {
                Func<object> typeFactory = factory.Item1 as Func<object>;
                if (typeFactory != null)
                {
                    if (factory.Item2 == IocScope.PerRequest || !_instanceCache.TryGetValue(type, out instance))
                    {
                        instance = typeFactory();

                        if (factory.Item2 == IocScope.Shared)
                        {
                            instance = _instanceCache.GetOrAdd(type, instance);
                        }
                    }
                }
            }
            return instance;
        }

        private Tuple<Type, IocScope> LookUpDependency(Type type)
        {
            Tuple<Type, IocScope> result;
            _dependencyMap.TryGetValue(type, out result);
            return result;
        }

        private IEnumerable<object> ResolveParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters.Select(p => Resolve(p.ParameterType));
        }

        public IEnumerable<Type> FindMatchingTypes<T>()
        {
            Type matchType = typeof(T);
            foreach (Type tp in _dependencyMap.Keys)
            {
                if (matchType.IsAssignableFrom(tp))
                {
                    yield return tp;
                }
            }
        }

        public IEnumerable<T> ResolveMatchingInstances<T>()
        {
            Type matchType = typeof(T);
            foreach (Type tp in _factoryMap.Keys)
            {
                if (matchType.IsAssignableFrom(tp))
                {
                    yield return (T)Resolve(tp);
                }
            }
        }
    }
}