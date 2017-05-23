using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.PWA.Utils
{
    public class CustomConcurrentArray
    {
        PwaProcessedHtml[] _array;
        static int _limit = BWConfiguration.Instance.PwaLocalCahceLimit;
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


    public class CustomConcurrentDictionary
    {
        Dictionary<string,PwaProcessedHtml> _dict;
        static int _limit = BWConfiguration.Instance.PwaLocalCahceLimit;
        Queue<string> _queue;
        int _currentCount = 0;
        object _addItemDictionaryLockObj;

        public CustomConcurrentDictionary()
        {
            _dict = new Dictionary<string, PwaProcessedHtml>();
            _queue = new Queue<string>();
            _addItemDictionaryLockObj = new object();
        }  

        public PwaProcessedHtml Get(string key)
        {
            PwaProcessedHtml outData;
            if (_dict.TryGetValue(key,out outData))
            {
                return outData;
            }
            return null;            
        }

        public void Add(string key, IHtmlString value, string jsonStr)
        {
            lock (_addItemDictionaryLockObj)
            {
                _currentCount++;
                if (_currentCount > _limit)
                //remove the first added entry from dictionary
                {
                    _currentCount--;
                    _dict.Remove(_queue.Dequeue());// it will never happen that the queue will be empty when _currentCount=limit
                }
                _queue.Enqueue(key);
                _dict[key] = new PwaProcessedHtml() { HashKey = key, HtmlString = value, Json = jsonStr };
            }
        }

    }
    

}
