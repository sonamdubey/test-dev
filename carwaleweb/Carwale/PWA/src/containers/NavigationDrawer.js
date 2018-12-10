import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import AppDownloadMenu from '../components/AppDownloadMenu'

import { closeNavDrawer } from '../actionCreators/Header'

import {
	lockScroll,
	unlockScroll
} from '../utils/ScrollLock'
import { fireInteractiveTracking } from '../utils/Analytics';
import Cookies  from 'js-cookie'
import { trackCustomData } from '../utils/cwTrackingPwa';
/**
 * This class representing the container component
 * for navigation drawer
 *
 * @class NavigationDrawer
 * @extends {React.Component}
 */
class NavigationDrawer extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount() {
		window.addEventListener('popstate', this.onPopState);
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.header.isNavDrawerActive) {
			lockScroll()
			window.history.pushState('navDrawer', '', '')
		}
	}

	trackMainMenuItemClick = (event) => {
		fireInteractiveTracking('Navigation-drawer-Mobile','Main-Menu-item-Click', event.currentTarget.text +' '+ window.location.href)
		trackCustomData('NavigationDrawerMobile', 'MainMenuItemClick',"text="+ event.currentTarget.text, false)
	}

	trackSubMenuItemClick = (event) => {
		fireInteractiveTracking('Navigation-drawer-Mobile','Sub-Menu-item-Click', event.currentTarget.text +' '+ window.location.href)
		trackCustomData('NavigationDrawerMobile', 'SubMenuItemClick',"text="+ event.currentTarget.text, false)
	}

	trackNcfClick = (event) => {
		fireInteractiveTracking('NCFLinkage','NCFSlug_click', 'mainMenu')
		trackCustomData('NCFLinkage', 'NCFSlugClick',"source=mainMenu", false)
	}

	toggleNestedNav = (event) => {
		let targetListItem = event.currentTarget.parentElement
		let activeListItem = this.refs.navList.querySelectorAll('.nav-list__item.item--active')[0]

		if (targetListItem.classList.contains('item--active')) {
			targetListItem.classList.remove('item--active')
		}
		else {
			if (activeListItem) {
				activeListItem.classList.remove('item--active')
			}

			targetListItem.classList.add('item--active')
			this.trackMainMenuItemClick(event)
		}
	}

	handleNavigationClose = () => {
		unlockScroll()
		this.props.closeNavDrawer()
	}

	onPopState = (state) => {
		this.handleNavigationClose()
	}

	render() {

		if (!this.props.header.isVisible) {
			return null
		}

		const activeClass = this.props.header.isNavDrawerActive ? 'nav--active' : ''
		return (
			<div ref="navigationDrawer" className={activeClass}>
				<nav id="navDrawer">
					<ul ref="navList" className="nav__list">
						<li className="nav-list__item">
							<a href="/m/" className="nav-list-item__target">
								<span className="home-icon"></span>
								<span className="nav-item__title">Home</span>
							</a>
						</li>
						<li className="nav-list__item">
							<a href="javascript:void(0);" className="nav-list-item__target newCarDropDown" onClick={this.toggleNestedNav} rel="nofollow">
								<span className="newCars-icon"></span>
								<span className="nav-item__title">New Cars</span>
								<span className="nav-arrow-icon"></span>
							</a>
							<ul className="nav__nested-list">
								<li>
                                    <a href="/m/new/" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>Find New Cars</a>
                                </li>
								<li>
									<a href="/m/advantage/" id="menuAdvantage" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>New Car Offers</a>
								</li>
								<li>
									<a href="/m/comparecars/" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>Compare Cars</a>
								</li>
								<li>
									<a href="/m/research/pricequote.aspx" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>Check On-Road Price</a>
								</li>
								<li>
									<a href="/m/research/locatedealerpopup.aspx" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>Find Dealer</a>
								</li>
								<li>
									<a href="/m/upcoming-cars/" className="nav-list-item__target" onClick={this.trackSubMenuItemClick}>Upcoming Cars</a>
								</li>
							</ul>
						</li>
						<li className="nav-list__item">
							<a href="javascript:void(0);" className="nav-list-item__target" onClick={this.toggleNestedNav} rel="nofollow">
								<span className="usedCars-icon"></span>
								<span className="nav-item__title">Used Cars</span>
								<span className="nav-arrow-icon"></span>
							</a>
							<ul className="nav__nested-list">
                                <li>
									<a href="/m/used/search.aspx" onClick={this.trackSubMenuItemClick}>All Used Cars</a>
								</li>
								<li>
									<a href="/m/used/" onClick={this.trackSubMenuItemClick}>Find Used Cars</a>
								</li>
								<li>
									<a href="/m/used/carvaluation/" onClick={this.trackSubMenuItemClick}>Check Car Valuation</a>
								</li>
								<li>
									<a href="/used/mylistings/search/" onClick={this.trackSubMenuItemClick}>My Listings</a>
								</li>
							</ul>
						</li>
						<li className="nav-list__item">
							<a href="/used/sell/" className="nav-list-item__target" onClick={this.trackMainMenuItemClick}>
								<span className="sellCars-icon"></span>
								<span className="nav-item__title">Sell Car</span>
							</a>
						</li>
						<li className="nav-list__item">
							<a href="javascript:void(0);" className="nav-list-item__target" onClick={this.toggleNestedNav} rel="nofollow">
								<span className="reviews-icon"></span>
								<span className="nav-item__title">Reviews and News</span>
								<span className="nav-arrow-icon"></span>
							</a>
							<ul className="nav__nested-list">
								<li>
									<a href="/m/news/" onClick={this.trackSubMenuItemClick}>News</a>
								</li>
								<li>
									<a href="/m/expert-reviews/" onClick={this.trackSubMenuItemClick}>Expert Reviews</a>
								</li>
								<li>
									<a href="/m/userreviews/" onClick={this.trackSubMenuItemClick}>User Reviews</a>
								</li>
								<li>
									<a href="/m/images/" onClick={this.trackSubMenuItemClick}>Images</a>
								</li>
								<li>
									<a href="/m/videos/" onClick={this.trackSubMenuItemClick}>Videos</a>
								</li>
								<li>
									<a href="/m/tipsadvice/" onClick={this.trackSubMenuItemClick}>Tips and Advice</a>
								</li>
								<li>
									<a href="/m/features/" onClick={this.trackSubMenuItemClick}>Special Reports</a>
								</li>
								<li>
									<a href="/m/forums/" onClick={this.trackSubMenuItemClick}>Forums</a>
								</li>
							</ul>
						</li>
						<li id="navSpecials" className="nav-list__item">
							<a href="javascript:void(0);"  className="nav-list-item__target" onClick={this.toggleNestedNav} rel="nofollow">
								<span className="specials-icon"></span>
								<span className="nav-item__title">Specials</span>
								<span className="nav-arrow-icon"></span>
							</a>
							<ul className="nav__nested-list">
								<li>
									<a href="/m/tyres/" onClick={this.trackSubMenuItemClick}>Car Tyres</a>
								</li>
								<li>
									<a href="/m/electriccars/" onClick={this.trackMainMenuItemClick}>All about Electric</a>
								</li>
								<li>
									<a href="/the-great-indian-hatchback-of-2016/" onClick={this.trackMainMenuItemClick}>Great Indian Hatchback</a>
								</li>
							</ul>
						</li>
						<li id="navInsuranceLink" className="nav-list__item" >
							<a href="/m/insurance/?utm=mnavigation/" className="nav-list-item__target" onClick={this.trackMainMenuItemClick}>
								<span className="insurance-icon"></span>
								<span className="nav-item__title">Insurance</span>
							</a>
						</li>
						<li className="nav-list__item">
							<a href="https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|form_page_oops&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired" onClick={this.trackMainMenuItemClick} rel="nofollow" className="nav-list-item__target" target="_blank">
								<span className="nav-loan-icon"></span>
								<span className="nav-item__title">Loans</span>
								<span className="sponsored-superscript-text">Ad</span>
							</a>
						</li>
						<li className="nav-list__item list-item__contact">
							<a href="tel:18002090230" className="nav-list-item__target" onClick={this.trackMainMenuItemClick}>
								<span className="nav-contact-icon"></span>
								<div className="nav-content__content">
									<span className="nav-item__title contact-item__number">1800 2090 230</span>
									<span className="contact-item__number-label">Toll free</span>
									<p className="contact-item__timing">Mon-Fri (8 AM - 8 PM) <br />Sat (8 AM - 5:30 PM)<br />Sun (9 AM - 7 PM)</p>
								</div>
							</a>
						</li>
						<li className="nav-list__item">
							<AppDownloadMenu menuClass="app-menu--nav-drawer" navDrawer={true} />
						</li>
					</ul>
				</nav>
				<div
					className="nav-blackout-window"
					onClick={this.handleNavigationClose}
				></div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		header
	} = state

	return {
		header
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		closeNavDrawer: bindActionCreators(closeNavDrawer, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(NavigationDrawer)
