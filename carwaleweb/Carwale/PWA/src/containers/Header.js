import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { openNavDrawer } from '../actionCreators/Header'

import { fireInteractiveTracking } from '../utils/Analytics'
import { trackCustomData } from '../utils/cwTrackingPwa';

/**
 * This class representing the container component
 * for header
 *
 * @class Header
 * @extends {React.Component}
 */

const propTypes = {
	// this is react element of childern that you want to render in header (right side)
	rightChildern: PropTypes.element,
	// to make header sticky
	isSticky: PropTypes.bool
}

const defaultProps = {
	rightChildern: null,
	isSticky: false
}

class Header extends React.Component {
	constructor(props) {
		super(props)
		this.lastScrollPosition = 0
	}

	handleNavClick = () => {
		this.props.openNavDrawer()
		this.trackEvent("Hamburger-Icon-Click ")
	}

	trackEvent = (action) => {
		fireInteractiveTracking("TopMenu-Mobile", action,  window.location.href)
		trackCustomData("TopMenuMobile", action.split('-').join(''), "", false)
	}

	render() {
		return (this.props.isVisible ?
			<header id="header" className={this.props.isSticky ? "header--fixed" : ""} >
				<div className="header__content">
					<div className="header__left-content">
						<span className="navbar-btn hamburger-icon" onClick={this.handleNavClick}></span>
						<a href="/m/" className="cw-logo-color" onClick={this.trackEvent.bind(this, "CarwaleLogo-Click")}></a>
					</div>
					<div className="header__right-content">
						{this.props.rightChildern}
					</div>
				</div>
			</header> : null
		)
	}
}

const mapStateToProps = (state) => {
	const {
		isVisible
	} = state.header

	return {
		isVisible
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		openNavDrawer: bindActionCreators(openNavDrawer, dispatch)
	}
}

Header.propTypes = propTypes
Header.defaultProps = defaultProps

export default connect(mapStateToProps, mapDispatchToProps) (Header)
