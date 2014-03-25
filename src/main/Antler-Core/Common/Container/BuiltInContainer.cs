using System;
using System.Collections.Generic;
using System.Linq;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Common.Container
{
    //todo: write specs + use as default container in AntlerConfigurator
    public class BuiltInContainer: IContainer
    {        
        private readonly Dictionary<string, object> _maps = new Dictionary<string, object>();
                        
        public T Get<T>()
        {
            return (T)Get(ComposeKey<T>());
        }

        public T Get<T>(string name)
        {
            Requires.NotNullOrEmpty(name, "name");
            
            return (T)Get(ComposeKey<T>(name));
        }

        public object Get(Type type)
        {
            Requires.NotNull(type, "type");
            
            return Get(ComposeKey(type));
        }

        public object Get(Type type, string name)
        {
            Requires.NotNull(type, "type");
            Requires.NotNull(name, "name");

            return Get(ComposeKey(type, name));
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
            Requires.NotNull(binding.Binding, "binding");

            var singleBinding = binding.Binding as SingleBinding;            
            Assumes.True(singleBinding != null);
            
            var instanceResolver = singleBinding.Resolver as InstanceResolver;
            Assumes.True(instanceResolver != null);
            Assumes.True(instanceResolver.Instance != null);

            _maps.Add(ComposeKey(singleBinding.Service, singleBinding.Name), instanceResolver.Instance);
        }

        public bool Has<T>()
        {
            return _maps.Any(t => t.Key.Equals(ComposeKey<T>()));
        }

        public bool Has(Type type)
        {
            return _maps.Any(t => t.Key.Equals(ComposeKey(type)));
        }

        public bool Has<T>(string name)
        {
            return _maps.Any(t => t.Key.Equals(ComposeKey<T>(name)));
        }

        public bool Has(Type type, string name)
        {
            return _maps.Any(t => t.Key.Equals(ComposeKey(type, name)));
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

        private object Get(string key)
        {
            Assumes.True(_maps.ContainsKey(key));
            return _maps[key];
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
