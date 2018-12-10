<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.Default"  AutoEventWireup="false" trace="false" %>
<%
	Title = "Car Forums | Ask, Answer and Discuss about Cars - CarWale";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="../includes/global-scripts.aspx" -->
<link rel="stylesheet" href="/static/m/css/design.css" type="text/css" >
</head>

<body>
	<!--Outer div starts here-->
	<div data-role="page">
    	<!--Main container starts here-->
    	<div id="main-container">
			<!-- #include file="../includes/global-header.aspx" -->
            <div class="pgsubhead">Search Forums</div>
            <div id="searchExt" class="box new-line5" style="padding-top:5px;padding-bottom:5px;">
                <form runat="server">
                <div class="new-line5">
                    <asp:RadioButton ID="rdoTitles" runat="server" Text="Titles" GroupName="type"  />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoAll" runat="server" Text="Threads" GroupName="type" Checked="true" />
                </div>
                <div class="new-line10">
                    <table class="table" cellpadding="0" cellspacing="0">
                        <tr>
                            <td><asp:TextBox id="txtSearch" runat="server" style="height:25px;" /></td>
                            <td style="width:50px;height:30px;padding-left:10px;" onclick="SearchClicked();">
                                <div style='height:30px;background-image: url("../images/nav.png");background-position: 0px 0px;border-radius:7px;-moz-border-radius:7px;-webkit-border-radius:7px;'>
                                    <div style="height:30px;background-image: url('../images/icons-sheet.png');background-position: 12px -672px;"></div>
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
            <div class="pgsubhead">Forum Categories</div>
            <div id="forumContainer">
            <asp:Repeater id="rptParent" runat="server">
                <itemtemplate>
                    <div class="box new-line5" onclick="BoxClicked(this);" type="expando">
                        <div class="heading"><%# DataBinder.Eval(Container.DataItem, "Name").ToString() %></div>
                        <div class="plus"></div>
                    </div>
                    <div class="box-bot" style="display:none;padding:0px 5px;">
                    <asp:Repeater runat="server" DataSource='<%# GetSubCategories(DataBinder.Eval(Container.DataItem, "ID").ToString()) %>'>
                        <itemtemplate>
                        <a href='ViewThreads.aspx?forum=<%# DataBinder.Eval(Container.DataItem, "SubCatId").ToString() %>' class="normal">
                            <div class="container">
                                <div class="sub-heading">
                                    <%# DataBinder.Eval(Container.DataItem, "SubCatName").ToString() %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                </div>
                                <div class="lightgray">
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
            <div class="box-top bot-red new-line5">Latest Discussions</div>
            <div class="box-bot" style="padding:0px 5px;">
                <asp:Repeater ID="rptNewDiscussions" runat="server">
                    <itemtemplate>
                    <a href='ViewPosts.aspx?thread=<%# DataBinder.Eval(Container.DataItem, "FId").ToString() %>' class="normal">	
                        <div class="container"><span><%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %></span>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></div>	
                    </a>	
                    </itemtemplate>
                </asp:Repeater>
            </div>
            <div class="box-top bot-red new-line5">Hot Discussions</div>
            <div class="box-bot" style="padding:0px 5px;">
                <asp:Repeater ID="rptHotDiscussions" runat="server">
                    <itemtemplate>
                    <a href='ViewPosts.aspx?thread=<%# DataBinder.Eval(Container.DataItem, "FId").ToString() %>' class="normal">
                        <div class="container"><span><%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %></span>  <span class="darkgray">(<%# DataBinder.Eval(Container.DataItem,"TOTALPOSTS")%> new posts)</span>&nbsp;&nbsp;<span class="arr-small">&raquo;</span></div>	
                    </a>
                    </itemtemplate>
                </asp:Repeater>
            </div>
            <script language="javascript" type="text/javascript">
                Common.showCityPopup = false;
                function BoxClicked(box)
                {
                    var divIcon = $(box).find("div:nth-child(2)");
                    if (divIcon.attr("class").toString() == "plus"){
                        divIcon.attr("class", "minus");
                        $(box).next().show();
                        $(box).addClass("bot-rad-0");
                    }
                    else{
                        divIcon.attr("class", "plus");
                        $(box).next().hide();
                        $(box).removeClass("bot-rad-0");	
                    }
                }
                
                function SubCategoryClicked(subCatId)
                {
                    location.href = "ViewThreads.aspx?forum=" + subCatId;
                }
                
                function ThreadClicked(threadId)
                {
                    location.href = "ViewPosts.aspx?thread=" + threadId;
                }
                
                $(document).ready(function(){
                    $("input[type='text']").each(function(){
                        $(this).width(parseInt($(this).parent().width())-5);
                    });
                    
                    SetDefaultSearchOptions();
                    ActivateIndex(2);
                    ActivateMenu(3);
                });
                
                function SetDefaultSearchOptions()
                {
                    var cookieVal = GetCookieVal();
                    if (cookieVal != "")
                    {
                        var splittedCookieVal = cookieVal.split("~");
                        if (splittedCookieVal.length == 5)
                        {
                            $("#txtSearch").val(splittedCookieVal[0]);
                            if (splittedCookieVal[1] == "et")
                            {
                                $("#rdoAll").attr("checked", true);$("#rdoTitles").attr("checked", false);
                            }
                            else
                            {
                                $("#rdoAll").attr("checked", false);$("#rdoTitles").attr("checked", true);
                            }
                            $("#currSearchValue").html(splittedCookieVal[0]);
                            $("#currSearchType").html(splittedCookieVal[1]);
                            $("#currSearchPage").html(splittedCookieVal[4]);
                        }
                    }	
                }
                
                function GetCookieVal()
                {
                    var theCookie=""+document.cookie;
                    var ind=theCookie.indexOf("CurrForumSearch");
                    if (ind==-1 || "CurrForumSearch"=="") return ""; 
                    var ind1=theCookie.indexOf(';',ind);
                    if (ind1==-1) ind1=theCookie.length; 
                    return unescape(theCookie.substring(ind+"CurrForumSearch".length+1,ind1));
                }	
                
                function SearchClicked()
                {
                    var searchTerm = $("#txtSearch").val().trim();
                    
                    var searchType = "";
                    if($("#rdoAll").is(":checked"))
                        searchType = "et";
                    else
                        searchType = "to";
                        
                    var currPage;
                    if ($("#currSearchValue").html() == searchTerm && $("#currSearchType").html() == searchType)
                        currPage =  $("#currSearchPage").html();
                    else
                        currPage = "1";	
                                 
                    if (IsValid())
                        location.href = "Search.aspx?s="+ searchTerm +"&st=" + searchType + "&pg=" + currPage;
                }
                
                function IsValid()
                {
                    var retVal = true;
                    $("#spnSearch").html("");
                    
                    var searchVal = $("#txtSearch").val().trim();
                    
                    if(searchVal == "")
                    {
                        retVal = false;
                        $("#spnSearch").html("(Required)");
                    }
                    else if(searchVal.length < 3)
                    {
                        retVal = false;
                        $("#spnSearch").html("(Min. 3 Chars)");
                    }
                    
                    return retVal;
                }		
            </script>
            <!-- #include file="../includes/Footer-New.aspx" -->
        </div>
        <!--Main container ends here-->
    </div>
    <!--Outer div ends here-->
</body>
</html>
