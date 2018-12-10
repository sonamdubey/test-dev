import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import {CATEGORY_NAME} from '../constants/index';
import {
	createRipple
} from '../../utils/Ripple'

import {
	openFilterScreen
} from '../actionCreators/FiltersScreen'


import { throttle } from 'throttle-debounce'
import { fireInteractiveTracking } from '../../utils/Analytics';
import { trackCustomData } from '../../utils/cwTrackingPwa';
/**
 * This class representing the container component
 * for filters toolbar
 *
 * @class FiltersToolbar
 * @extends {React.Component}
 */
class FiltersToolbar extends React.Component {
	constructor(props) {
		super(props)

		this.lastScrollPosition = 0
	}

	componentDidMount() {
		//window.addEventListener('scroll', this.floatToolbar)
	}

	componentWillUnmount() {
		//window.removeEventListener('scroll', this.floatToolbar)
	}

	floatToolbar = () => {
		let windowScrollPosition = window.pageYOffset || document.documentElement.scrollTop

		this.placeholderOffsetTop = this.refs.toolbarPlaceholder.getBoundingClientRect().top + windowScrollPosition

		if (this.lastScrollPosition > windowScrollPosition) {
			this.refs.toolbarContent.classList.add('toolbar--fixed')

			if (windowScrollPosition < this.placeholderOffsetTop) {
				this.refs.toolbarContent.classList.remove('toolbar--fixed')
			}
		}
		else {
			this.refs.toolbarContent.classList.remove('toolbar--fixed')
		}

		this.lastScrollPosition = windowScrollPosition
	}

	handleEditClick = (event) => {
		this.props.openFilterScreen()
		this.trackEvent("EditOptionClick")
		trackCustomData(CATEGORY_NAME,"ListingEditOptionClick","filterType=none",false)
	}

	handleFilterTabClick = (event) => {
		createRipple(event)
		const filterType = event.currentTarget.getAttribute('data-filter-type')

		this.props.openFilterScreen(filterType)
		this.trackEvent("EditOptionClick_" + filterType)
		trackCustomData(CATEGORY_NAME,"ListingEditOptionClick","filterType="+filterType,false)
	}

	getSelections = () => {
		const { selectionObject } = this.props
		if (selectionObject) {
			return Object.keys(selectionObject).map(selection => {
				return (<li className="filter-toolbar-list__item" key={selection}>
					<div
						className="btn-tab"
						data-filter-type={selection}
						onClick={this.handleFilterTabClick}
					>
						<span className="tab__key">{selection}</span>
						<span className="tab__value">{selectionObject[selection]}</span>
					</div>
				</li>)
			});
		}
	}

	trackEvent(label) {
		fireInteractiveTracking("NCFPWA", "NCF_Listing", label)
	}

	render() {
		return (
			<div ref="toolbarPlaceholder" className="filters__toolbar-placeholder">
				<div ref="toolbarContent" className="filters-toolbar__content">
					<div className="toolbar--relative">
						<div className="filters-toolbar-list__content">
							<ul className="filter-toolbar__list">
								{this.getSelections()}
							</ul>
						</div>
						<div
							onClick={this.handleEditClick}
							className="filter__edit"
						>
							Edit
						</div>
					</div>
				</div>
			</div>
		)
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		openFilterScreen: bindActionCreators(openFilterScreen, dispatch)
	}
}

export default connect(null, mapDispatchToProps)(FiltersToolbar)
