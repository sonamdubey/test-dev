<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.UsedBikeSearch" %>
<div class="margin-top40 margin-left10" style="border:1px solid #E2E2E2;">                
    <div>
        <h2 class="left-float margin-top15">Used Bikes By</h2>  
        <ul class="tabs-container left-float margin-top10" id="tab-list">                
            <li><a id="city" class="active-tab first" show_content="browseByCity">City</a></li>
            <li><a id="make" show_content="browseByMake">Make</a></li>
        </ul>
            <div class="clear"></div>
    </div>
    <div class="clear"></div>
    <div class="margin-top10 margin-bottom10 margin-right5  grey-text">
        <div id="browseByCity">
            <div class="col"><a href="/used/bikes-in-bangalore/">Bangalore</a></div>
            <div class="col"><a href="/used/bikes-in-mumbai/">Mumbai</a></div>
            <div class="col"><a href="/used/bikes-in-newdelhi/">New Delhi</a></div>
            <div class="col"><a href="/used/bikes-in-chennai/">Chennai</a></div>
            <div class="col"><a href="/used/bikes-in-pune/">Pune</a></div>
            <div class="col"><a href="/used/bikes-in-hyderabad/">Hyderabad</a></div>
            <div class="clear"></div>
            <div class="col"><a href="/used/bikes-in-ahmedabad/">Ahmedabad</a></div>
            <div class="col"><a href="/used/bikes-in-coimbatore/">Coimbatore</a></div>                  
            <div class="col"><a href="/used/bikes-in-gurgaon/">Gurgaon</a></div>
            <div class="col"><a href="/used/bikes-in-kolkata/">Kolkata</a></div>
            <div class="col"><a href="/used/bikes-in-faridabad/">Faridabad</a></div>
            <div class="col"><a href="/used/bikes-in-lucknow/">Lucknow</a></div>
            <div class="clear"></div>
            <div>&nbsp;</div>
            <a href="/used/bikesincity.aspx" class="right-float margin-right5"><b>More cities</b></a>
            <div class="clear"></div>
        </div>
        <div id="browseByMake" class="hide">
            <div class="col"><a href="/used/bajaj-bikes-in-india/">Bajaj</a></div>
            <div class="col"><a href="/used/hero-bikes-in-india/">Hero</a></div>
            <div class="col"><a href="/used/honda-bikes-in-india/">Honda</a></div>
            <div class="col"><a href="/used/yamaha-bikes-in-india/">Yamaha</a></div>      
            <div class="col"><a href="/used/royalenfield-bikes-in-india/">Royal Enfield</a></div>
            <div class="col"><a href="/used/tvs-bikes-in-india/">TVS</a></div>
            <div class="clear"></div>
            <div class="col"><a href="/used/suzuki-bikes-in-india/">Suzuki</a></div>
            <div class="col"><a href="/used/herohonda-bikes-in-india/">Hero Honda</a></div>
            <div class="col"><a href="/used/mahindra-bikes-in-india/">Mahindra</a></div>
            <div class="col"><a href="/used/kawasaki-bikes-in-india/">Kawasaki</a></div>
            <div class="col"><a href="/used/ktm-bikes-in-india/">KTM</a></div>
            <div class="col"><a href="/used/hyosung-bikes-in-india/">Hyosung</a></div>
            <div class="clear"></div>
            <div>&nbsp;</div>
            <a href="/used/bikesbymake.aspx" class="right-float margin-right5"><b>More makes</b></a>
            <div class="clear"></div>
        </div>
        <div id="browseByModel" class="hide">
            <ul class="ul-hrz margin-top10">
                <li><a href="/used/aprilia-mana850abs-bikes-in-india/">Aprilia Mana 850</a></li>
                <li><a href="/used/bajaj-platina-bikes-in-india/">Bajaj Platina</a></li>
                <li><a href="/used/honda-activai-bikes-in-india/">Honda Activa I</a></li>
                <li><a href="/used/hero-splendorplus-bikes-in-india/">Hero Splendor Plus</a></li>
                <li><a href="/used/harleydavidson-street-750-bikes-in-india/">Harley Davidson Street 750</a></li>
                <li><a href="/used/kawasaki-ninja300-bikes-in-india/">kawasaki Ninja 300</a></li>
                <li><a href="/used/royalenfield-bullet350twinspark-bikes-in-india/">Royal Enfield Bullet 350 Twinspark</a></li>
                <li><a href="/used/hyosung-gt250r-bikes-in-india/">Hyosung GT250R </a></li>
                <li><a href="/used/ktm-duke200-bikes-in-india/">KTM Duke 200</a></li>
                <li><a href="/used/yamaha-ray-bikes-in-india/">Yamaha Ray</a></li>
                <li><a href="/used/ducati-1199panigale-bikes-in-india/">Ducati 1199 Panigale</a></li>
                <li><a href="/used/triumph-bonneville-bikes-in-india/">Triumph Bonneville</a></li>
            </ul>
            <div class="clear"></div>
            <div>&nbsp;</div>
            <a href="/used/bikesbymodel.aspx">More Models</a>
            <div class="clear"></div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#tab-list li a").click(function (){
            var showContent = $(this).attr("show_content");
            $("#" + showContent).siblings().removeClass("show").addClass("hide");
            if (!$(this).hasClass("active-tab")) {
                $(this).addClass("active-tab").parent().siblings().find("a").removeClass("active-tab");
            }
            if (!$("#" + showContent).hasClass("show")) {
               $("#" + showContent).removeClass("hide").addClass("show");
            }
        });
    });
</script>