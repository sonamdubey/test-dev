<%@ Page trace="false" AutoEventWireUp="false" Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Collections.Specialized" %>
<%@ Import Namespace="Carwale.DAL.CustomerVerification" %>

<script runat="server"  language="c#">

    protected override void OnInit(EventArgs e)
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        this.Load += new EventHandler(Page_Load);
    }

    void Page_Load(object sender, EventArgs e)
    {
        CheckResponse();       
    }

    public void CheckResponse()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["CallSid"]))
        {
            string transToken = Request.QueryString["CallSid"];
            string fromCall = Request.QueryString["From"];
            if (fromCall.Length > 10)
            {
                fromCall = fromCall.Substring(fromCall.Length - 10, fromCall.Length - 1);
            }
            string toCall = Request.QueryString["To"];
            string email;
            CustomerVerificationRepository custVeriRepo = new CustomerVerificationRepository();
            custVeriRepo.VerifyByMissedCall(fromCall, transToken, toCall, out email);
        }
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    </form>
</body>
</html>
