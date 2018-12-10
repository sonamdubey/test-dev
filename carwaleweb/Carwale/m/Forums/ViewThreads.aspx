<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.ViewThreads"  AutoEventWireup="false" trace="false" %>
<%@ Register TagPrefix="uc" TagName="PageThreads" src="/m/Controls/PageThreads.ascx" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    Title = subCatName + " | Car Forums - CarWale";
    Keywords = "";
    Description = "CarWale View forums section involves Discussion / Thread  realated to Finance/Loan , Insurance , latest happenings in the Indian car industry and many more";
    Canonical = "https://www.carwale.com/forums/" + pageUrl + "/";
    MenuIndex = "7";
    if(Convert.ToInt32(pageNo) != 1)
        PrevPageUrl = "https://www.carwale.com/forums/" + pageUrl +"-p" + Convert.ToString(Convert.ToInt32(pageNo)-1);
    if (Convert.ToInt32(pageNo) != totalPages && totalPages != 0)
        NextPageUrl = "https://www.carwale.com/forums/" + pageUrl + "-p" + Convert.ToString(Convert.ToInt32(pageNo) + 1);
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);

%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style>
        .arr-small {
    color: red;
    font-size: 15px;
    }
        .arr-big {
    color: red;
    font-size: 25px;
}
</style>
</head>

<body class="<%= (showExperimentalColor ? "btn-abtest" : "")%> m-special-skin-body m-no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	    <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12">
            <div id="br-cr" class="margin-top10 margin-bottom10"><a href="/m/forums/" class="normal m-special-skin-text">Forums</a></div>
            <h1 class="pgsubhead margin-bottom10 m-special-skin-text"><%=subCatName%></h1>
            <div class="new-line5 margin-left5 m-special-skin-text"><%=subCatDesc%></div>
            <div class="margin-top10 margin-bottom10">
                <div class="darkgray" style="margin-top:7px;">
                <a class="normal" href = "/m/forums/createthread.aspx?forum=<%=forumId%>">
                    <div class="btn btn-xs btn-orange rightfloat">Create New Thread</div>
                </a>
                <div style="clear:both;"></div>
            </div>
            </div>
            <div id="pagesContainer">
                <asp:Repeater id="rptStickyThreads" runat="server">
                <itemtemplate>
                <a class="normal" href='/m/forums/<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>-<%# DataBinder.Eval(Container.DataItem, "Url").ToString() %>.html'  style="text-decoration:none">
                <div class="box content-inner-block-10 content-box-shadow rounded-corner2 text-black margin-bottom10">
                    <div class="sub-heading">
                        <span style="color:black;">Sticky :</span> <%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                    </div>
                    <div class="lightgray new-line">
                        by <%# DataBinder.Eval(Container.DataItem, "HandleName").ToString() %>
                    </div>
                    <div class="lightgray new-line">
                        <%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %> views | <%# Convert.ToString(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Replies").ToString())-1) %> replies
                    </div>
                </div>
                </a>
                </itemtemplate>
                </asp:Repeater>
                <div type="page" id="page1">
                    <uc:PageThreads id="ucPageThreads" runat="server" />	
                </div>
            </div>
            
            <div class="darkgray margin-top10 margin-bottom10">
                <a class="normal" href = "/m/forums/createthread.aspx?forum=<%=forumId%>">
                    <div class="btn btn-xs btn-orange rightfloat">Create New Thread</div>
                </a>
                <div style="clear:both;"></div>
            </div>
            
            <div class="new-line5 f-14">
                <table style="width:100%;" cellspacing="0" cellpadding="0" border="0" class="m-special-skin-text">
                    <tr>
                        <td style="width:60px;">
                            <%if(Convert.ToInt32(pageNo) != 1){%>
                            <span style="position:relative;top:-4px;" onclick="PrevClicked()" class="m-special-skin-text"><span class="arr-big">&laquo;</span>&nbsp;Prev</span>
                            <%}%>
                        </td>
                        <td style="text-align:center;">
                            <%if (totalPages > 1){%>
                            Page  
                            <select id="ddlPages" onChange="PageChanged(this)" data-role="none">
                                <%for(int i=1; i<=totalPages; i++){%>
                                <option value="<%=i%>"><%=i%></option>
                                <%}%>
                            </select>
                            of <%=totalPages.ToString()%>
                            <%}%>
                        </td>
                        <td style="width:60px;">
                            <%if(Convert.ToInt32(pageNo) != totalPages && totalPages != 0){%>
                            <span style="position:relative;top:-4px;" onclick="NextClicked()" class="m-special-skin-text">Next<span class="arr-big">&nbsp;&raquo;</span></span>
                            <%}%>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div id="divLoader" style="position:absolute;width:100%;background-color:white;top:0;left:0;filter:alpha(opacity=70);opacity:0.70;display:none;">&nbsp;</div>
            
            </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script language="javascript" type="text/javascript">
        Common.showCityPopup = false;
        $(document).ready(function () {
            $("#ddlPages").val(<%=pageNo%>);
                });

                function PageChanged(ddlPage) {
                    location.href = "/m/forums/<%=pageUrl%>-p" + $(ddlPage).val() + "/";
                }

                function LoadPage() {
                    divNewPage.load("/m/forums/PagewiseThreads.aspx?forum=<%=forumId%>&page=" + selPage);
                }

                /*function ThreadClicked(threadId) {
                    location.href = "/m/forums/viewposts.aspx?thread=" + threadId;
                }*/

                function CreateThread() {
                    location.href = "/m/forums/createthread.aspx?forum=<%=forumId%>";
                }

                function PrevClicked() {
                    location.href = "/m/forums/<%=pageUrl%>-p<%=Convert.ToString(Convert.ToInt32(pageNo)-1)%>";
                }

                function NextClicked() {
                    location.href = "/m/forums/<%=pageUrl%>-p<%=Convert.ToString(Convert.ToInt32(pageNo)+1)%>";
                }

            </script>

</body>
</html>
