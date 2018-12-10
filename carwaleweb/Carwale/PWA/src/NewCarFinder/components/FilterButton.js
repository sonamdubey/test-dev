import React from 'react'
import PropTypes from 'prop-types'
const propTypes = {
    onClick: PropTypes.func,
    text: PropTypes.string,
    count: PropTypes.number
}
const defaultProps = {
    onClick: null,
    text: 'Filter',
    count: 0
}
function FilterButton(props) {
    let{
        onClick,
        text,
        count
    } = props
    let className = props.withCompare ? " filterBtn__withCompare" : ""
    return (
        <div className={"filter-container"+className} onClick={onClick} >
            <a className="filter-container__link">
                {text}
                {
                    (count > 0) && <span className="filter-container__count" >{count}</span>
                }
            </a>
        </div>
    )
}

export const ChangeFilterPosition = function ChangePosition(isComparePresent){
    const element = document.getElementsByClassName('filter-container')[0]
    if(element){
        if(isComparePresent){
            element.className += " filterBtn__withCompare"
        }
        else{
            element.className = "filter-container"
        }
    }
}

FilterButton.propTypes = propTypes
FilterButton.defaultProps = defaultProps
export default FilterButton
