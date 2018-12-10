import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// body type data
	item: PropTypes.object,
	// body type count
	showCount: PropTypes.bool,
}

const defaultProps = {
	showCount: true,
}

class MakeItem extends React.PureComponent {
	constructor(props) {
		super(props)
	}

	render(){
		const {
			item
		} = this.props
		return (
				<div className="item__heading btn-secondary-pill control control--checkbox">{item.makeName}
					<div className="control__indicator"></div>
				</div>
		)
	}
}

MakeItem.propTypes = propTypes
MakeItem.defaultProps = defaultProps

export default MakeItem
