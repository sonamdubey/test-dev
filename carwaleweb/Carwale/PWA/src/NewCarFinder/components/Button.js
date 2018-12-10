import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
    // data object
    iconClass : PropTypes.string,
    btnText: PropTypes.string.isRequired,
    clickHandle: PropTypes.func.isRequired,
    active: PropTypes.bool
}
const defaultProps = {
    iconClass : '',
    active : true
}

function Button({iconClass, btnText, clickHandle, active}) {
    return (
        <div className="btn-container" onClick={clickHandle} >
            <span className={"btn-container__icon "+iconClass}></span>
            <span className={"btn-container__text " + (active ? "active" : "")}>{btnText}</span>
        </div>
    )
}

Button.propTypes = propTypes
Button.defaultProps = defaultProps

export default Button
