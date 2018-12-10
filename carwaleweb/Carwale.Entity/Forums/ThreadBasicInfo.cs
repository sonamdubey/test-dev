using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// This class holds the basic thread information properties.
/// </summary>
/// 
namespace Carwale.Entity
{
    [Serializable]
    public class ThreadBasicInfo
    {
        #region Global Variables
        protected string _ForumId = "-1", _ForumName = "-1", _ForumDescription = "-1", _ThreadName = "-1", _StartedOn = "-1", _ThreadUrl = "-1", _ForumUrl = "-1", _StartedByEmail = "-1", _StartedByName = "-1";
        protected bool _ReplyStatus = false, _IsStarterFake = false;
        #endregion

        #region Properties
        public string ForumId
        {
            get { return _ForumId; }
            set { _ForumId = value; }
        }
        public string ForumName
        {
            get { return _ForumName; }
            set { _ForumName = value; }
        }
        public string ForumDescription
        {
            get { return _ForumDescription; }
            set { _ForumDescription = value; }
        }
        public string ThreadName
        {
            get { return _ThreadName; }
            set { _ThreadName = value; }
        }
        public string StartedOn
        {
            get { return _StartedOn; }
            set { _StartedOn = value; }
        }
        public bool replyStatus
        {
            get { return _ReplyStatus; }
            set { _ReplyStatus = value; }
        }
        public string ThreadUrl
        {
            get { return _ThreadUrl; }
            set { _ThreadUrl = value; }
        }
        public string ForumUrl
        {
            get { return _ForumUrl; }
            set { _ForumUrl = value; }
        }
        public string StartedByEmail
        {
            get { return _StartedByEmail; }
            set { _StartedByEmail = value; }
        }
        public string StartedByName
        {
            get { return _StartedByName; }
            set { _StartedByName = value; }
        }
        public bool IsStarterFake
        {
            get { return _IsStarterFake; }
            set { _IsStarterFake = value; }
        }
        #endregion

    } //class
}//namespace