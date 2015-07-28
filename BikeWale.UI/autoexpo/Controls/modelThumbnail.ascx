<%@ Control Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.ModelThumbnails" %>
<div style="width:645px;margin:10px 0 0 0;">
    <ul id="slides1" style="margin-left:30px;">
        <asp:Repeater id="rptThumbnail"  runat="server">
            <ItemTemplate>
	            <li>
                    <asp:Repeater ID="rptImages" DataSource='<%# GetImages(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Item"))) %>'  runat="server">
                        <ItemTemplate>
                            <div class="scroll-img">
                                <%# "<a class='cbBox"+DataBinder.Eval(Container.DataItem,"BasicId")+"' rel='slide"+DataBinder.Eval(Container.DataItem,"BasicId")+"' href='http://"+Eval("HostUrl").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathLarge").ToString()+"'><img class='alignleft size-thumbnail img-border-news' src= 'http://"+Eval("HostUrl").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathThumbnail").ToString()+"'  align='right' border='0' /></a>"%>
                                <%--<%# "<a class='cbox2012' rel='group4' href='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/l/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg'><img class='alignleft size-thumbnail img-border-news' src='" + AutoExpo.Common.ImagingFunctions.GetImagePath("/ec/", DataBinder.Eval(Container.DataItem, "HostURL").ToString()) + DataBinder.Eval(Container.DataItem, "BasicId") + "/img/t/" + DataBinder.Eval(Container.DataItem, "Id") + ".jpg' align='right' border='0' /></a>" %>--%>                                
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ItemTemplate>
        </asp:Repeater>               	        
    </ul>
</div>
<script type="text/javascript" src="/autoexpo/js/slider.js"></script>
<script type="text/javascript" language="javascript">
    $('#slides1').bxSlider({
        controls: true,
        wrapper_class: 'slides1_wrap',
        auto: false,
        auto_controls: true,
        prev_image: '/autoexpo/images/arrow-left.gif',
        next_image: '/autoexpo/images/arrow-right.gif',
        auto_hover: false
    });
    $("a.cbox2012").colorbox({ rel: 'group4', current: '' }); 
</script> 