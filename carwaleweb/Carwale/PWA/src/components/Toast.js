import React from 'react'
import PropTypes from 'prop-types'
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
		super(props)
	}

	render() {
		const {
			toast
		} = this.props

		if(!toast.isVisible & !toast.outclick) {
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
	const {
		toast
	} = state

	return {
		toast
	}
}

export default connect(mapStateToProps, null)(Toast)
