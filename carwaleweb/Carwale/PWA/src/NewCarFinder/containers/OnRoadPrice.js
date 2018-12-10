import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PriceBreakup from '../components/PriceBreakup'
import OxygenLoader from '../../components/OxygenLoader'
import PricesApi from '../apis/Prices'
import {openPopup as openLocationPopup} from '../../Location/actionCreators/index'


const propTypes = {
	// on-road price details object
	data: PropTypes.object,
	cityName: PropTypes.string
}

class OnRoadPrice extends React.Component {
	constructor(props) {
		super(props)

		this.state = {
			priceBreakupTableVisibility: false,
			priceBreakupTable: false,
			priceBreakup: null,
			isFetching:false,
		}
	}
	setTableContentDimension = () => {
		const priceTable = this.refs.priceTableContent.querySelectorAll('.price__table')[0]
		const priceTableHeight = priceTable.getBoundingClientRect().height + 10 // 10px offset value

		this.refs.priceTableContent.style.height = `${priceTableHeight}px`
	}

	handlePriceInCityClick = () => {
		this.props.openLocationPopup()
		window.history.pushState('locationPopup','','')
		this.props.trackEvent && this.props.trackEvent("PriceBreakupViewClicked","PriceBreakupViewClicked","versionId="+this.props.data.id+"&element=ShowPriceInMyCity")
	}

	render() {
		let {
			data,
			cityName,
			cityId
		} = this.props
		return (
			<div className="on-road-price__content">
				<div className="price__content">
					<div className="price__column">
						<span className="price-content__amount">{data.priceOverview.price}</span>
						{this.props.pageName == "mainListing" && (!Number.isInteger(cityId) || cityId == -1) ?
						<span
							onClick={this.handlePriceInCityClick}
							className="price__breakup-link">
							Show Price In My City
						</span> : null
						}
						<p className="price-content__title">{data.priceOverview.label} {cityName ? ', ' + cityName : ''}</p>
					</div>
					<div className="price__emi-content text--italic">
						<p className="price-content__title">Emi</p>
						<span className="price-content__emi-amount">{data.emi}</span>
					</div>
				</div>
				<div ref="priceTableContent" className="price-table__content">
					{
						this.state.priceBreakupTable
							? <PriceBreakup data={this.state.priceBreakup} />
							: ( this.state.isFetching ? <OxygenLoader /> : null)
					}
				</div>
			</div>
		)
	}
}

OnRoadPrice.propTypes = propTypes

const mapDispatchToProps = (dispatch) => {
	return {
		openLocationPopup: bindActionCreators(openLocationPopup, dispatch),
	}
}

export default connect(null, mapDispatchToProps)(OnRoadPrice)

