import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { selectOwner } from '../actionCreators/Owners'

import { scrollIntoView } from '../../../utils/ScrollTo'

import { createRipple } from '../../../utils/Ripple'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'

class Owners extends React.Component {
    constructor(props) {
        super(props);
    }

    getItemStatus = (item) => {
        let status = ''
        if (item.isSelected) {
            status = 'active'
        }
        return status
    }

    getOwnersList = () => {
        const data = this.props.owners.data

        let list = data.map((item) => {
            const itemStatus = this.getItemStatus(item)

            return (
                <li
                    key={item.id}
                    className="owners-list__item"
                >
                    <span
                        className={"btn-secondary-pill " + itemStatus}
                        onClick={this.handleOwnersClick.bind(this, item)}
                    >
                        {item.name}
                    </span>
                </li>
            )
        })

        return (
            <ul ref="ownersList" className="owners__list">
                {list}
            </ul>
        )
    }

    handleOwnersClick = (item, event) => {
        createRipple(event)
        trackForMobile(trackingActionType.ownersSelect, '')
        this.props.selectOwner(item)
        scrollIntoView(this.refs.ownersList, event)
    }

    render() {
        const validationStatus = this.props.owners.isValid ? '' : 'invalid'
        return (
            <div className={"owners__content " + validationStatus}>
                <p className="owners__title">Owners</p>
                {this.getOwnersList()}
                <span className="error-text">Please select Owner</span>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    const {
        owners
    } = state.usedCar.valuation

    return {
        owners
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        selectOwner: bindActionCreators(selectOwner, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Owners)
