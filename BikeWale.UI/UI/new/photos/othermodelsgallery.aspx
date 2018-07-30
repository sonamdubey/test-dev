<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.PhotoGallery.OtherModelsGallery" Trace="false" Debug="false" async="true"%>
<%--<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" src="/Controls/RepeaterPagerPhotoGallery.ascx" %>--%>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" Src="/controls/NoFollowPagerControl.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Data" %>
<div>
    <div class="sept-dashed">
        <div class="dtTable" id="tbl_res" style="overflow:hidden">
            <asp:Repeater ID="rptModelPhotos" runat="server" EnableViewState="false">
                <itemtemplate>        
                    <div class="img-placer">
	                    <a href="/<%#UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString()) %>-bikes/<%#DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName").ToString() %>/photos/">			
                        <div class="rollover-container">
                            <div style="padding-left:5px;">
			                    <h4 class="rollover-text"><%#DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName").ToString() %></h4>
                            </div>
	                    </div>	            
                        <%--<div class="imgBox"><img style="border:1px solid #e5e5e5;" src='<%#  ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem,"ImagePathLarge").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString())%>' width="160" height="88"/></div>--%>
                            <div class="imgBox"><img style="border:1px solid #e5e5e5;" src='<%#  Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._160x89)%>' width="160" height="88"/></div>
	                    </a>
                    </div>
                <!--div style="padding:10px;float:left;"></div-->
                </itemtemplate>
            </asp:Repeater>  
        </div>
    </div>
    <div class="sept-dashed" style="padding-bottom:10px;" runat="server" id="divPager">
         <div class="dgNavDivTop right-float">
            <BikeWale:RepeaterPager id="spanPager" runat="server"/>   
        </div>
        <div class="clear"></div>
   </div>
</div>

