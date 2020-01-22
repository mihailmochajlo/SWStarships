using System;

namespace App
{
    public interface IParser<T>
    {
        public T Parse ();
        public string GetPropertyAsString (string propertyName);
    }
}