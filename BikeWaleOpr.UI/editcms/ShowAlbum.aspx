<%@ Page AutoEventWireup="false" Inherits="BikeWaleOpr.EditCms.ShowAlbum" Language="C#" Trace="false" Debug="false" %>
<%@Import namespace="BikeWaleOpr.Common"%>
<%@Import namespace="BikeWaleOpr"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/common/common.css?V1.1" type="text/css" />
<title>BikeWale Operations</title>
</head>
<body>
<form runat="server">
	<%--<table border="0" width="835px;" cellpadding="2" cellspacing="2">--%>
		<%--<br />
		<div id="dvThumbNail" runat="server"><b><%= tnCount == 0 ? "" : "ThumbNail Photos" %></b><table id="tbThumbNail" runat="server"></table></div><br />
		<div id="dvMedium" runat="server"><b><%= mCount == 0 ? "" : "Medium Photos" %></b><table id="tbMedium" runat="server"></table></div><br />
		<div id="dvLarge" runat="server"><b></b><table id="tbLarge" runat="server"></table></div><br />		--%>
        <div>
            <asp:Repeater id="rpt_ShowPhotos" runat="server" >
                <ItemTemplate>
                    <div style="padding:5px; border:1px solid black; margin-top:10px;">                        
                        <img src="<%# ImagingOperations.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"ImagePathThumbnail").ToString(), DataBinder.Eval(Container.DataItem,"HostURL").ToString()) %>" alt="loading.."/>
                        <a onclick="javascript:window.open('ViewImage.aspx?hostUrl=<%# DataBinder.Eval(Container.DataItem,"HostURL").ToString()%>&imagepath=<%# DataBinder.Eval(Container.DataItem,"ImagePathLarge").ToString() %>','Large_Image','width=600px,height=600px,top=200px,left=600px')" style="cursor:pointer;Padding-left:35px;">View Large Image</a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>            
        </div>
	<%--</table>--%>

</form>


