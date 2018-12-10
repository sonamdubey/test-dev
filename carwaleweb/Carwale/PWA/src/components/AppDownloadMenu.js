import React from 'react'
import PropTypes from 'prop-types'

import {
	APP_STORE_LINK,
	PLAY_STORE_FOOTER_LINK,
	PLAY_STORE_NAV_DRAWER_LINK
} from '../constants'

const propTypes = {
	// app download menu placement flag
	navDrawer: PropTypes.bool,
	// custom class
	menuClass: PropTypes.string
}

const defaultProps = {
	navDrawer: false,
	menuClass: '',
	appStoreLink: APP_STORE_LINK,
	playStoreFooterLink: PLAY_STORE_FOOTER_LINK,
	playStoreNavDrawerLink: PLAY_STORE_NAV_DRAWER_LINK
}

class AppDownloadMenu extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		let playStoreLink

		if (this.props.navDrawer) {
			playStoreLink = this.props.playStoreNavDrawerLink
		}
		else {
			playStoreLink = this.props.playStoreFooterLink
		}

		return (
			<div className={"app-download__content " + this.props.menuClass}>
				<p className="app-content__title">Download Mobile App</p>
				<ul className="app-link__list">
					<li className="app-list__item">
						<a href={this.props.appStoreLink} title="Download App" target="_blank" className="app-store-icon app-logo__item"></a>
					</li>
					<li className="app-list__item">
						<a href={playStoreLink} title="Download App" target="_blank" className="play-store-icon app-logo__item"></a>
					</li>
				</ul>
			</div>
		)
	}
}

AppDownloadMenu.propTypes = propTypes
AppDownloadMenu.defaultProps = defaultProps

export default AppDownloadMenu
