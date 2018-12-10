import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import FiltersScreen from '../../filterPlugin/containers/FiltersScreen'
import FilterContainer from './FilterContainer'
import ModelList from './ModelList'
import CompareCars from './CompareCars'
import Heading from '../components/Heading'
import { deserialzeQueryStringToObject, serialzeObjectToQueryString } from '../../utils/Common'
import ncfDefaultFilters from '../../filterPlugin/constants/NCFdefaultfilters'

import { reduceToSelectionObject, mergeQStoStore, getNCFFilterParams, getFilterStoreObject, modifySearchParamsWithStoredFilters } from '../../filterPlugin/selectors/FilterPluginSelectors'

import { setCurrentScreen } from '../actionCreators/Filter';
import { NEWCARFINDER_RESULTS_ENDPOINT } from '../constants/index';
import {setFilters} from '../../filterPlugin/actionCreators/FiltersScreen'
import {
	showFooter
} from '../../actionCreators/Footer'
import {
	showHeader
} from '../../actionCreators/Header'
import {
	showShortlistIcon,makeHeaderSticky
} from '../actionCreators/HeaderWrapper'
import {
	hideShortlistPopup
} from '../actionCreators/Shortlist'
import ShortlistPopup from '../components/ShortlistPopup'
import Footer from '../../containers/Footer'
import { unlockScroll, lockScroll } from '../../utils/ScrollLock'
import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index'
import { Helmet } from 'react-helmet'
import { getTitle, getHeading, getDimensions } from '../utils/seo-tags'
/**
 * This class representing the container component
 * for listing screen
 *
 * @class Listing
 * @extends {React.Component}
 */
class Listing extends React.Component {
	constructor(props) {
		super(props)
		if (this.props.currentScreen !== this.props.maxScreens && this.props.currentScreen > -1) {
			const filterParams = getNCFFilterParams(this.props.state)
			const qsFromStore = '?' + serialzeObjectToQueryString(filterParams)
			if(qsFromStore!==this.props.location.search){
				this.props.history.replace((NEWCARFINDER_RESULTS_ENDPOINT + qsFromStore))
			}
		}
		this.renderResults = !this.props.shortlistCars.active
		this.shouldBeAtPageTop = true
		}

	componentWillMount(){
        this.props.showHeader()
		this.props.showShortlistIcon()
		this.props.makeHeaderSticky()
	}

	componentDidMount() {
		this.props.setCurrentScreen(this.props.maxScreens)
		trackCustomData(CATEGORY_NAME, "ListingScreenImpression", "NA", false)
	}
	componentWillUpdate(nextProps){
		if(nextProps.shortlistCars.active){
			lockScroll()
		}
		this.shouldBeAtPageTop = nextProps.shortlistCars.active === this.props.shortlistCars.active
	}
	componentDidUpdate(){
		if(this.props.shortlistCars.count == 0 && !this.props.shortlistCars.active){
			unlockScroll()
		}
	}
	shouldComponentUpdate(nextProps, nextState) {
		if(nextProps.location.search != this.props.location.search) {
			return true
		}
		if(nextProps.shortlistCars.active != this.props.shortlistCars.active) {
			return true
		}
		if(nextProps.isSEOPage != this.props.isSEOPage) {
			return true
		}
		return false
	}

	handleShortlistBack = () => {
		this.props.hideShortlistPopup()
		unlockScroll()
	}

	renderMainListingPage (searchParams,seoData){
		if(this.renderResults){
			return (
				<ModelList key="mainListing" isSEOPage={this.props.isSEOPage}  pagename="mainListing" searchParams={searchParams} seoData={seoData}/>
			)
		}
		else{
			this.renderResults = true
		}
	}
	renderHelmetComponent (seoData) {
		return (
			<Helmet>
				<title>{seoData.title}</title>
				<link rel="canonical" href={seoData.url} />
			</Helmet>
		)
	}
	getSEOData (searchParams) {
		let dimension = getDimensions(searchParams)
		let title = getTitle(searchParams,dimension)
		let heading = getHeading(searchParams,dimension,false)
		let url = window.location.href
		let h1 = getHeading(searchParams,dimension,true)
		return {title , heading, url, dimension, h1 }
	}
	render() {
		if(this.shouldBeAtPageTop){
			window.scrollTo(0, 0)
		}
		const ncfDefaultFiltersClone = ncfDefaultFilters()
		const searchParams = deserialzeQueryStringToObject(this.props.location.search)

		if(this.props.shortlistCars.removedModelIds != undefined && this.props.shortlistCars.removedModelIds.length){
			searchParams.removedModelIds =this.props.shortlistCars.removedModelIds
		}

		let seoData = null,
			headingComponent = null,
			helmetComponent = null,
			modifiedSearchParams = {
				... {},
				... searchParams
			}
		if(this.props.isSEOPage) {
			seoData = this.getSEOData (searchParams)
			helmetComponent = this.renderHelmetComponent(seoData)
			headingComponent =	<Heading heading={seoData.h1} />
			modifiedSearchParams = modifySearchParamsWithStoredFilters(modifiedSearchParams, this.props.storedFilters)
		}
		else{
			this.props.setFilters(getFilterStoreObject(searchParams))
			helmetComponent = (
				<Helmet>
					<title>New Car Finder - CarWale.com</title>
				</Helmet>
			)
		}
		const defaultFilters = mergeQStoStore(modifiedSearchParams, ncfDefaultFiltersClone)
		const selectionObject = reduceToSelectionObject(defaultFilters)

		return (
			<div className="listing-screen">
				{helmetComponent}
				{headingComponent}
				{this.renderMainListingPage(searchParams,seoData)}
				<CompareCars />
				<FilterContainer
					withCompare={this.props.state.compareCars.active}
					searchParams={searchParams} />
				<FiltersScreen
					defaultFilters={defaultFilters}
					selectionObject={selectionObject}
					trackingCategory={CATEGORY_NAME}
					location={this.props.location}
					history={this.props.history}
					match={this.props.match}
				/>
				<ShortlistPopup status={this.props.shortlistCars.active} handleBack={this.handleShortlistBack} cityId={searchParams.cityId}/>
				<Footer />
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const filterScreens = state.newCarFinder.filter.filterScreens
	const currentScreen = filterScreens.currentScreen
	const maxScreens = filterScreens.screenOrder.length
	const city = state.location
	const {
			shortlistCars
		} = state.newCarFinder
	const {
		filters: storedFilters
	} = state.filtersScreen
	return {
		currentScreen,
		maxScreens,
		state,
		shortlistCars,
		city,
		storedFilters
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		setCurrentScreen: bindActionCreators(setCurrentScreen, dispatch),
		showFooter: bindActionCreators(showFooter, dispatch),
		showHeader: bindActionCreators(showHeader, dispatch),
		hideShortlistPopup: bindActionCreators(hideShortlistPopup, dispatch),
		showShortlistIcon: bindActionCreators(showShortlistIcon, dispatch),
		makeHeaderSticky: bindActionCreators(makeHeaderSticky, dispatch),
		setFilters: bindActionCreators(setFilters, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Listing)
