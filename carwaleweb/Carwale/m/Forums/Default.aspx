<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.Default"  AutoEventWireup="false" trace="false" ViewStateMode="Disabled" %>
<%
	Title = "Car Forums | Ask, Answer and Discuss about Cars - CarWale";
	Keywords = "car forum, auto forum, car forum India, car forums, car discussions, car help, car howtos";
	Description = "India's finest car discussion forum. Discuss anything related to cars in India. Ask car related questions and get fast response.";
    Canonical = "https://www.carwale.com" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString().ToString().Replace("/m/", "/");
    MenuIndex = "7";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style type="text/css">
   .box .plus {
    width: 18px;
    height: 18px;
    position: absolute;
    top: 10px;
    right: 5px;
    background-image: url("/m/images/icons-sheet.png?v=cwkghjfyfbnstyuitgktiutggbkyoi");
    background-position: -2px -248px;
}
   .box .minus {
    width: 18px;
    height: 18px;
    position: absolute;
    top: 10px;
    right: 5px;
    background-image: url("/m/images/icons-sheet.png?v=cwkghjfyfbnstyuitgktiutggbkyoi");
    background-position: -2px -272px;
}
   .arr-small {
    color: red;
    font-size: 15px;
}
</style>
</head>

<body class="m-special-skin-body m-no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	
        <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12">
            <h1 class="pgsubhead margin-bottom10 m-special-skin-text hide">Search Forums</h1>
            <div id="searchExt" class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom20 hide">
                <form runat="server">
                <div id="spnSearch" class="error"></div>
                <div class="new-line5 search-radio-btns">
                    <asp:RadioButton ID="rdoTitles" runat="server" Text="Titles" GroupName="type" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoAll" runat="server" Text="Threads" GroupName="type" Checked="true" />
                    <div class="clear"></div>
                </div>
                <div class="margin-top10">
                    <table class="table" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td><asp:TextBox id="txtSearch" runat="server"  CssClass="form-control" data-role="textBox"/></td>
                            <td style="width:50px;height:38px;padding-left:10px;" onclick="SearchClicked();">
                                <div style='width:40px;height:38px;background-image: url("../images/nav.png");background-position: 0px 0px;border-radius:7px;-moz-border-radius:7px;-webkit-border-radius:7px;'>
                                    <div style="height:30px;background-image: url('../images/icons-sheet.png'); background-repeat:no-repeat;background-position: 8px -668px;"></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <span id="currSearchPage" style="display:none;">1</span>
                <span id="currSearchValue" style="display:none;"></span>
                <span id="currSearchType" style="display:none;"></span>
                </form>
            </div>
            <h2 class="pgsubhead m-special-skin-text">Forum Categories</h2>
            <div id="forumContainer" class="margin-bottom20">
            <asp:Repeater id="rptParent" runat="server">
                <itemtemplate>
                    <div class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-top10 position-rel" onclick="BoxClicked(this);" type="expando">
                        <div class="heading"><%# DataBinder.Eval(Container.DataItem, "Name").ToString() %></div>
                        <div class="plus"></div>
                    </div>
                    <div class="box-bot hide box-bot content-inner-block-10 content-box-shadow rounded-corner2 text-black">
                    <asp:Repeater runat="server" DataSource='<%# GetSubCategories(DataBinder.Eval(Container.DataItem, "ID").ToString()) %>'>
                        <itemtemplate>
                        <%--<a href='/m/forums/viewforum-<%# DataBinder.Eval(Container.DataItem, "SubCatId").ToString() %>.html' class="normal">--%>
                        <a href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "Url").ToString() %>/' class="normal">
                            <div class="container">
                                <div class="sub-heading">
                                    <%# DataBinder.Eval(Container.DataItem, "SubCatName").ToString() %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                </div>
                                <div class="text-grey border-solid-bottom padding-bottom5 margin-bottom5 margin-top5">
                                    <%# DataBinder.Eval(Container.DataItem, "Posts").ToString() %> Posts | <%# DataBinder.Eval(Container.DataItem, "Threads").ToString() %> Threads
                                </div>
                            </div>
                        </a>
                        </itemtemplate>
                    </asp:Repeater>	
                    </div>
                </itemtemplate>
            </asp:Repeater>
            </div>
            <h2 class="margin-bottom10 m-special-skin-text">Latest Discussions</h2>
            <div class="box-bot content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom20">
                <asp:Repeater ID="rptNewDiscussions" runat="server">
                    <itemtemplate>
                    <div class="margin-bottom10">
                    <a href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "FId").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "PostUrl").ToString() %>.html' class="normal" style="text-decoration:none">	
                        <div class="container text-black"><span><%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %></span>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></div>	
                    </a>	
                    </div>
                    </itemtemplate>
                </asp:Repeater>
            </div>
            <h2 class="margin-bottom10 m-special-skin-text">Hot Discussions</h2>
            <div class="box-bot content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom20">
                <asp:Repeater ID="rptHotDiscussions" runat="server">
                    <itemtemplate>
                    <div class="margin-bottom10">
                    <a href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "FId").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "PostUrl").ToString() %>.html' class="normal" style="text-decoration:none">
                        <div class="container text-black"><span><%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %></span>  <span class="darkgray">(<%# DataBinder.Eval(Container.DataItem,"TOTALPOSTS")%> new posts)</span>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></div>	
                    </a>
                    </div>
                    </itemtemplate>
                </asp:Repeater>
            </div>
            
            </div>
        </div>
            <div class="clear"></div>
        <!--Main container ends here-->
        </section>
        <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script language="javascript" type="text/javascript">
        Common.showCityPopup = false;
        function BoxClicked(box) {
            var divIcon = $(box).find("div:nth-child(2)");
            if (divIcon.attr("class").toString() == "plus") {
                divIcon.attr("class", "minus");
                $(box).next().show();
                $(box).addClass("bot-rad-0");
            }
            else {
                divIcon.attr("class", "plus");
                $(box).next().hide();
                $(box).removeClass("bot-rad-0");
            }
        }

        function SubCategoryClicked(subCatId) {
            location.href = "/m/viewthreads.aspx?forum=" + subCatId;
        }

        function ThreadClicked(threadId) {
            location.href = "/m/viewposts.aspx?thread=" + threadId;
        }

        $(document).ready(function () {

            SetDefaultSearchOptions();
        });

        function SetDefaultSearchOptions() {
            var cookieVal = GetCookieVal();
            if (cookieVal != "") {
                var splittedCookieVal = cookieVal.split("~");
                if (splittedCookieVal.length == 5) {
                    $("#txtSearch").val(splittedCookieVal[0]);
                    if (splittedCookieVal[1] == "et") {
                        $("#rdoAll").attr("checked", true); $("#rdoTitles").attr("checked", false);
                    }
                    else {
                        $("#rdoAll").attr("checked", false); $("#rdoTitles").attr("checked", true);
                    }
                    $("#currSearchValue").html(splittedCookieVal[0]);
                    $("#currSearchType").html(splittedCookieVal[1]);
                    $("#currSearchPage").html(splittedCookieVal[4]);
                }
            }
        }

        function GetCookieVal() {
            var theCookie = "" + document.cookie;
            var ind = theCookie.indexOf("CurrForumSearch");
            if (ind == -1 || "CurrForumSearch" == "") return "";
            var ind1 = theCookie.indexOf(';', ind);
            if (ind1 == -1) ind1 = theCookie.length;
            return unescape(theCookie.substring(ind + "CurrForumSearch".length + 1, ind1));
        }

        function SearchClicked() {
            var searchTerm = $("#txtSearch").val().trim();

            var searchType = "";
            if ($("#rdoAll").is(":checked"))
                searchType = "et";
            else
                searchType = "to";

            var currPage;
            if ($("#currSearchValue").html() == searchTerm && $("#currSearchType").html() == searchType)
                currPage = $("#currSearchPage").html();
            else
                currPage = "1";

            if (IsValid())
                location.href = "/m/forums/search.aspx?s=" + searchTerm + "&st=" + searchType + "&pg=" + currPage;
        }

        function IsValid() {
            var retVal = true;
            $("#spnSearch").html("");

            var searchVal = $("#txtSearch").val().trim();

            if (searchVal == "") {
                retVal = false;
                $("#spnSearch").html("(Required)");
            }
            else if (searchVal.length < 3) {
                retVal = false;
                $("#spnSearch").html("(Min. 3 Chars)");
            }

            return retVal;
        }
            </script>
</body>
</html>