import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// body type data
	item: PropTypes.object,
	// body type count
	showCount: PropTypes.bool,
	// body type icon
	iconType: PropTypes.string
}

const defaultProps = {
	showCount: true,
	iconType: 'color'
}

class BodyTypeItem extends React.PureComponent {
	constructor(props) {
		super(props)
	}

	render(){
		const {
			item,
			showCount,
			iconType
		} = this.props

		let icon

		if (iconType === 'line') {
			icon = item.lineIcon
		}
		else {
			icon = item.icon
		}

		return (
			<div>
				<div className="item__icon-content">
					<img src={icon} alt={item.name} className="item__icon" />
				</div>
				<p className="item__heading">
					{item.name}
					{showCount ? " (" + item.carCount + ")" : ""}
				</p>
			</div>
		)
	}
}

BodyTypeItem.propTypes = propTypes
BodyTypeItem.defaultProps = defaultProps

export default BodyTypeItem
