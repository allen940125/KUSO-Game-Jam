using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datamanager 
{
    public interface IDataBase
    {
        public object[] DataArray { get; set; }
        public Type ThisDataType { get; set; }
        public T GetArrayObj<T>(int i) where T : class
        {
            var obj = DataArray[i];
            return obj as T;
        }
    }
}

