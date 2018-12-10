using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

/// <summary>
/// Summary description for PostsBusinessLogic
/// </summary>
/// 
namespace Carwale.BL.Forums
{
    public class PostsBusinessLogic
    {
        #region Get Last Post
        public string GetLastPost(string threadId, string threadTitle, string postedBy, string postDate, string postedById, string url)
        {
            string lastPost = "";
            if (postDate.Length > 0)
            {
                lastPost = "<a href='" + threadId + "-" + url + ".html'>" + threadTitle + "</a>";
                if (postedBy != "anonymous")
                    lastPost += "<br>by <a target='_blank' title=\"View " + postedBy + "'s complete profile\" class='startBy' href='/community/members/" + postedBy + ".html'>" + postedBy + "</a>";
                else
                    lastPost += "<br>by <span class='startBy'>" + postedBy + "</span>";

                lastPost += ", <span class='startBy'>" + Convert.ToDateTime(postDate).ToString("dd-MMM-yy hh:mm tt") + "</span>";
            }

            return lastPost;
        }
        #endregion

        public string GetLastPost(string title, string name, string date, string id, string posts, string startedById, string url)
        {
            return GetLastPost(title, name, date, id, posts, startedById, false, url);
        }

        public string GetLastPost(string title, string name, string date, string id, string posts, string startedById, bool openInNewWindow, string url)
        {
            string retVal = "";
            if (title != "")
            {
                string openWindow = "";
                if (openInNewWindow) // to be opened in new window.
                    openWindow = " target='_blank'";
                // thread title
                retVal = "<a" + openWindow + " href='/forums/" + id + "-" + url + ".html'>" + title + "</a>";
                // if more than 10 posts in a thread, add pages strip
                if (Convert.ToInt32(posts) > 10)
                {
                    string pageStrip = "";
                    string pageUrl = id + "-" + url;
                    // total pages
                    int pages = (int)Math.Ceiling(Convert.ToDouble(posts) / 10);
                    int i = 0;
                    pageStrip = " (<img alt='Page' title='Go directly to page no' align='absmiddle' src='" + ConfigurationManager.AppSettings["imgRootPath"] + "/images/forums/multipage.gif' /> ";
                    i = pages < 5 ? pages : 5;
                    for (int j = 1; j <= i; j++)
                    {
                        if (j > 1) pageStrip += " ";
                        pageStrip += "<a href='/forums/" + pageUrl + "-p" + j.ToString() + ".html'>"
                                                + j.ToString() + "</a>";
                    }
                    if (pages > 5)
                    {
                        pageStrip += " ... ";
                        pageStrip += "<a href='/forums/" + pageUrl + "-p" + pages + ".html'>Last</a>";
                    }
                    pageStrip += ")";
                    retVal += pageStrip;
                }
                if (name != "")
                    if (name != "anonymous")
                        retVal += "<br>by <a target='_blank' title=\"View " + name + "'s complete profile\" class='startBy' href='/community/members/" + name + ".html'>" + name + "</a>";
                    else
                        retVal += "<br>by <span class='startBy'>" + name + "</span>";
            }
            else
                retVal = " ";
            return retVal;
        }
    }
}