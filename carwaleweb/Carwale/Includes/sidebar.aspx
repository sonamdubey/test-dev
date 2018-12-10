<%@ Register TagPrefix="uc" TagName="UpcomingCars" src="/Controls/UpcomingCars.ascx" %>
<style>
    .pitstop-sidebar ul li ul li { background: transparent url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat 2px; padding: 2px 0 5px 9px;}
</style>
<script language="c#" runat="server"></script>
		<%= Carwale.UI.HtmlHelpers.Helpers.GetAdBar("1396440332273", 300, 250, 20, 20, false, 0) %>
    <div class="content-box-shadow content-inner-block-10 margin-bottom20">
        <h2 class="hd2">Upcoming Cars</h2>
        <p class="text-grey">Upcoming cars, their specifications, features, estimated prices and launch date.</p>
            <uc:UpcomingCars ID="UpcomingCars1" runat="server" VerticalDisplay="true" LoadStatic="true" TopCount="3"/>
    </div>
		<%= Carwale.UI.HtmlHelpers.Helpers.GetAdBar("1396440332273", 300, 250, 0, 20, false, 1) %>
	<div class="content-box-shadow content-inner-block-10 margin-bottom20 pitstop-sidebar">
		<ul>
			<li>
      			<!-- Put 300x250 Ad Code Here -->
    		</li>
			<li>
				<h2>Stay Connected</h2>
				<ul>
					<div style="padding:10px 0 10px 5px;">
						<div style="padding:3px;"> <a target="_blank" href="https://www.carwale.com/news/feed/"><img border="0" align="absmiddle" src="https://img.carwale.com/icons/rss.png"> Subscribe to RSS Feed</a></div>
						<div style="padding:3px;"> <a target="_blank" href="http://twitter.com/CarWale/"><img border="0" align="absmiddle" src="https://img.carwale.com/icons/twitter.png"> Follow CarWale on Twitter</a></div>
						<div style="padding:3px;"> <a target="_blank" href="http://www.facebook.com/CarWale/"><img border="0" align="absmiddle" src="https://img.carwale.com/icons/fb.png"> CarWale Page on Facebook</a></div>
					</div>
				</ul>
			</li>	
			<!-- li class="categories">
				<h2>Categories</h2>
				<ul>	
					<li class="cat-item cat-item-730"><a href="http://server/news-old/category/accessories-components" title="View all posts filed under Accessories &amp; Components">Accessories &amp; Components</a> (11)</li>
					<li class="cat-item cat-item-27"><a href="http://server/news-old/category/alternate-fuel" title="View all posts filed under Alternate Fuel">Alternate Fuel</a> (33)</li>
				</ul>
			</li -->	
			<li>
				<h2>Research Cars in India</h2>
				<div class="content-inner-block-10 margin-bottom10">
					<ul class="leftfloat grid-6">
						<li><a href="/audi-cars/">Audi</a></li>
						<li><a href="/bmw-cars/">BMW</a></li>
						<li><a href="/chevrolet-cars/">Chevrolet</a></li>
						<li><a href="/fiat-cars/">Fiat</a></li>
						<li><a href="/ford-cars/">Ford</a></li>
						<li><a href="/honda-cars/">Honda</a></li>
						<li><a href="/hyundai-cars/">Hyundai</a></li>
						<li><a href="/mahindra-cars/">Mahindra</a></li>
					</ul>
					<ul class="rightfloat grid-6">
						<li><a href="/marutisuzuki-cars/">Maruti Suzuki</a></li>
						<li><a href="/mercedesbenz-cars/">Mercedes-Benz</a></li>
						<li><a href="/mitsubishi-cars/">Mitsubishi</a></li>
						<li><a href="/nissan-cars/">Nissan</a></li>
						<li><a href="/skoda-cars/">Skoda</a></li>
						<li><a href="/tata-cars/">Tata</a></li>
						<li><a href="/toyota-cars/">Toyota</a></li>
						<li><a href="/volkswagen-cars/">Volkswagen</a></li>
					</ul>
					<div class="clear" style="float:right"><a href="/">All Cars in India &raquo;</a></div>					
				</div>
				<p>&nbsp;</p>
			</li>
			<li>
				<h2>Popular Cars in India</h2>
				<div class="content-inner-block-10 margin-bottom10">
					<ul class="grid-6 leftfloat">	
						<li><a title='Ford Ecosport in India' href='/ford-cars/ecosport/'>Ford EcoSport</a></li>
						<li><a title='Honda Amaze in India' href='/honda-cars/amaze/'>Honda Amaze</a></li>
						<li><a title='Renault Duster in India' href='/renault-cars/duster/'>Renault Duster</a></li>
						<li><a title='Maruti Ertiga in India' href='/marutisuzuki-cars/ertiga/'>Maruti Ertiga</a></li>
						<li><a title='Hyundai Eon in India' href='/hyundai-cars/eon/'> Hyundai Eon</a></li>
						<li><a title='Maruti Alto 800 in India' href='/marutisuzuki-cars/alto800/'>Maruti Alto 800</a></li>
						<li><a title='Maruti Wagon R in India' href='/marutisuzuki-cars/wagonr10/'>Maruti Wagon R</a></li>
						<li><a title='Hyundai Santro Xing in India' href='/hyundai-cars/santroxing/'>Hyundai Santro Xing</a></li>
						<li><a title='Audi Q3 A-Star in India' href='/audi-cars/q3/'>Audi Q3</a></li>
						<li><a title='Mahindra Bolero in India' href='/mahindra-cars/bolero/'>Mahindra Bolero</a></li>
					</ul>
					<ul class="grid-6 rightfloat">
						<li><a title='Honda City in India' href='/honda-cars/city/'>Honda City</a></li>
						<li><a title='Maruti DZire in India' href='/marutisuzuki-cars/swiftdzire/'>Maruti DZire</a></li>
						<li><a title='Hyundai i10 in India' href='/hyundai-cars/i10/'>Hyundai i10</a></li>
						<li><a title='Hyundai i20 in India' href='/hyundai-cars/i20/'>Hyundai i20</a></li>
						<li><a title='Hyundai Verna in India' href='/hyundai-cars/verna/'>Hyundai Verna</a></li>
						<li><a title='Mahindra Scorpio in India' href='/mahindra-cars/scorpio/'>Mahindra Scorpio</a></li>
                        <li><a title='Toyota Innova in India' href='/toyota-cars/innova/'>Toyota Innova</a></li>
                        <li><a title='Maruti Suzuki Swift in India' href='/marutisuzuki-cars/swift/'>Maruti Swift</a></li>
						<%--<li><a title='Skoda Laura in India' href='/skoda-cars/laura/'>Skoda Laura</a></li>--%>
						<li><a title='Tata Indica Vista in India' href='/tata-cars/indicavista/'>Tata Indica Vista</a></li>
						<li><a title='Toyota Etios Altis in India' href='/toyota-cars/etios/'>Toyota Etios</a></li>
					</ul>
                    <div class="clear"></div>
				</div>
			</li>
			<p>&nbsp;</p>
		</ul>
   </div>