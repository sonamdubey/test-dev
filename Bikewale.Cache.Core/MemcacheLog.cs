using log4net;
using System;

namespace Bikewale.Cache.Core
{
    class MemcacheLog : Enyim.Caching.ILog
    {
         
        private ILog log;

        bool Enyim.Caching.ILog.IsDebugEnabled
        {
            get
            {
                return this.log.IsDebugEnabled;
            }
        }

        bool Enyim.Caching.ILog.IsErrorEnabled
        {
            get
            {
                return this.log.IsErrorEnabled;
            }
        }

        bool Enyim.Caching.ILog.IsFatalEnabled
        {
            get
            {
                return this.log.IsFatalEnabled;
            }
        }

        bool Enyim.Caching.ILog.IsInfoEnabled
        {
            get
            {
                return this.log.IsInfoEnabled;
            }
        }

        bool Enyim.Caching.ILog.IsWarnEnabled
        {
            get
            {
                return this.log.IsWarnEnabled;
            }
        }

        public MemcacheLog(ILog log)
        {
            this.log = log;
        }

        void Enyim.Caching.ILog.Debug(object message)
        {
            this.log.Debug(message);
        }

        void Enyim.Caching.ILog.Debug(object message, Exception exception)
        {
            this.log.Debug(message, exception);
        }

        void Enyim.Caching.ILog.DebugFormat(string format, object arg0)
        {
            this.log.DebugFormat(format, arg0);
        }

        void Enyim.Caching.ILog.DebugFormat(string format, object arg0, object arg1)
        {
            this.log.DebugFormat(format, arg0, arg1);
        }

        void Enyim.Caching.ILog.DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.DebugFormat(format, arg0, arg1, arg2);
        }

        void Enyim.Caching.ILog.DebugFormat(string format, params object[] args)
        {
            this.log.DebugFormat(format, args);
        }

        void Enyim.Caching.ILog.DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.DebugFormat(provider, format, args);
        }

        void Enyim.Caching.ILog.Error(object message)
        {
            this.log.Error(message);
            //ErrorClass.LogError(new ArgumentNullException(), message.ToString());
        }

        void Enyim.Caching.ILog.Error(object message, Exception exception)
        {
            this.log.Error(message, exception);
        }

        void Enyim.Caching.ILog.ErrorFormat(string format, object arg0)
        {
            this.log.ErrorFormat(format, arg0);
        }

        void Enyim.Caching.ILog.ErrorFormat(string format, object arg0, object arg1)
        {
            this.log.ErrorFormat(format, arg0, arg1);
        }

        void Enyim.Caching.ILog.ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.ErrorFormat(format, arg0, arg1, arg2);
        }

        void Enyim.Caching.ILog.ErrorFormat(string format, params object[] args)
        {
            this.log.ErrorFormat(format, args);
        }

        void Enyim.Caching.ILog.ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.ErrorFormat(provider, format, args);
        }

        void Enyim.Caching.ILog.Fatal(object message)
        {
            this.log.Fatal(message);
        }

        void Enyim.Caching.ILog.Fatal(object message, Exception exception)
        {
            this.log.Fatal(message, exception);
        }

        void Enyim.Caching.ILog.FatalFormat(string format, object arg0)
        {
            this.log.FatalFormat(format, arg0);
        }

        void Enyim.Caching.ILog.FatalFormat(string format, object arg0, object arg1)
        {
            this.log.FatalFormat(format, arg0, arg1);
        }

        void Enyim.Caching.ILog.FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.FatalFormat(format, arg0, arg1, arg2);
        }

        void Enyim.Caching.ILog.FatalFormat(string format, params object[] args)
        {
            this.log.FatalFormat(format, args);
        }

        void Enyim.Caching.ILog.FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.FatalFormat(provider, format, args);
        }

        void Enyim.Caching.ILog.Info(object message)
        {
            this.log.Info(message);
        }

        void Enyim.Caching.ILog.Info(object message, Exception exception)
        {
            this.log.Info(message, exception);
        }

        void Enyim.Caching.ILog.InfoFormat(string format, object arg0)
        {
            this.log.InfoFormat(format, arg0);
        }

        void Enyim.Caching.ILog.InfoFormat(string format, object arg0, object arg1)
        {
            this.log.InfoFormat(format, arg0, arg1);
        }

        void Enyim.Caching.ILog.InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.InfoFormat(format, arg0, arg1, arg2);
        }

        void Enyim.Caching.ILog.InfoFormat(string format, params object[] args)
        {
            this.log.InfoFormat(format, args);
        }

        void Enyim.Caching.ILog.InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.InfoFormat(provider, format, args);
        }

        void Enyim.Caching.ILog.Warn(object message)
        {
            this.log.Warn(message);
        }

        void Enyim.Caching.ILog.Warn(object message, Exception exception)
        {
            this.log.Warn(message, exception);
        }

        void Enyim.Caching.ILog.WarnFormat(string format, object arg0)
        {
            this.log.WarnFormat(format, arg0);
        }

        void Enyim.Caching.ILog.WarnFormat(string format, object arg0, object arg1)
        {
            this.log.WarnFormat(format, arg0, arg1);
        }

        void Enyim.Caching.ILog.WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this.log.WarnFormat(format, arg0, arg1, arg2);
        }

        void Enyim.Caching.ILog.WarnFormat(string format, params object[] args)
        {
            this.log.WarnFormat(format, args);
        }

        void Enyim.Caching.ILog.WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.WarnFormat(provider, format, args);
        }
    }
}

