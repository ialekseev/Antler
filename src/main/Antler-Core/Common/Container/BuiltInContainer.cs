using System;
using System.Collections.Generic;
using System.Linq;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Common.Container
{
    //todo: more powerful built-in container. this is temp solution
    public class BuiltInContainer: IContainer
    {        
        private readonly Dictionary<string, object> _maps = new Dictionary<string, object>();
                        
        public T Get<T>()
        {
            return (T)GetInternal(ComposeKey<T>());
        }

        public T Get<T>(string name)
        {
            Requires.NotNullOrEmpty(name, "name");
            
            return (T)GetInternal(ComposeKey<T>(name));
        }

        public object Get(Type type)
        {
            Requires.NotNull(type, "type");
            
            return GetInternal(ComposeKey(type));
        }

        public object Get(Type type, string name)
        {
            Requires.NotNull(type, "type");
            Requires.NotNull(name, "name");

            return GetInternal(ComposeKey(type, name));
        }
        
        public void Release(object instance)
        {
            Requires.NotNull(instance, "instance");
                                    
            var found = _maps.FirstOrDefault(t => t.Value.Equals(instance));
            Assumes.True(!found.Equals(default(KeyValuePair<string, object>)));
            
            _maps.Remove(found.Key);
        }

        public void Put(IBindingSyntax binding)
        {
            Requires.NotNull(binding, "binding");
            Requires.NotNull(binding.Binding, "binding.Binding");

            var singleBinding = binding.Binding as SingleBinding;            
            Assumes.True(singleBinding != null);
            
            var instanceResolver = singleBinding.Resolver as InstanceResolver;
            Assumes.True(instanceResolver != null);
            Assumes.True(instanceResolver.Instance != null);

            _maps.Add(ComposeKey(singleBinding.Service, singleBinding.Name), instanceResolver.Instance);
        }

        public bool Has<T>()
        {
            return HasInternal(ComposeKey<T>());
        }

        public bool Has(Type type)
        {
            return HasInternal(ComposeKey(type));
        }

        public bool Has<T>(string name)
        {
            return HasInternal(ComposeKey<T>(name));
        }

        public bool Has(Type type, string name)
        {
            return HasInternal(ComposeKey(type, name));
        }
        
        public void Dispose()
        {
            _maps.Clear();            
        }

        public IList<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public System.Collections.IList GetAll(Type type)
        {
            throw new NotImplementedException();
        }

        #region Internal

        private object GetInternal(string key)
        {
            if (!_maps.ContainsKey(key))
                return null;
            return _maps[key];
        }
        
        private bool HasInternal(string key)
        {
            return _maps.Any(t => t.Key.Equals(key));
        }

        private static string ComposeKey(Type type, string name = null)
        {
            Requires.NotNull(type, "type");

            if (string.IsNullOrEmpty(name))
                return type.Name;
            return type.Name + name;
        }

        private static string ComposeKey<T>(string name = null)
        {
            return ComposeKey(typeof(T), name);
        }

        #endregion
    }
}
