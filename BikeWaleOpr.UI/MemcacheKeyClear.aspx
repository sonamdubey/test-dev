<%@ Page Language="C#" AutoEventWireup="false" Trace="false" Debug="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Memcache Check Page</title>
</head>
<body>
<form id="form1" runat="server">        
        Memcache Key :<asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
        <asp:Button ID="btnGetVal" Text="Get Memcache Object" runat="server" OnClick="btnGetVal_click" />
		<asp:CheckBox id="chkClearCache" runat="server"
                    AutoPostBack="false"
                    Text="Clear memcache object"
                    TextAlign="Right" />
    </form>
    <script runat="server">
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {  
            HttpContext.Current.Response.Write(BikeWaleOpr.Common.CurrentUser.Id); 				
        }
        
        protected void btnGetVal_click(object sender, EventArgs e)
        {
            string keyName = "";
            keyName = txtKey.Text;
			
            try{
			if (!String.IsNullOrEmpty(keyName))
	        {
		        Enyim.Caching.MemcachedClient _mc1= new Enyim.Caching.MemcachedClient("memcached");
                      
                        string key1 = keyName;

                        var cacheObject1 = _mc1.Get(key1);
                
                        /*if (cacheObject1 != null)
                        {
				        HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(cacheObject1));
				        HttpContext.Current.Response.Write("<br />");
                        HttpContext.Current.Response.Write("Given key '" + key1 + "' - object exists in the memcache. On Page");
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("Given key '" + key1 + "' - object do not exists in the memcache. On Page");
                        }*/
			  
                        bool refreshKey = chkClearCache.Checked;
			  
                        if(refreshKey)
                        {
						for(int i=0;i<1500;i++){
						_mc1.Remove(key1 + i);
						}                          
                          HttpContext.Current.Response.Write("Given key '" + key1 + "' - object removed from the memcache. On Page");
                        }
						else{
							HttpContext.Current.Response.Write("Given key '" + key1 + "' - object not removed from the memcache. On Page");
						}						
	        }
			}catch(Exception ex){
			Bikewale.Notifications.ErrorClass err = new Bikewale.Notifications.ErrorClass(ex,"PageLoad");
			HttpContext.Current.Response.Write(ex.Message);
			}
        }        
        
        </script>    
</body>
</html>
