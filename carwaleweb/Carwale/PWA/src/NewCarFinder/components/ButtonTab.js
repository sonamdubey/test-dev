import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// data object
	item: PropTypes.object
}

function ButtonTab({ item }) {
	return (
		<div className="btn-tab">
			<span className="tab__key">Budget</span>
			<span className="tab__value">&#x20b9;&nbsp;39 lakh</span>
		</div>
	)
}

ButtonTab.propTypes = propTypes

export default ButtonTab
