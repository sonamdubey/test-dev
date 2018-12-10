import React from 'react'
import {connect } from 'react-redux'
import {bindActionCreators } from 'redux'
import {withRouter }from 'react-router-dom'

import LocationContainer from '../../components/LocationContainer'
import {deserialzeQueryStringToObject, serialzeObjectToQueryString }from '../../utils/Common'
import {
	hideShortlistIcon

}from '../actionCreators/HeaderWrapper'

import {
	setCity
}from '../../actionCreators/CityAutocomplete'
import {fetchFilterScreen }from '../actionCreators/Filter';
import {
	fireInteractiveTracking
}from '../../utils/Analytics'
import { closePopup } from '../../Location/actionCreators/index'
import { NEWCARFINDER_RESULTS_ENDPOINT } from '../constants/index'
import { trackCustomData } from '../../utils/cwTrackingPwa'
import { CATEGORY_NAME } from '../constants/index'
/**
 * This class representing the container component
 * for city selection screen.
 *
 * @class Location
 * @extends {React.Component}
 */
class Location extends React.Component {
	constructor(props) {
		super(props)
	}
	componentWillMount() {
		window.addEventListener('popstate', this.onPopState);
	}

	componentWillUnmount() {
		window.removeEventListener('popstate', this.onPopState);
	}

	handleCitySelection = (city) => {
		this.props.setCity(city)
		let label = this.props.cityLocation.cityId <= 0?city.cityName:this.props.cityLocation.cityName + '/' + city.cityName
		if (city.isConfirmBtnClicked) {
			fireInteractiveTracking('NCFPWA', 'CitySelected', label)
			trackCustomData(CATEGORY_NAME,"CitySelected","selectedCity="+city.cityName+"|currentCity="+this.props.cityLocation.cityName)
		}
		this.props.closePopup()
		this.updateCityInUrl(city)

	}
	updateCityInUrl =(city)=>{
		const endPoint = this.props.location.pathname
		if(endPoint === NEWCARFINDER_RESULTS_ENDPOINT){
			const url = this.props.location.search
			let searchParams = deserialzeQueryStringToObject(url)
			searchParams.cityId = city.cityId
			const currentUrl = serialzeObjectToQueryString(searchParams)
			this.props.history.replace(NEWCARFINDER_RESULTS_ENDPOINT + '?' + currentUrl)
		}
		else {
			window.location.reload()
		}
	}
	handleClose = () =>  {
		this.props.closePopup()
		trackCustomData(CATEGORY_NAME,"LocationScreenCloseClick","")
	}

	onPopState = (state) => {
		this.props.closePopup()
	}

	render() {
		let nextPath = '/'
		if (this.props.location) {
			const {location } = this.props
			const queryParams = deserialzeQueryStringToObject(location.search)
			nextPath = queryParams['next']
		}
		return ( < LocationContainer
				isPopup
				closeable
				onClose={this.handleClose}
				isActive={this.props.isActive}
				nextPath={nextPath}
				setCity={this.handleCitySelection}
				cityLocation={this.props.cityLocation}
				resetInputOnClose

			/> )
	}
}
const mapStateToProps = (state) =>  {
	const {
		location
	} = state

	const {locationPopupActive}=state.newCarFinder
	return {
		cityLocation:location,
		isActive:locationPopupActive
	}
}
const mapDispatchToProps = (dispatch, getState) =>  {
	return {
		setCity:bindActionCreators(setCity, dispatch),
		fetchFilterScreen:bindActionCreators(fetchFilterScreen, dispatch),
		hideShortlistIcon:bindActionCreators(hideShortlistIcon, dispatch),
		closePopup: bindActionCreators(closePopup, dispatch),
	}
}
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Location))
