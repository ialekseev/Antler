using System;
using System.Collections.Generic;
using System.Linq;
using SmartElk.Antler.Core.Abstractions;

namespace SmartElk.Antler.Core.Container
{
    //todo: implement + write specs + use as default container in AntlerConfigurator
    public class DefaultContainer: IContainer
    {        
        private readonly Dictionary<string, object> _maps = new Dictionary<string, object>();

        public T Get<T>()
        {
            return (T)_maps[typeof(T).ToString()];
        }

        public T Get<T>(string name)
        {
            return (T)_maps[typeof(T) + name];
        }

        public object Get(Type type)
        {
            return _maps[type.ToString()];
        }

        public object Get(Type type, string name)
        {
            return _maps[type + name];
        }

        public IList<T> GetAll<T>()
        {
            return _maps.Where(t => t.Key.Equals(typeof (T).ToString())).Select(t => (T) t.Value).ToList();
        }

        public System.Collections.IList GetAll(Type type)
        {
            return _maps.Where(t => t.Key.Equals(type.ToString())).Select(t => t.Value).ToList();
        }

        public void Release(object instance)
        {
            throw new System.NotImplementedException();
        }

        public void Put(Abstractions.Registration.IBindingSyntax binding)
        {
            throw new System.NotImplementedException();                        
        }

        public bool Has<T>()
        {
            throw new System.NotImplementedException();
        }

        public bool Has(System.Type type)
        {
            throw new System.NotImplementedException();
        }

        public bool Has<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool Has(System.Type type, string name)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
