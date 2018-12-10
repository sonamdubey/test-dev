import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'

import AppDownloadMenu from '../components/AppDownloadMenu'

/**
 * This class representing the container component
 * for footer
 *
 * @class Footer
 * @extends {React.Component}
 */
class Footer extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		if (!this.props.footer.isVisible) {
			return null
		}

		return (
			<div id="footer">
				<ul className="footer-logo__list">
					<li className="logo-list__item">
						<a href="/m/" className="cw-logo-white logo__item"></a>
						<p className="cw-logo-tag">ask the experts</p>
					</li>
					<li className="logo-list__item">
						<a href="https://m.cartrade.com/" target="_blank" className="ct-logo-white logo__item"></a>
					</li>
					<li className="logo-list__item">
						<a href="https://www.bikewale.com/m/" target="_blank" className="bw-logo-white logo__item"></a>
					</li>
				</ul>

				<div className="footer-link__content">
					<ul className="social-link__list">
						<li className="social-link-list__item">
							<a href="/m/forums/" title="Forums">Forums</a>
						</li>
						<li className="social-link-list__item">
							<a href="/m/social/" title="Social Hub">Social Hub</a>
						</li>
					</ul>

					<AppDownloadMenu />
				</div>

				<div className="footer-about__content">
					<span className="footer-about__left-column">&copy; CarWale India</span>
					<div className="footer-about__right-column">
						<a href="/visitoragreement.aspx" title="Visitor Agreement">Visitor Agreement</a>&nbsp;&amp;&nbsp;<a href="/privacypolicy.aspx" title="Privacy Policy">Privacy Policy</a>
					</div>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		footer
	} = state

	return {
		footer
	}
}

export default connect(mapStateToProps)(Footer)
