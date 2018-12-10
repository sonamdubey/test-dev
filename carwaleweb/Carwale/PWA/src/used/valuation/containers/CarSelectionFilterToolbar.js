import React from 'react'
import { createRipple } from '../../../utils/Ripple'

class CarSelectionFilterToolbar extends React.Component {
    constructor(props) {
        super(props);
    }
    onFilterItemClick = (event) => {
        createRipple(event)
        let stateObj = {};
        if(event.currentTarget.getAttribute('data-type') === 'make'){
            stateObj['type']='make';
            stateObj['status'] = 1;
            this.props.makeSelection({id:"",name:"",state:stateObj}); // reset make
        }else{
            stateObj['type']='model';
            stateObj['status']=2;
            this.props.modelSelection({id:"",name:"",state:stateObj}); // reset model
        }
        this.props.clearInput();
        this.props.filterCar({ type: event.currentTarget.getAttribute('data-type'), data: "",isFilterApplied:false });
    }
    getTag = value =>{
        if(value && value.name){
            return (
                <span className="filter-item" onClick={this.onFilterItemClick} data-type={value.type}>
                        {value.name}
                </span>
            )
        }
    }
    render() {
        const {
            makeName,
            modelName
        } = this.props
        return (
            <div
                className="filter-container__filter-toolbar"
                id="summaryToolbar">
                {this.getTag({name:this.props.makeName,type:"make"})}
                {this.getTag({name:this.props.modelName,type:"model"})}
            </div>
        )
    }
}

export default CarSelectionFilterToolbar
