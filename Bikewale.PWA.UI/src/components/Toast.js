import React from 'react'
import PropTypes from 'prop-types'
import { toJS } from '../immutableWrapperContainer'
import { connect } from 'react-redux'

const propTypes = {
	// visibility state
	isVisible: PropTypes.bool,
	// toast message to display
	message: PropTypes.string,
	// time period for toast message to stay on screen
	duration: PropTypes.number,
	// value return by setTimeout method
	toastTimer: PropTypes.number,
	//Event
	userEvent: PropTypes.func,
}

class Toast extends React.Component {
	constructor(props) {
		super(props);
	}

	render() {
		const {
			toast
		} = this.props

		if (toast == undefined || (toast != undefined && !toast.isVisible && !toast.outclick)) {
			return null
		}
		else {
			return (
				<div className="toast-box" style={toast.style}>
					{toast.message}
				</div>
			)
		}
	}
}

Toast.propTypes = propTypes

const mapStateToProps = (state) => {
	return {
		toast: state.getIn(['Toast', 'Toast'])
	}
}

export default connect(mapStateToProps, null)(toJS(Toast));