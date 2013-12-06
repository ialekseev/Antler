using System;
using System.Collections;
using System.Collections.Generic;
using SmartElk.Antler.Core.Abstractions.Registration;

namespace SmartElk.Antler.Core.Abstractions
{
    public interface IContainer: IDisposable
    {        
        T Get<T>();        
        T Get<T>(string name);        
        object Get(Type type);        
        object Get(Type type, string name);        
        IList<T> GetAll<T>();        
        IList GetAll(Type type);        
        void Release(object instance);        
        void Put(IBindingSyntax binding);        
        bool Has<T>();        
        bool Has(Type type);        
        bool Has<T>(string name);        
        bool Has(Type type, string name);
    }
}
