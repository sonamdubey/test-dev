<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Bikewale.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ABApiHostUrl%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ApplicationId%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ApplicationName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.AppPath%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.AutoExpo%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.AutoSuggestType%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.BillDeskWorkingKey%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.BWConnectionString%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.BwHostUrl%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.CityIndexName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.CwApiHostUrl%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.CWConnectionString%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.DefaultCity%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.DefaultName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ElasticHostUrl%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ErrorMailTo%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.FeedbackEmailTo%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.GetDefaultCityName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ImageQueueName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ImgHostURL%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ImgPathFolder%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.IsMemcachedUsed%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.IsMSMQ%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.LocalMail%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.MailFrom%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.MemcacheTimespan%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.MMindexName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.MobileSourceId%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.OfferClaimAlertEmail%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.OfferUniqueTransaction%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.PageSize%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.PQindexName%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.RabbitImgHostURL%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.ReplyTo%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.SendError%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.SendSMS%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.SMTPSERVER%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.SourceId%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.StaticFileVersion%></h1>
        <h1>Test : <%= Bikewale.Common.BWConfiguration.StaticUrl%></h1>
    </div>
    </form>
</body>
</html>
