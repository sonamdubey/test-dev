import React from 'react'
import PropTypes from 'prop-types'

import LocationContainer from '../../../components/LocationContainer'

const propTypes = {
    //popup visibility
    isActive: PropTypes.bool,
    //handle back click of pop up
    handleBackClick: PropTypes.func,
    cityLocation: PropTypes.object,
    clearSelection: PropTypes.func
}

class CitySelectionPopup extends React.Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <LocationContainer
                isPopup={true}
                isBackButton={true}
                isActive={this.props.isActive}
                isSubtitleActive={false}
                handleBackClick={this.props.handleBackClick}
                setCity={this.props.setCity}
                cityLocation={this.props.cityLocation}
                clearSelection={this.props.clearSelection}/>
        )
    }
}

CitySelectionPopup.propTypes = propTypes

export default CitySelectionPopup
