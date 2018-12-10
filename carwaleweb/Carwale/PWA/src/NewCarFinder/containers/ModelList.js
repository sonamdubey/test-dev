/**
 * This class representing the container component
 * for find car model listing
 *
 * @class ModelList
 * @extends {React.Component}
 */
import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { throttle } from 'throttle-debounce'

// action creators
import {
	fetchModelList, fetchNextPage
} from '../actionCreators/ModelList'

import {
	initToast
} from '../../actionCreators/Toast'

import {
	openFilterScreen
} from '../../filterPlugin/actionCreators/FiltersScreen'

import ModelCard from '../components/ModelCard';
import ModelBoxLoader from '../../components/ModelBoxLoader'
import VersionBoxLoader from '../../components/VersionBoxLoader'
import NoResult from '../../components/NoResult'
import { Helmet } from 'react-helmet'

import { getDescription, getSchema, getBudgetText, getSocialTags } from '../utils/seo-tags'
import { fireInteractiveTracking} from '../../utils/Analytics';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index';

class ModelList extends React.Component {
	constructor(props) {
		super(props)

		this.defaultActiveVersionCount = 1
		this.handlePagination = throttle(100, this.handlePagination)
		this.mediaQuery = window.matchMedia("(orientation: portrait)")
	}

	componentDidMount(){
		const {
			fetchModelList,
			searchParams
		} = this.props
		window.addEventListener('scroll', this.handlePagination)
		this.mediaQuery.addListener(this.handleOrientationChange)
		fetchModelList(searchParams,this.props.pageName)
	}

	componentWillUnmount(){
		window.removeEventListener('scroll', this.handlePagination)
		this.mediaQuery.removeListener(this.handleOrientationChange)
	}

	componentDidUpdate(prevProps) {
		if (this.props.pageName != 'mainListing' && prevProps.isFetching && !this.props.isFetching) {
			window.dispatchEvent(new Event('scroll'))
		}
	}

	shouldComponentUpdate(nextProps, nextState){
		if (nextProps.isFetching != this.props.isFetching) {
			return true
		}
		else if(this.props.shortlistCars.count != nextProps.shortlistCars.count){
			return true
		}
		if(nextProps.isSEOPage != this.props.isSEOPage) {
			return true
		}
		return false
	}

	componentWillReceiveProps(nextProps){
		const {
			totalModels,
			searchParams,
			fetchModelList
		} = nextProps

		let { isFetching } = nextProps

		const searchKeys = Object.keys(searchParams)

		if (searchKeys.length !== Object.keys(this.props.searchParams).length) {
			isFetching = true
			fetchModelList(searchParams,this.props.pageName)
		}
		else {
			for (let index in searchKeys) {
				let key = searchKeys[index]
				if (searchParams[key] !== this.props.searchParams[key]) {
					isFetching = true
					fetchModelList(searchParams,this.props.pageName)
					break
				}
			}
		}
	}

	getModelList = (data) => {
        let list = data.map((item, index) => {
            let nextTrigger = false
            let rank = index+1
			if (index === data.length - 2) {
				nextTrigger = {
					ref: 'nextTrigger'
                }
			}

			let isShortlisted = this.props.shortlistCars.modelIds.findIndex(x => x == item.modelId) >= 0
			let shortlistClass = isShortlisted ? "shortlisted" : ""
			let action = this.props.pageName == 'mainListing' ? 'ListingModelImpression' : 'ShortlistModelImpression'
			if(!isShortlisted && this.props.pageName != 'mainListing') return null
			return (
				<li key={item.modelId} className={"model-list__item " + shortlistClass} {...nextTrigger}>
					<ModelCard
						action={action}
						label={"makeName="+item.makeName + "|modelId=" + item.modelId + "|modelName=" + item.modelName + "|modelRank=" + rank} item={item}
						isShortlisted={isShortlisted}
						rank={rank}
						pageName={this.props.pageName}
						cityName={this.props.cityName}
						cityId={this.props.cityId}
						shortlistCars={this.props.shortlistCars}/>
				</li>
			)
		})

		return (
			<ul className="model-list__content">
				{list}
			</ul>
		)
	}

	getLoaderList = (count) => {
		let listItems = []
		let item = (key) => {
			return (
				<li className="model-loader-list__item" key={key}>
					<ModelBoxLoader />
					<div className="version-loader__content">
						<VersionBoxLoader />
					</div>
				</li>
			)
		}

		if (!count) {
			count = 1
		}

		for (let i = 0; i < count; i++) {
			listItems.push(item(i))
		}

		this.removeNoResultStyle()

		return (
			<ul className="model-loader-list__content">
				{listItems}
			</ul>
		)
	}

	trackEvent = (action,label) => {
		fireInteractiveTracking("NCFPWA", action, label)
	}

	handlePagination = () => {
		if (!this.props.isFetching && this.refs.nextTrigger && this.props.modelListData.length < this.props.totalModels) {
			const loaderBounds = this.refs.nextTrigger.getBoundingClientRect() //alternative ?
			if ((loaderBounds.top >= 0 && loaderBounds.top < window.innerHeight) || (loaderBounds.top < 0)) {
				this.props.fetchNextPage(this.props.searchParams,this.props.pageName)
			}
		}
	}

	handleOrientationChange = (mediaQuery) => {
		if(!(this.props.pagename == 'mainListing' && this.props.shortlistCars.active)){
			let action = this.props.pageName == 'mainListing' ? 'Listing' : 'Shortlist'
			if (mediaQuery.matches) {
				// Changed to portrait
				this.trackEvent("NCF_"+action,"TO_PORTRAIT")
				trackCustomData(CATEGORY_NAME,action+"OrientationChange","orientation=Portrait|pageName="+(this.props.pageName == 'mainListing' ? 'Listing' : 'Shortlist'),false)
			}
			else {
				// Changed to landscape
				this.trackEvent("NCF_"+action,"TO_LANDSCAPE")
				trackCustomData(CATEGORY_NAME,action+"OrientationChange","orientation=Landscape|pageName="+(this.props.pageName == 'mainListing' ? 'Listing' : 'Shortlist'),false)
			}
		}
	}

	handleEditClick = () => {
		this.props.openFilterScreen()
	}

	removeNoResultStyle = () => {
		let bodyElement = document.getElementsByTagName('body')[0]

		if (bodyElement.classList.contains('no-result')) {
			bodyElement.classList.remove('no-result')
		}
	}

	noResultFound = () => {
		document.getElementsByTagName('body')[0].classList.add('no-result')

		const noResultProps = {
			type: 'listing__no-result',
			title: 'No Cars found',
			subtitle: 'Kindly change your preferences to view results.',
			imageClass: 'listing-no-result__image'
		}
		trackCustomData(CATEGORY_NAME,"NoCarsWidgetShown","url="+window.location.href,true)
		return (
			<NoResult {...noResultProps}>
				<button
					type="button"
					className="btn-secondary"
					onClick={this.handleEditClick}
				>
					Edit Filters
				</button>
			</NoResult>
		)
	}

	renderHelmetComponent (description,budgetText,modelListData) {
		const logoUrl = "https://imgd.aeplcdn.com/0x0/cw/design15/carwale.png?v=1.1"
		return (
			<Helmet>
				<meta name="description" key="description" content={description} />
				{getSocialTags(this.props.seoData.heading,description,
					this.props.seoData.url,logoUrl)}
				<script type='application/ld+json' name="schema">
					{getSchema(modelListData,this.props.seoData.url,this.props.seoData.heading,this.props.seoData.dimension,budgetText)}
				</script>
			</Helmet>
		)
	}
	render() {
		const {
			modelListData,
			totalModels,
			isFetching,
			searchParams
		} = this.props

		let modelList

		const isDataAvailable = modelListData && modelListData.length
		let description = null,
			helmetComponent = null,
			budgetText = null

		if (isDataAvailable) {
			modelList = this.getModelList(modelListData)
			this.removeNoResultStyle()
			if(this.props.isSEOPage && this.props.pageName == 'mainListing') {
				description = getDescription(searchParams,this.props.seoData.dimension,
					{name: modelListData[0].makeName + " " + modelListData[0].modelName, price: modelListData[0].priceOverview.price})
				budgetText = getBudgetText(searchParams)
				helmetComponent = this.renderHelmetComponent(description,budgetText,modelListData)
			}
		}

		return (
			<div className="listing__content">
				{helmetComponent}
				{modelList}
				{
					isFetching
						? this.getLoaderList(2)
						: (isDataAvailable
							? ''
							: (this.props.pageName == 'mainListing' ? this.noResultFound() : '')
						)
				}
			</div>
		)
	}
}

const mapStateToProps = (state, ownProps) => {
	const pageName = ownProps.pagename
	const {
		data: modelListData,
		totalModels,
		isFetching,
		cityName,
		cityId
	} = state.newCarFinder.listingData[pageName]
	const shortlistCars = state.newCarFinder.shortlistCars
	return {
		modelListData,
		isFetching,
		totalModels,
		cityName,
		cityId,
		pageName,
		shortlistCars
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		fetchModelList: bindActionCreators(fetchModelList, dispatch),
		fetchNextPage: bindActionCreators(fetchNextPage, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		openFilterScreen: bindActionCreators(openFilterScreen, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(ModelList)
