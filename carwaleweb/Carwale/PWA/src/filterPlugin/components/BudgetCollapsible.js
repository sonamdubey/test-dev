import React from 'react'
import Collapsible from '../../components/Collapsible'
import Rheostat from '../../components/Rheostat'

class BudgetCollapsible extends React.Component {
    constructor(props) {
        super(props)
    }

    shouldComponentUpdate(nextProps){
        return this.props.collapsibleProps.open !== nextProps.collapsibleProps.open || this.props.collapsibleProps.selectionPreview != nextProps.collapsibleProps.selectionPreview
    }

    render(){
        return (<Collapsible {...this.props.collapsibleProps}>
            <Rheostat
                {...this.props.slider}
            />
        </Collapsible>)
    }
}

export default BudgetCollapsible
