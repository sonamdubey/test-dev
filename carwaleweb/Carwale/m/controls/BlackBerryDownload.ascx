<%@ Control Language="C#" AutoEventWireUp="false"%>
<style>
    .btnClose {position:absolute; width:18px; height:19px; right:5px;top:5px;background-image:url('https://st.aeplcdn.com/m/images/cross.png'); }
    .f-14Bold {font-size:14px;font-weight:bold;}
    .f-12 {font-size:12px;}
    .rateFull {height:11px;width:12px;background-image:url('https://st.aeplcdn.com/m/images/star.png'); float:left;margin-right:5px;margin-top:4px;}
    .rateHalf {height:11px;width:12px;background-image:url('https://st.aeplcdn.com/m/images/star_half.png'); float:left;margin-right:5px;margin-top:4px;}
    .btnInstallBlackBerry {width:80px;height:29px;background:url(https://st.aeplcdn.com/m/images/btn_blackberry.png);background-repeat:repeat-x;border-radius:7px;-moz-border-radius:7px;-webkit-border-radius:7px;text-align:center;line-height:29px;padding:0px 10px;color:black;text-decoration:none;font-size:14px;}
</style>


<%
    if ((HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] ?? string.Empty).ToString().ToLower().IndexOf("blackberry") > -1 && Request.Cookies["BlackBerryDownload"] == null)
//if(Request.Cookies["BlackBerryDownload"] == null)
{
%>
<div id="divBlackBerry" style="padding:10px 5px; background-color:#ffffff;position:relative;">  
    <table style="border:0px;" cellpadding="0" cellspacing="0">
		<tr>
			<td style="width:40px;"><a href="http://appworld.blackberry.com/webstore/content/53782888/" class="normal"><div><img src="https://st.aeplcdn.com/m/images/logo_blackberry.png"/></div></a></td>
            <td style=" padding:0px 10px;vertical-align:top;">
                <a href="http://appworld.blackberry.com/webstore/content/53782888/" class="normal">
                    <div class="f-14Bold">Download Our BlackBerry App</div>
                </a>
                <div class="f-12 new-line5">
                    <div class="rateFull"></div>
                    <div class="rateFull"></div>
                    <div class="rateFull"></div>
                    <div class="rateFull"></div>
                    <div class="rateFull"></div>
                    <div style="margin:3px 10px 0px 5px;">5 / 5</div>
                </div>  
                <a href="http://appworld.blackberry.com/webstore/content/53782888/" class="normal">    
                    <div class="new-line10 btnInstallBlackBerry">Open </div>
                </a>
            </td>
		</tr>
	</table>
    <div class="btnClose" onclick="CloseBlackBerryLink();"></div>
</div>

<%
 } 
%>

<script language="javascript" type="text/javascript">
    function CloseBlackBerryLink() {
        $("#divBlackBerry").slideUp(500);
        var expiryTime = 1000 * 60 * 60 * 24 * 30;  //30 days
        //var expiryTime = 60000;  //1 min
        var expires = new Date((new Date()).valueOf() + expiryTime);
        //alert("expires : " + expires);
        document.cookie = "BlackBerryDownload=1;expires=" + expires.toUTCString() + ";path=/";
    }
</script>