<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ViewTA" Trace="false" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%
    title = ArticleTitle;
    description = "Bike Tips And Advices - " + ArticleTitle;
    keywords = "car tips, car advices, car how to";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<!-- #include file="/UI/includes/headnew.aspx" -->
<div class="container_12">
    <div class="grid_12"><ul class="breadcrumb"><li>You are here: </li><li><a href="/">Home</a></li><li>&rsaquo;<a title="Tips And Advices" href="/tipsadvices/">Bike Tips And Advices</a></li><li class="current">&rsaquo; <strong><%= ArticleTitle%></strong></li></ul><div class="clear"></div></div>
	<div class="grid_8 margin-top10">        
		<h1 class="hd1"><%= ArticleTitle%></h1>               
		<div class="byline" style="padding-bottom:5px;"><asp:Label ID="lblAuthor" runat="server" />, <asp:Label ID="lblDate" runat="server" /></div>
		    
        <div class="clear"></div>
		<div class="margin-top10 content-block grey-bg" id="topNav" runat="server">
			<div align="right" style="width:245px;float:right;">
				<asp:DropDownList ID="drpPages" CssClass="drpClass" AutoPostBack="true" runat="server"></asp:DropDownList>
			</div>
			<div style="width:380px; padding:5px 0;">
				<b>Read Page : </b>
				<asp:Repeater ID="rptPages" runat="server">
					<itemtemplate>
						<%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url ) %>
					</itemtemplate>
					<footertemplate>
						<% if ( ShowGallery )  { %>
						<%# CreateNavigationLink( Str, Url ) %>
						<% } %>	
					</footertemplate>
				</asp:Repeater>
			</div>	
		</div>
		<div class="margin-top10">
			<div class="format-content"><asp:Label ID="lblDetails" runat="server" /></div>
            <div id="divOtherInfo" runat="server"></div>
			<asp:DataList ID="dlstPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="top">
				<itemtemplate>
					<a rel="slidePhoto" target="_blank" rel="noopener" href="<%# "https://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathLarge" ).ToString() %>" title="<b><%# DataBinder.Eval( Container.DataItem, "Caption" ).ToString() %></b>" />
						<%--<img alt="<%# DataBinder.Eval( Container.DataItem, "MakeName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "ModelName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "CategoryName" ).ToString() %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# "https://" + DataBinder.Eval( Container.DataItem, "HostURL" ).ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ).ToString() %>" title="Click to view larger photo" />--%>
                        <img alt="<%# DataBinder.Eval( Container.DataItem, "MakeName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "ModelName" ).ToString() + " " + DataBinder.Eval( Container.DataItem, "CategoryName" ).ToString() %>" border="0" style="margin:0px 45px 10px 0px;cursor:pointer;" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString(),DataBinder.Eval( Container.DataItem, "HostURL" ).ToString(),Bikewale.Utility.ImageSize._144x81) %>" title="Click to view larger photo" />
					</a>
				</itemtemplate>
			</asp:DataList>
		</div>
		<div class="margin-top10 content-block grey-bg" id="bottomNav" runat="server">
			<div align="right" style="width:245px;float:right;">
				<asp:DropDownList ID="drpPages_footer" AutoPostBack="true" CssClass="drpClass" runat="server"></asp:DropDownList>
			</div>
			<div style="width:380px; padding:5px 0;">
				<b>Read Page : </b>
				<asp:Repeater ID="rptPages_footer" runat="server">
					<itemtemplate>
						<%# CreateNavigationLink(DataBinder.Eval( Container.DataItem, "Priority" ).ToString(), Url) %>
					</itemtemplate>
					<footertemplate>
						<% if ( ShowGallery )  { %>
						<%# CreateNavigationLink( Str, Url ) %>
						<% } %>	
					</footertemplate>
				</asp:Repeater>
			</div>	
		</div>
    </div>   
    <div class="grid_4">
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250.aspx" -->
        </div>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top5">
            <h1>Instant bike On-Road Price</h1>
            <p>Check the on-road price absolutely free</p>
            <div class="action-btn margin-top5"><a href="#">Check Now</a></div>
            <div class="clear"></div>
        </div>
        <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20 margin-top15">
            <h1>Locate Dealer</h1>
            <p>Find a new bike dealer & authorized showroom</p>
            <div class="left-float margin-top10 margin-right10 padding-bottom20">
                <input type="text" onclick="this.value='';" onfocus="this.select()" onblur="this.value=!this.value?'Enter City Name':this.value;" value="Enter City Name" />
            </div>
            <div class="action-btn margin-top10"><a href="#">Go</a></div>                            
            <div class="clear"></div>
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>    
</div>
<script type="text/javascript" type="text/javascript">
    $(document).ready(function () {
        $("#btnSubmit").click(function () {
            if (Validate()) {
                return false;
            }
            //return false;
        });
        $("#lnkRegCaptcha").click(function () { RegenerateCaptcha(); return false; });
        $("#txtUserName").click(function () {
            if ($("#txtUserName").val() == $("#txtUserName").attr("placeholder")) {
                $("#txtUserName").val("");
            }
        }).focus(function () {
            if ($("#txtUserName").val() == $("#txtUserName").attr("placeholder")) {
                $("#txtUserName").val("");
            }
        }).blur(function () {
            if ($("#txtUserName").val() == "") {
                $("#txtUserName").val($("#txtUserName").attr("placeholder"));
            }
        });
        $("#txtUserEmail").click(function () {
            if ($("#txtUserEmail").val() == $("#txtUserEmail").attr("placeholder")) {
                $("#txtUserEmail").val("");
            }
        }).focus(function () {
            if ($("#txtUserEmail").val() == $("#txtUserEmail").attr("placeholder")) {
                $("#txtUserEmail").val("");
            }
        }).blur(function () {
            if ($("#txtUserEmail").val() == "") {
                $("#txtUserEmail").val($("#txtUserEmail").attr("placeholder"));
            }
        });
        $("#txtComment").click(function () {
            if ($("#txtComment").val() == $("#txtComment").attr("placeholder")) {
                $("#txtComment").val("");
            }
        }).focus(function () {
            if ($("#txtComment").val() == $("#txtComment").attr("placeholder")) {
                $("#txtComment").val("");
            }
        }).keydown(function () {
            showCharactersLeft();
        }).blur(function () {
            if ($("#txtComment").val() == "") {
                $("#txtComment").val($("#txtComment").attr("placeholder"));
            }
            else {
                showCharactersLeft();
            }
        });
    });

    function Validate() {
        var isError = false;
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        var regExp = /^[A-Za-z ]$/;

        var name = $("#txtUserName").val();
        var email = $("#txtUserEmail").val();
        var comment = $("#txtComment").val();
        var captcha = $("#txtCaptcha").val();

        $("#spnName").text("");
        $("#spnEmail").text("");
        $("#spnError").text("");
        $("#spnCaptcha").text("");

        if (name != null && name != "") {
            for (var i = 0; i < name.length; i++) {
                if (!name.charAt(i).match(regExp)) {
                    $("#spnName").text("Please enter your name");
                    isError = true;
                }
            }
        } else {
            $("#spnName").text("Please enter your name");
            isError = true;
        }

        if (email != null && email != "") {
            if (email != $("#txtUserEmail").attr("placeholder")) {
                if (!emailPattern.test(email)) {
                    $("#spnEmail").text("Please enter a valid email");
                    isError = true;
                }
            } else {
                $("#spnEmail").text("Please enter your email");
                isError = true;
            }
        } else {
            $("#spnEmail").text("Please enter your email");
            isError = true;
        }

        if (comment == $("#txtComment").attr("placeholder")) {
            $("#spnError").text("Please enter the comment");
            isError = true;
        } else if (comment == "") {
            $("#spnError").text("Please enter the comment");
            isError = true;
        }

        if (captcha == "") {
            $("#spnCaptcha").text("Please enter the code shown above");
            isError = true;
        }

        return isError;
    }

    function RegenerateCaptcha() {
        $("#ifrmCaptcha").attr("src", "/Common/CaptchaImage/JpegImage.aspx");
    }

    function showCharactersLeft() {
        var maxSize = 500;
        var comment = $("#txtComment").val();
        var size = comment.length;

        if (size >= maxSize) {
            $("#txtComment").val(comment.substring(0, maxSize - 1));
            size = maxSize;
        }

        $("#spnDesc").html("Characters Left : " + (maxSize - size));
    }

</script>
<!-- #include file="/UI/includes/footerInner.aspx" -->
