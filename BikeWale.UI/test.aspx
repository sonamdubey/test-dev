<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Bikewale.test" Trace="true" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>sssssssssssssssss<%= ConfigurationManager.AppSettings["sdsdsisMSMQdd"] %></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.NonReadOnlyStatic%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ABApiHostUrl%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ApplicationId%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ApplicationName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.AppPath%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.AutoExpo%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.AutoSuggestType%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.BillDeskWorkingKey%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.BWConnectionString%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.BwHostUrl%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.CityIndexName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.CwApiHostUrl%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.CWConnectionString%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.DefaultCity%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ElasticHostUrl%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ErrorMailTo%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.FeedbackEmailTo%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.GetDefaultCityName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ImageQueueName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ImgHostURL%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ImgPathFolder%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.IsMemcachedUsed%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.IsMSMQ%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.LocalMail%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.MailFrom%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.MemcacheTimespan%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.MMindexName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.MobileSourceId%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.OfferClaimAlertEmail%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.OfferUniqueTransaction%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.PageSize%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.PQindexName%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.RabbitImgHostURL%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.ReplyTo%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.SendError%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.SendSMS%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.SMTPSERVER%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.SourceId%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.StaticFileVersion%></h1>
        <h1>Test : <%= Bikewale.Utility.BWConfiguration.Instance.StaticUrl%></h1>
    </div>
    </form>
</body>
</html>
