import React from 'react'
import PropTypes from 'prop-types'
const propTypes = {
    heading: PropTypes.string.isRequired
}

function Heading(props) {
    let{
        heading
    } = props
    return (
        <div className="heading-container">
            <h1 className="listing-heading">{heading}</h1>
        </div>
    )
}

Heading.propTypes = propTypes

export default Heading
