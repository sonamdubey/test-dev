import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import ValuationReportContainer from './../components/ValuationReportContainer';
import SpeedometerLoader from '../../../../src/components/SpeedometerLoader';
import { closeValidationReport } from '../actionCreators/CheckReport'
import { BUY_CAR_ID, SELL_CAR_ID, BUY_CAR_BUTTON_TEXT, SELL_CAR_BUTTON_TEXT } from '../constants/index'
import { trackForMobile, trackingActionType } from '../utils/valuationTracking'

const propTypes = {
	//popup visibility
	active: PropTypes.bool,
	report: PropTypes.object,
	type: PropTypes.number
}
const defaultProps = {
	active: false,
	report: {},
	type: BUY_CAR_ID
}

class ValuationReport extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			containerHeight: '400px'
		}
	}

	setHeaderRef = element => {
		this.header = element;
	};
	setSubmitBtnRef = element => {
		this.submitBtn = element;
	};
	componentDidMount = () => {
		this.calculateReportContainerHeight();
	}

	componentWillReceiveProps = (nextProps) => {
		if (nextProps.report.active) {
			window.addEventListener('popstate', this.onPopState);
		}
		else {
			window.removeEventListener('popstate', this.onPopState)
		}
	}

	onPopState = () => {
		this.props.closeValidationReport()
	}

	calculateReportContainerHeight = () => {
		const value = window.innerHeight - this.submitBtn.offsetHeight - this.header.offsetHeight + 'px'
		this.setState({
			containerHeight: value
		});
	}
	handleReportBackClick = () => {
		history.back();
	}
	handleCampaignButtonClick = (e, valuationType) => {
		if (valuationType == SELL_CAR_ID) {
			trackForMobile(trackingActionType.sellCarButtonClick, '')
		}
		else {
			trackForMobile(trackingActionType.searchPageButtonClick, '')
		}
		return true
	}
	render() {
		const percentage = this.state.containerHeight;
		const active = this.props.report.active ? 'selection-report--active' : '';
		return (
			<div>
				<div className={this.props.report.isFetching ? "" : "hide"}>
					{SpeedometerLoader()}
				</div>
				<div className={"valuation-report " + active}>
					<div className="valuation-report__header" ref={this.setHeaderRef}>
						<span className="valuation-report__back-arrow" onClick={this.handleReportBackClick}></span>
						<p className="valuation-report__header-title">
							Car Valuation
			</p>
					</div>
					<div className="valuation-report__container" style={{ height: percentage }}>
						<ValuationReportContainer valuationHtml={this.props.report.valuationHtml} handleReportBackClick={this.handleReportBackClick} />
					</div>
					<div
						ref={this.setSubmitBtnRef}
						className="valuation-report__btn">
						<a href={this.props.report.campaignUrl} className="btn-primary"
							onClick={(e) => this.handleCampaignButtonClick(e, this.props.type)}>
							{this.props.type == SELL_CAR_ID ? SELL_CAR_BUTTON_TEXT : BUY_CAR_BUTTON_TEXT}
						</a>
					</div>
				</div>
			</div>
		)
	}
}
ValuationReport.propTypes = propTypes
ValuationReport.defaultProps = defaultProps

const mapStateToProps = (state) => {
	const { report, type } = state.usedCar.valuation;
	return {
		report, type
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		closeValidationReport: bindActionCreators(closeValidationReport, dispatch),
	}
}
export default connect(mapStateToProps, mapDispatchToProps)(ValuationReport);
