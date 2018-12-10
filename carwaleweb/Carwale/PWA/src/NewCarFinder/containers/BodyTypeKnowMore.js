import React from 'react'
import PropTypes from 'prop-types'

import ListItem from '../../components/ListItem'

import {
	lockScroll,
	unlockScroll
} from '../../utils/ScrollLock'
import { fireInteractiveTracking } from '../../utils/Analytics';
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';

const propTypes = {
	// Array of body type data
	bodyTypeData: PropTypes.array,
	// A custom function to handle body type click
	handleBodyTypeClick: PropTypes.func,
	// A custom function to get status of body type i.e. active || disable
	getItemStatus: PropTypes.func
}

class BodyTypeKnowMore extends React.Component {
	constructor(props) {
		super(props)

		this.state = {
			previewHeadHeight: null,
			filtersFooterHeight: null,
			bodyTypeNameListHeight: null,
			isKnowMoreActive: false
		}
	}

	componentDidMount() {
		const previewHeadHeight = this.knowMoreContent.querySelectorAll(".know-more__head")[0].offsetHeight

		const bodyTypeNameListHeight = this.knowMoreContent.querySelectorAll(".know-more-body__top-content")[0].offsetHeight

		const filtersFooterHeight = document.getElementById("filtersFooter").offsetHeight

		this.setState({
			previewHeadHeight: previewHeadHeight,
			filtersFooterHeight: filtersFooterHeight,
			bodyTypeNameListHeight: bodyTypeNameListHeight
		}, () => {
			this.setPosition()
			window.addEventListener('resize', this.handleKnowMorePosition)
		})

		document.getElementsByClassName("body-type-screen")[0].style.marginBottom = (previewHeadHeight + filtersFooterHeight + 10) + "px" // 10px gutter space
		this.knowMoreContent.style.height = previewHeadHeight + "px"
	}

	componentWillUnmount() {
		unlockScroll()
		window.removeEventListener('resize', this.handleKnowMorePosition)
	}

	handleKnowMoreClick = () => {
		const {
			isKnowMoreActive,
			previewHeadHeight
		} = this.state

		if (!isKnowMoreActive) {
			lockScroll()
			this.knowMoreContent.style.bottom = "0"
			this.knowMoreContent.style.height = "100%"

			this.setBodyTypeListHeight()
		}
		else {
			unlockScroll()
			this.knowMoreContent.style.height = previewHeadHeight + "px"

			this.setPosition()
		}

		this.setState({
			isKnowMoreActive: !isKnowMoreActive
		})

		fireInteractiveTracking(CATEGORY_NAME, "NCF_BodyTypeFilter", "KnowMoreClick_"+ (isKnowMoreActive ? "close":"open"))
		trackCustomData(CATEGORY_NAME,"BodyTypeFilterKnowMoreClick","state=" + (isKnowMoreActive ? "close":"open"),false)
	}

	getBodyTypeNameList = () => {
		const {
			bodyTypeData,
			handleBodyTypeClick,
			getItemStatus,
			clickSource
		} = this.props

		let list = bodyTypeData.map(item => {
			const itemStatus = getItemStatus(item)

			return (
				<li
					key={item.id}
					className="list__item"
				>
					<span
						className={"btn-secondary-pill " + itemStatus}
						onClick={handleBodyTypeClick.bind(this, item, clickSource.KNOW_MORE_HEADER)}
					>
						{item.name}
					</span>
				</li>
			)
		})

		return list
	}

	getBodyTypeList = () => {
		const {
			bodyTypeData,
			handleBodyTypeClick,
			getItemStatus,
			clickSource
		} = this.props

		let list = bodyTypeData.map(item => {
			const itemStatus = getItemStatus(item)

			return (
				<li
					key={item.id}
					className={"list__item " + itemStatus}
					onClick={handleBodyTypeClick.bind(this, item, clickSource.KNOW_MORE_BODY)}
				>
					<ListItem item={item} />
				</li>
			)
		})

		return list
	}

	handleKnowMorePosition = () => {
		if(!this.state.isKnowMoreActive) {
			this.setPosition()
		}
		this.setBodyTypeListHeight()
	}

	setPosition = () => {
		const {
			filtersFooterHeight
		} = this.state

		this.knowMoreContent.style.bottom = filtersFooterHeight + "px"
	}

	setBodyTypeListHeight = () => {
		const {
			previewHeadHeight,
			bodyTypeNameListHeight,
			filtersFooterHeight
		} = this.state

		this.bodyTypeList.style.height = window.innerHeight - filtersFooterHeight - (previewHeadHeight + bodyTypeNameListHeight) + "px"
	}

	setKnowMoreContentReference = (ref) => {
		this.knowMoreContent = ref
	}

	setBodyTypeListReference = (ref) => {
		this.bodyTypeList = ref
	}

	render() {
		const {
			bodyTypeData
		} = this.props

		let bodyTypeNameList
		let bodyTypeList

		if (bodyTypeData && bodyTypeData.length) {
			bodyTypeNameList = this.getBodyTypeNameList()
			bodyTypeList = this.getBodyTypeList()
		}

		const activeClass = this.state.isKnowMoreActive ? 'active' : ''

		return (
			<div
				ref={this.setKnowMoreContentReference}
				className={"know-more__content " + activeClass}
			>
				<div
					onClick={this.handleKnowMoreClick}
					className="know-more__head"
				>
					<p className="know-more__title">Know more about body types</p>
					<span className="know-more__arrow"></span>
				</div>
				<div className="know-more__body">
					<div className="know-more-body__top-content">
						<ul className="body-type__name-list">
							{bodyTypeNameList}
						</ul>
					</div>
					<div
						ref={this.setBodyTypeListReference}
						className="know-more-body__bottom-content"
					>
						<ul className="body-type__list-about">
							{bodyTypeList}
						</ul>
					</div>
				</div>
			</div>
		)
	}
}

BodyTypeKnowMore.propTypes = propTypes

export default BodyTypeKnowMore
