import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// loader container height
	containerHeight: PropTypes.string,
	// loader height
	loaderHeight: PropTypes.string
}

const defaultProps = {
	containerHeight: '100px',
	loaderHeight: '50px'
}

class OxygenLoader extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		const {
			containerHeight,
			loaderHeight
		} = this.props

		return (
			<div className="oxygen-loader" style={{ height: containerHeight }}>
				<img src="https://imgd.aeplcdn.com/0x0/cw/static/icons/oxygen-loader.gif" alt="Loading" style={{ height: loaderHeight }} />
			</div>
		)
	}
}

OxygenLoader.propTypes = propTypes
OxygenLoader.defaultProps = defaultProps

export default OxygenLoader
