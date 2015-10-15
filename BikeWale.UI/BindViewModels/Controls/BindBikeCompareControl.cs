using Bikewale.Entities.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Bike Compare View Model
    /// Author  :   Sumit Kate
    /// Date    :   02 Sept 2015
    /// </summary>
  public class BindBikeCompareControl
  {
    static readonly string m_bwHostUrl=ConfigurationManager.AppSettings["bwHostUrl"];
    static readonly string m_requestType = "application/json";
    static readonly string m_BikeCompareListString = "/api/BikeCompareList/?topCount=";
    /// <summary>
    /// Total records requested
    /// </summary>
    private static int m_TotalRecords = 0;
    public static int TotalRecords
    {
      get
      {
        return m_TotalRecords;
      }
      set
      {
        m_TotalRecords = value;
      }
    }
    /// <summary>
    /// Total Fetched records
    /// </summary>
    private static int m_FetchedRecordCount = 0;
    public static int FetchedRecordCount
    {
      get
      {
        return m_FetchedRecordCount;
      }
      set
      {
        m_FetchedRecordCount = value;
      }
    }

    public static IEnumerable<TopBikeCompareBase> m_CompareList;
    public static IEnumerable<TopBikeCompareBase> CompareList
    {
      get
      {
        return m_CompareList;
      }
      set
      {
        m_CompareList = value;
      }
    }

    public static TopBikeCompareBase FetchTopRecord()
    {
      if (m_FetchedRecordCount > 0)
      {
        return m_CompareList.First();
      }
      return null;
    }

    /// <summary>
    /// Bind the repeater to the Repeater
    /// </summary>
    /// <param name="repeater">Repeater object</param>
    public static void BindBikeCompare(Repeater repeater, int skipCount = 0)
    {
      try
      {
        if (m_FetchedRecordCount > 0 && repeater != null)
        {
          repeater.DataSource = m_CompareList.Skip(skipCount);
          repeater.DataBind();
        }
      }
      catch (Exception ex)
      {
        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        objErr.SendMail();
      }
    }

    public static void FetchBikeCompares()
    {
      IEnumerable<TopBikeCompareBase> topBikeCompares = null;

      string apiUrl = String.Empty;
      m_FetchedRecordCount = 0;
      try
      {
        apiUrl = m_BikeCompareListString + m_TotalRecords;// String.Format("/api/BikeCompareList/?topCount={0}", m_TotalRecords);

        topBikeCompares = BWHttpClient.GetApiResponseSync<IEnumerable<TopBikeCompareBase>>(m_bwHostUrl, m_requestType, apiUrl, topBikeCompares);
        if (topBikeCompares != null && topBikeCompares.Count() > 0)
        {
          m_FetchedRecordCount = topBikeCompares.Count();
          m_CompareList = topBikeCompares;
        }
        else
        {
          m_FetchedRecordCount = 0;
        }
      }
      catch (Exception ex)
      {
        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        objErr.SendMail();
      }
    }
  }
}