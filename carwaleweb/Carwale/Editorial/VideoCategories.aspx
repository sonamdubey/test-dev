<%@ page language="C#" autoeventwireup="false" inherits="Carwale.UI.Editorial.VideoCategories" trace="false" debug="false" ViewStateMode="Disabled" %>
<%@ register tagprefix="Vspl" tagname="RepeaterPager" src="/Controls/VideosPager.ascx" %>
<%@ import namespace="Carwale.UI.Common" %>
<%@ import namespace="System.Data" %>
<style type="text/css">
    .play-btn { background:url(https://imgd.aeplcdn.com/0x0/cw/static/sprites/video-sprite.png) no-repeat; display:inline-block;background-position: 0 -163px;bottom: 35%;height: 38px;left: 42%;position: absolute;width: 38px;}
    #tbl_res { border: 1px solid #CECECE; border-collapse: collapse; }
    table { color: #2F2F2F;}
    .dtTable{font-size:12px;}
    .dtTable td{padding:8px;}
    .dt_header td{padding:10px 5px; border-bottom:1px solid #E9E9E7; background-color:#F6F6F6; font-weight:bold;}
    .dt_header a{color:#2f2f2f; font-weight:bold; text-decoration:underline;}
    .dt_body{cursor:pointer;}
    .dt_body td{/*border-bottom:1px solid #E4E4E4;*/ padding:10px 5px;}
    .dt_body_hover{background-color:#F6F6F6;}    
    /* Navigation */
    span.pg{padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px;}
    span.pgSel{background-color:#CCDBF8; padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px; color:#5B5B5B; font-weight:bold;}
    span.pgEnd{padding:2px 5px; border:1px solid #ABABAB; margin:0 2px; color:#898989; cursor:default;}
    .dgNavDiv td{padding:10px 0 10px 0; background-color:#F6F6F6; border:0;}
    .dgNavDivTop a:hover{text-decoration:none;}
    .dgNavDivTop td{padding:8px; background:url(https://imgd.aeplcdn.com/0x0/statics/used/search_head.gif) repeat-x;}
     #tbl_res {border: none!important;}
</style>
<vspl:repeaterpager id="rpgVideos" resultname="Cars" showheadersvisible="true" pagerposition="TopBottom" runat="server">
	<asp:Repeater ID="rptVideos" runat="server" EnableViewState="false">
	<itemtemplate>
		<li>
            <div class="pic-place">
               <a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank">
                    <div class="play-btn"></div>
                   <img class="lazy" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" data-original="<%# !string.IsNullOrWhiteSpace(DataBinder.Eval(Container.DataItem,"ImagePath").ToString())? DataBinder.Eval(Container.DataItem,"ImgHost").ToString()+ Carwale.Utility.ImageSizes._210X118 + DataBinder.Eval(Container.DataItem,"ImagePath").ToString() :"https://img.youtube.com/vi/"+DataBinder.Eval(Container.DataItem,"VideoId").ToString()+"/mqdefault.jpg" %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>"  width="202" height="114" border="0" />
                </a>
            </div>
            <div class="content-place">
                <p><a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank"><strong><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></strong></a></p>
                <div>
                    <span class="video-sprite review-icon-light margin-right5"></span> <%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"Views").ToString()).ToString("#,##0") %> Views
                </div>
            </div>
        </li>
	</itemtemplate>
	</asp:Repeater>
</vspl:repeaterpager>

<div id="emptyVid" runat="server" visible="false"><span>Videos Coming Soon...</span></div>
<script type="text/javascript">
    Common.showCityPopup = false;
</script>  