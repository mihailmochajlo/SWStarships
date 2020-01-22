using System;

namespace App
{
    public interface IDataSource<T>
    {
        public T GetData ();
    }
}