import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import EmiHeader from '../components/EmiHeader'
import EmiSliders from './EmiSliders'

import '../utils/events'

import {
	isDesktop
} from '../../utils/Common'

import {
	setPricesData,
	updateEmiModel
} from '../actionCreators/emiData'
import {
	hideEmiPopupState,
	showEmiPopupState
} from '../actionCreators/emiPopupState'

class EmiPrices extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			newInputValue: 1
		}
		this.newInputValueSetter = this.newInputValueSetter.bind(this);
		this.newEmiToggleSetter = this.newEmiToggleSetter.bind(this);
	}
	componentDidMount(){
		if (window.EmiCalculator.setEMIModelData) {
			window.EmiCalculator.setEMIModelData();
		}
	}
	newInputValueSetter(n) {
		this.setState({newInputValue:n});
	}
	newEmiToggleSetter(n) {
		this.setState({emiPopuptoggle:n});
	}

	render() {
		const popupClassName = this.props.emiPopupState ? 'show': 'hide';
		return (
			<div className={"emi-pop-div " + popupClassName}>
				<EmiHeader tenureValue={this.state.newInputValue}/>
				<EmiSliders
					changeTenureValue={this.newInputValueSetter}
					isTitleVisible={isDesktop}
				/>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		emiPopupState
	} = state
	return {
		emiPopupState
	}
}


const mapDispatchToProps = (dispatch, getState) => {
	return {
		setPricesData: bindActionCreators(setPricesData, dispatch),
		updateEmiModel: bindActionCreators(updateEmiModel, dispatch),
		showEmiPopupState: bindActionCreators(showEmiPopupState, dispatch),
		hideEmiPopupState: bindActionCreators(hideEmiPopupState, dispatch),
	}
}
export default connect(mapStateToProps, mapDispatchToProps)(EmiPrices)
