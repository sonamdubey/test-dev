<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Bikewale.test" Trace="false" Debug="true" %>
<%@ Register Src="/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />  
        
        <%
           Enyim.Caching.MemcachedClient _mc1= new Enyim.Caching.MemcachedClient("memcached");
                      
              string key1 = "BW_MakeMapping";

              var cacheObject1 = _mc1.Get(key1);
                
              if (cacheObject1 != null)
              {
                  HttpContext.Current.Response.Write("Given key '" + key1 + "' - object exists in the memcache. On Page");
              }
              else
              {
                  HttpContext.Current.Response.Write("Given key '" + key1 + "' - object do not exists in the memcache. On Page");
              }
			  
			  bool refreshKey = false;
			  
			  if(refreshKey)
			  {
                  _mc1.Remove(key1);
				HttpContext.Current.Response.Write("Given key '" + key1 + "' - object removed from the memcache. On Page");
			  }
			  
            %>    
    </div>
    </form>
</body>
</html>
