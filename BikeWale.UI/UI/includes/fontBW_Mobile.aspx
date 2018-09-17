<script language="c#" runat="server">
    string fontFile = "";
    string fontUrl = "";	
</script>
<%     fontFile  = "/UI/css/fonts/OpenSans/open-sans-v15-latin-regular.woff";
       fontUrl  = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion);
%>
<style>
    @font-face { 
        font-family: 'Open Sans';
        font-style: normal;
        font-weight: 400;
        src: local('Open Sans Regular'), local('OpenSans-Regular'), url('<%=fontUrl%>') format('woff');
    }

</style>
<%     fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-600.woff"; %>
<%      fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); %>
<style>
     @font-face { 
        font-family: 'Open Sans';
        font-style: normal;
        font-weight: 600;
        src: local('Open Sans SemiBold'), local('OpenSans-SemiBold'), url('<%=fontUrl%>') format('woff');
     }
</style>
<%     fontFile = "/UI/css/fonts/OpenSans/open-sans-v15-latin-700.woff"; %>
<%       fontUrl = String.Format("{0}{1}?{2}", Bikewale.Utility.BWConfiguration.Instance.StaticUrl, fontFile, Bikewale.Utility.BWConfiguration.Instance.StaticCommonFileVersion); %>
<style>
     @@font-face { 
	    font-family: 'Open Sans';
	    font-style: normal;
	    font-weight: 700;
	    src: local('Open Sans Bold'), local('OpenSans-Bold'), url('<% =fontUrl %>') format('woff');
     }
</style>
