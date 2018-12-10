import React from 'react'
import PropTypes from 'prop-types'

import {
	formatToINR
} from '../../utils/Common'

const propTypes = {
	// price breakup object
	data: PropTypes.object
}

function PriceBreakup(props) {
	let {
		data
	} = props

	let pricesList

	if (data.pricesList && data.pricesList.length) {
		pricesList = data.pricesList.map(x => {
			return (<tr key={x.id}>
				<td className="price-table__td-key">{x.name}</td>
				<td className="price-table__td-value">{formatToINR(x.value)}</td>
			</tr>)
		})
	}

	return (
		<table className="price__table">
			<tbody>
				{pricesList}
			</tbody>
		</table>
	)
}

export default PriceBreakup
