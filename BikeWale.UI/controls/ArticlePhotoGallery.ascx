<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticlePhotoGallery.ascx.cs" Inherits="Bikewale.Controls.ArticlePhotoGallery" %>
<style>
    #colorbox   { width:700px !important; height:450px !important; }
    .article-jcarousel-wrapper, .article-jcarousel { width: 618px; }
    .article-jcarousel li { width:200px; height:110px; overflow:hidden;}
    .article-jcarousel-wrapper .jcarousel-control-left { left:-12px; }
    .article-jcarousel-wrapper .jcarousel-control-right { right:-12px; }
    .article-jcarousel-wrapper .jcarousel-control-left, .article-jcarousel-wrapper .jcarousel-control-right { top:18%; }
   
</style>
<div class="border-light" id="ctrlNewsPhotoGallery_taggedPhotogallery">
    <div class="content-block-white">
        <h2 class="content-inner-block padding5" style="border-bottom:1px solid #e9e9e9;">Gallery</h2>
        <div id="gallery" class="white-shadow content-inner-block margin-top10" >
            <div class="jcarousel-wrapper article-jcarousel-wrapper">
                <div class="jcarousel article-jcarousel">
                    
                         <asp:Repeater ID="rptPhotos" runat="server">
                            <headertemplate>
                                <ul id="image-gallery">                
                            </headertemplate>
                                <itemtemplate>
                                    <li>
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
                    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive"></a></span>
                    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    <div class="clear"></div>
                 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    var ucHome_prev = $('.jcarousel-prev');
    var ucHome_next = $('.jcarousel-next');
    

    $(document).ready(function () {
        $("#image-gallery").jcarousel({ scroll: 4, initCallback: initCallbackUC, buttonNextHTML: null, buttonPrevHTML: null });

        
       bindCarouselEvents();

       if ($('#image-gallery').find('img').length <= 4) {
           ucHome_next.addClass("disabled");
       }

        $(".cboxElement").colorbox({
            rel: 'cboxElement'
        });
    });

    function bindCarouselEvents() {
        
        ucHome_prev.click(function () {
            var carouselUC = $('#image-gallery').data('jcarousel');
            carouselUC.prev();
            if (carouselUC.first == 1)
                ucHome_prev.addClass("disabled");
            ucHome_next.removeClass("disabled");
        });

        ucHome_next.click(function () {
            var carouselUC = $('#image-gallery').data('jcarousel');
            carouselButtonBehaviour(carouselUC, ucHome_prev, ucHome_next);
            carouselUC.next();
        });
    }

    function initCallbackUC(carousel) {
        $('.jcarousel-next').click(function () {
            carouselButtonBehaviour(carouselUC, ucHome_prev, ucHome_next);
            carouselUC.next();
        });

        $('.jcarousel-prev').click(function () {
            carouselUC.prev();
            if (carouselUC.first == 1)
                ucHome_prev.addClass("disabled");
            ucHome_next.removeClass("disabled");
        });
    }
</script>