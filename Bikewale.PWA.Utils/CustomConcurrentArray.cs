using System;
using System.Web;

namespace Bikewale.PWA.Utils
{
    public class CustomConcurrentArray
    {
        PwaProcessedHtml[] _array;
        
        static int _limit=100;
        int _currentIndex=-1;
        object _addItemDictionaryLockObj;

        public CustomConcurrentArray()
        {
            _array = new PwaProcessedHtml[_limit];
            _addItemDictionaryLockObj = new object();
        }

        public PwaProcessedHtml Get(string key)
        {
            for (int i=0;i<_limit;i++)
            {
                var item = _array[i];

                if (item !=null && item.HashKey.Equals(key))
                    return item;
            }
            return null;
        }

        public void Add(string key, IHtmlString value,string jsonStr)
        {
            lock(_addItemDictionaryLockObj)
            {
                _currentIndex = (++_currentIndex) % _limit;
                _array[_currentIndex] = new PwaProcessedHtml() { HashKey = key, HtmlString = value, Json = jsonStr };
            }
        }
    }

    public class PwaProcessedHtml
    {
        public string HashKey { get; set; }
        public IHtmlString HtmlString { get; set; }
        public String Json { get; set; }
    }
}
