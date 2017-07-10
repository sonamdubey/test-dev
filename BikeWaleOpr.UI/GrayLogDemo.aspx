<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Debug="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>GreyLog Demo Page</title>
</head>
<body>
    </form>
    <script runat="server">
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {  
            sampleexception();
        }
        
        protected void sampleexception()
        {
            try{
				throw new System.Exception("This is demo exception from  : " + System.Environment.MachineName);
			}catch(Exception ex){
			BikeWaleOpr.Common.ErrorClass err = new BikeWaleOpr.Common.ErrorClass(ex,"PageLoad");
			HttpContext.Current.Response.Write(ex.Message);
			}
        }        
        
        </script>    
</body>
</html>
