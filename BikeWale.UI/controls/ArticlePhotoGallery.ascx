<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticlePhotoGallery.ascx.cs" Inherits="Bikewale.Controls.ArticlePhotoGallery" %>
<div class="border-light" id="ctrlNewsPhotoGallery_taggedPhotogallery">
    <div class="content-block-white">
        <h2 class="content-inner-block padding5" style="border-bottom:1px solid #e9e9e9;">Gallery</h2>
        <div id="gallery" class="white-shadow content-inner-block margin-top10" >
            <div class=" jcarousel-skin-tango">
                <div class="jcarousel-container jcarousel-container-horizontal" style="position: relative; display: block;">
                    <div class="jcarousel-clip jcarousel-clip-horizontal" style="position: relative;" >
                         <asp:Repeater ID="rptPhotos" runat="server">
                            <headertemplate>
                                <ul class="jcarousel-list jcarousel-list-horizontal" id="image-gallery">                
                            </headertemplate>
                                <itemtemplate>
                                    <li class="jcarousel-item jcarousel-item-horizontal jcarousel-item-1 jcarousel-item-1-horizontal" jcarouselindex="1">
                                        <a rel="gallery" class="pics cboxElement" href="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._640x348)%>">
                                            <%--<img border="0" src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"ImagePathLarge").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString())%>' alt="<%#Eval("AltImageName") %>" title="<%#Eval("ImageTitle") %>" class="thumb">--%>
                                            <img border="0" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._640x348)%>' alt="<%#Eval("AltImageName") %>" title="<%#Eval("ImageTitle") %>" class="thumb">
                                        </a>
                                    </li>
                                </itemtemplate>
                            <footertemplate>
                                </ul>
                            </footertemplate>
                        </asp:Repeater>
                    </div>               
                    <div class="jcarousel-prev jcarousel-prev-horizontal jcarousel-prev-disabled jcarousel-prev-disabled-horizontal" style="display: block;" disabled="disabled"></div><div class="jcarousel-next jcarousel-next-horizontal" style="display: block;"></div>
                    <div class="clear"></div>
                 </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#image-gallery").jcarousel({ scroll: 4, initCallback: initCallbackUC, buttonNextHTML: null, buttonPrevHTML: null });
        bindCarouselEvents();


        $(".cboxElement").colorbox({
            rel: 'cboxElement'
        });
    });

    function bindCarouselEvents() {
        var ucHome_prev = $('#jcarousel-prev');
        var ucHome_next = $('#jcarousel-next');
        var carouselUC = $('#image-gallery').data('jcarousel');
        if (carouselUC.size() > 4) {
            ucHome_prev.click(function () {
                carouselUC.prev();
                if (carouselUC.first == 1)
                    ucHome_prev.addClass("disabled");
                ucHome_next.removeClass("disabled");
            });
            ucHome_next.click(function () {
                carouselButtonBehaviour(carouselUC, ucHome_prev, ucHome_next);
                carouselUC.next();
            });
        }
        else {
            ucHome_next.addClass("disabled");
        }
    }

    function initCallbackUC(carousel) {
        $('#jcarousel-next').click(function () {
            return false;
        });

        $('#jcarousel-prev').click(function () {
            return false;
        });
    }
</script>