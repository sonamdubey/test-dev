import React from 'react'
import {PropTypes} from 'prop-types'

const propTypes ={
    getValuation: PropTypes.func
}

const defaultProps = {
    getValuation: null
}

class CheckReport extends React.Component {
    constructor(props) {
        super(props);
    }

    onClickHandler = () => {
        this.props.getValuation()
    }

    render() {
      return (
        <div className="valuation-button__container">
            <button className="btn-primary" onClick={this.onClickHandler}>
                Check Value
            </button>
        </div>
      );
    }
}

CheckReport.propTypes = propTypes
CheckReport.defaultProps = defaultProps

export default CheckReport

