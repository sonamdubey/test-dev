import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
    //handle click of component
    onClickHandler: PropTypes.func,
    value: PropTypes.string,
    isValid: PropTypes.bool
}

const defaultProps = {
    onClickHandler: null,
    value: 'Select City',
    isValid: true
}
class CitySelectionInput extends React.Component {
    constructor(props) {
        super(props)
    }
    render() {
        const validationStatus = this.props.isValid ? '' : 'invalid';
        return (
            <div className="city-selection__container">
                <p className="city-selection__title">City</p>
                <div className={"city-selection__input " + validationStatus}>
                    <div className="form-control" onClick={this.props.onClickHandler}>
                        {this.props.value/* TODO: add done class after selection */}
                    </div>
                    <span className="error-text">Please select city</span>
                </div>
            </div>
        );
    }
}

CitySelectionInput.propTypes = propTypes
CitySelectionInput.defaultProps = defaultProps

export default CitySelectionInput
