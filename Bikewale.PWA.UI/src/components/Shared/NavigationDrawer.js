import React from 'react'
import {Link} from 'react-router-dom'
import {withRouter} from 'react-router-dom'
import {pushNavMenuAnalytics, triggerGA} from '../../utils/analyticsUtils'

class NavigationDrawer extends React.Component {
    constructor(props) {
        super(props);
        this.pushAnalytics = this.pushAnalytics.bind(this);
        this.toggleNestedNav = this.toggleNestedNav.bind(this);
        this.pushAnalytics = this.pushAnalytics.bind(this);
        this.pushAnalyticsAndCloseDrawer = this.pushAnalyticsAndCloseDrawer.bind(this);
        this.renderNestedListItem = this.renderNestedListItem.bind(this);
        this.renderNestedListItemWithLinkTag = this.renderNestedListItemWithLinkTag.bind(this);
      
    }
    removeActiveItemClassName(element) {
        if(element.classList.contains('item-active'))
            element.className = ''
        else
            element.className = 'item-active'

    }
    toggleNestedNav(event) {
        let listItem = event.currentTarget.parentElement
        this.removeActiveItemClassName(listItem);
    }

    closeNavDrawer(event) {
        document.body.className = ''
    }

    closeNestedNavigation() {
        var element = document.getElementById('nav-drawer').getElementsByClassName('item-active');
        if(element && element[0]) {
            this.removeActiveItemClassName(element[0]);
        }
    }

    pushAnalytics(label) {
        if (typeof label === "string") {
            pushNavMenuAnalytics(label);
        }
        else {
            pushNavMenuAnalytics(event.currentTarget.innerHTML);
        }
    }

    pushAnalyticsAndCloseDrawer(label) {
        if (typeof label === "string") {
            pushNavMenuAnalytics(label);
        }
        else {
            pushNavMenuAnalytics(event.currentTarget.innerHTML);
        }
        if (event.target.nodeName) {
            this.closeNestedNavigation();
            this.closeNavDrawer();
        }
    }

    renderNestedListItemWithLinkTag(link,text) {
        return(
            <li>
                <Link to={link} onClick={this.pushAnalyticsAndCloseDrawer}>{text}</Link>
            </li>
        )
    }
    renderNestedListItem(link,text) {
        return(
            <li>
                <a href={link} onClick={this.pushAnalyticsAndCloseDrawer}>{text}</a>
            </li>
        )
    }
    render() {
     
        return (
            <div>
                <nav id="nav-drawer" className="transition-ease">
                    <ul className="nav-drawer-list padding-top10">
                        <li>
                            <a href="/m/" className="nav-item" onClick={this.pushAnalyticsAndCloseDrawer.bind(this,"Home")}>
                                <span className="bwmsprite home-icon"></span>
                                <span>Home</span>
                            </a>
                        </li>
                        <li>
                            <div className="nav-item" onClick={this.toggleNestedNav}>
                                <div onClick={this.pushAnalytics.bind(this, "New Bikes")}>
                                    <span className="bwmsprite newBikes-icon"></span>
                                    <span>New Bikes</span>
                                    <span className="bwmsprite fa-angle-down"></span>
                                </div>
                            </div>
                            <ul className="nested-nav-list">
                                {this.renderNestedListItem("/m/new-bikes-in-india/","Find New Bikes")}
                                {this.renderNestedListItem("/m/comparebikes/" ,"Compare Bikes")}
                                {this.renderNestedListItem("/m/pricequote/","Check On-Road Price")}
                                
                                {this.renderNestedListItem("/m/dealer-showrooms/","Locate Dealer")}
                                {this.renderNestedListItem("/m/service-centers/","Locate Service Center")}
                                {this.renderNestedListItem("/m/upcoming-bikes/" ,"Upcoming Bikes")}
                                {this.renderNestedListItem("/m/new-bike-launches/","New Launches")}
                                {this.renderNestedListItem("/m/electric-bikes/","Electric Bikes")}
                                {this.renderNestedListItem("/m/bikebooking/","Book Your Bike")}
                                
                            </ul>
                        </li>
                        <li>
                            <div className="nav-item" onClick={this.toggleNestedNav}>
                                <div onClick={this.pushAnalytics.bind(this, "New Scooters")}> 
                                    <span className="bwmsprite scooter-icon"></span>
                                    <span>New Scooters</span>
                                    <span className="bwmsprite fa-angle-down"></span>
                                </div>
                            </div>
                            <ul className="nested-nav-list">
                                {this.renderNestedListItem("/m/honda-scooters/","Honda Scooters")}
                                {this.renderNestedListItem("/m/hero-scooters/" ,"Hero Scooters")}
                                {this.renderNestedListItem("/m/tvs-scooters/","TVS Scooters")}
                                {this.renderNestedListItem("/m/yamaha-scooters/","Yamaha Scooters")}
                                {this.renderNestedListItem("/m/scooters/","All Scooters")}
                            </ul>
                        </li>
                        <li>
                            <div className="nav-item" onClick={this.toggleNestedNav}>
                                <div onClick={this.pushAnalytics.bind(this, "Buy & Sell Used Bikes")}>
                                    <span className="bwmsprite usedBikes-icon"></span>
                                    <span>Buy & Sell Used Bikes</span>
                                    <span className="bwmsprite fa-angle-down"></span>
                                </div>
                            </div>
                            <ul className="nested-nav-list">
                                {this.renderNestedListItem("/m/used/","Find Used Bikes")}
                                {this.renderNestedListItem("/m/used/bikes-in-india/","All Used Bikes")}
                                {this.renderNestedListItem("/m/used/sell/","Sell Your Bike")}
                            </ul>
                        </li>
                        <li>
                            <a href="/m/reviews/" className="nav-item"  onClick={this.pushAnalyticsAndCloseDrawer.bind(this, "Reviews")}>
                                <span className="bwmsprite reviews-icon"></span>
                                <span>Reviews</span>
                            </a>
                        </li>
                        <li>
                            <div className="nav-item" onClick={this.toggleNestedNav}>
                                <div onClick={this.pushAnalytics.bind(this, "News, Videos & Tips")}>
                                    <span className="bwmsprite news-icon"></span>
                                    <span>News, Videos & Tips</span>
                                    <span className="bwmsprite fa-angle-down"></span>
                                </div>
                            </div>
                            <ul className="nested-nav-list">
                                {this.renderNestedListItemWithLinkTag("/m/news/","News")}
                                {this.renderNestedListItemWithLinkTag("/m/expert-reviews/","Expert Reviews")}
                                {this.renderNestedListItem("/m/features/","Features")}
                                {this.renderNestedListItem("/m/bike-care/","Bike Care")}
                                {this.renderNestedListItemWithLinkTag("/m/bike-videos/","Videos")}
                                
                            </ul>
                        </li>
                        <li>
                            <a href="/featured/trackday-2018/" className="nav-item"  onClick={this.pushAnalyticsAndCloseDrawer.bind(this,"Track Day 2018")}>
                                <span className="bwmsprite track-day"></span>
                                <span>Track Day 2018</span>
                            </a>
                        </li>
                        <li>
                            <Link to="/m/bike-loan-emi-calculator/" className="nav-item" onClick={this.pushAnalyticsAndCloseDrawer.bind(this,"EMI Calculator")}>
								<span className="bwmsprite forum-icon"></span>
								<span>EMI Calculator</span>
							</Link>
                        </li>
                        <li>
                            <a href="/m/users/login.aspx" className="nav-item" onClick={this.pushAnalytics.bind()}>
                                <span className="bwmsprite myBikeWale-icon"></span>
                                <span>Login</span>
                            </a>
                        </li>
                    </ul>
                    <div id="nav-app-content" className="transition-ease">
                        <p className="font12 text-bold inline-block">India’s #1 Bike Research Destination</p>
                        <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DMobilesite%26utm_medium%3DDrawer%26utm_campaign=BikeWale%2520MobilesiteDrawer" target="_blank" className="btn btn-orange nav-app-install-btn text-bold inline-block" rel="noopener nofollow">Install</a>
                    </div>
                </nav>
                <div className="black-overlay" onClick={this.closeNavDrawer}></div>
            </div>
        )
    }
}

export default withRouter(NavigationDrawer)
