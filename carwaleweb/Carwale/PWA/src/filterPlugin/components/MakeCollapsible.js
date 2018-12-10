import React from 'react'
import Collapsible from '../../components/Collapsible'


class MakeCollapsible extends React.Component {
    constructor(props) {
        super(props)
    }

    shouldComponentUpdate(nextProps){
        return this.props.collapsibleProps.open !== nextProps.collapsibleProps.open || this.props.collapsibleProps.selectionPreview != nextProps.collapsibleProps.selectionPreview
}

    render(){
        return (
        <Collapsible {...this.props.collapsibleProps}>
            <ul className="filter-make__list">
                {this.props.getMakeList()}
            </ul>
        </Collapsible>)
    }
}

export default MakeCollapsible
