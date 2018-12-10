import React from 'react'
import Collapsible from '../../components/Collapsible'


class TransmissionTypeCollapsible extends React.Component {
    constructor(props) {
        super(props)
    }

    shouldComponentUpdate(nextProps){
        return this.props.collapsibleProps.open !== nextProps.collapsibleProps.open || this.props.collapsibleProps.selectionPreview != nextProps.collapsibleProps.selectionPreview
    }

    render(){
        return (
        <Collapsible {...this.props.collapsibleProps}>
            <ul className="filter-transmission-type__list">
                {this.props.getTransissionTypeList()}
            </ul>
        </Collapsible>)
    }
}

export default TransmissionTypeCollapsible
