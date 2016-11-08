<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikeMin" %>

<h2 class="font18 text-center margin-top20 margin-bottom10">Popular Comparisons</h2>

<div class="content-box-shadow grid-12">
    <ul class="compare-bikes-list">
        <li>
            <a href="" title="Compare Bajaj Avenger 150 Street vs Benelli TNT 25">
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd3.aeplcdn.com//210x118//bw/models/bajaj-avenger-150-street.jpg?20152710145912" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5 padding-right10">Bajaj Avenger 150 Street</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd4.aeplcdn.com//210x118//bw/models/benelli-tnt25.jpg?20152112153904" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5">Benelli TNT 25</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <li>
            <a href="" title="Compare Bajaj Avenger 150 Street vs Benelli TNT 25">
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd3.aeplcdn.com//210x118//bw/models/bajaj-avenger-150-street.jpg?20152710145912" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5 padding-right10">Bajaj Avenger 150 Street</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd4.aeplcdn.com//210x118//bw/models/benelli-tnt25.jpg?20152112153904" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5">Benelli TNT 25</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
        <li>
            <a href="" title="Compare Bajaj Avenger 150 Street vs Benelli TNT 25">
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd3.aeplcdn.com//210x118//bw/models/bajaj-avenger-150-street.jpg?20152710145912" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5 padding-right10">Bajaj Avenger 150 Street</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="grid-6">
                    <div class="comparison-image">
                        <img class="lazy" data-original="http://imgd4.aeplcdn.com//210x118//bw/models/benelli-tnt25.jpg?20152112153904" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                    </div>
                    <h3 class="font14 text-black margin-bottom5">Benelli TNT 25</h3>
                    <div class="text-default text-bold">
                        <span class="bwmsprite inr-xxsm-icon"></span><span class="font14">99,999 onwards</span>
                    </div>
                </div>
                <div class="clear"></div>
            </a>
        </li>
    </ul>
    <%--<asp:Repeater ID="rptCompareList" runat="server">
        <ItemTemplate>
            <a href="/m/comparebikes/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName1")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName1")%>-vs-<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName2")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName2")%>/?bike1=<%# DataBinder.Eval(Container.DataItem,"VersionId1")%>&bike2=<%# DataBinder.Eval(Container.DataItem,"VersionId2")%>" class="">
                <div class="">
                    <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._640x348) %>" />
                </div>
                
                <div style="text-align: center;"><%# DataBinder.Eval( Container.DataItem, "Bike1" ) %><%# DataBinder.Eval( Container.DataItem, "Bike2" ) %></div>
            </a>
        </ItemTemplate>
    </asp:Repeater>--%>
</div>