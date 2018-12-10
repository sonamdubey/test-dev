import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// pit element style object
	style: PropTypes.object,
	// label for pit element
	children: PropTypes.number
}

function PitComponent({ style, children }) {
	return (
		<span
			className="pit-item"
			style={{...style}}
		>
			{children}
		</span>
	)
}

export default PitComponent
