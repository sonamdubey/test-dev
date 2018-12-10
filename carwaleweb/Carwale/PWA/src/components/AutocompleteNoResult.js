import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// no result message
	message: PropTypes.string,
}

function AutocompleteNoResult(props) {
	let { message } = props

	return (
		<li className="autocomplete-list-item">
			<div className="autocomplete-list-item__label">
				{message}
			</div>
		</li>
	)
}

export default AutocompleteNoResult
