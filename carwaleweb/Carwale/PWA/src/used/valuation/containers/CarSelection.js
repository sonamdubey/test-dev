import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import CarSelectionContainer from './CarSelectionContainer'
import CarSelectionPopup from './CarSelectionPopup'
import {
    showCarPopup,
    fetchMakes,
    makeSelection,
    fetchModels,
    modelSelection,
    fetchVersions,
    versionSelection,
    carPopupStateSelection,
    filterCar,
    preSelectCarDetails
} from '../actionCreators/Car'
import { deserialzeQueryStringToObject } from '../../../utils/Common';

class CarSelection extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            status: 2
        }
    }
    getApiParameters = type => {
        let options = {};
        options["type"]="used";
        switch (type) {
            case "make":
                options["module"] = 2;
                options["year"] = this.props.manufacturingDetails.year;
                break;
            case "model":
                options["year"] = this.props.manufacturingDetails.year;
                options["makeid"] = this.props.car.selected.make.id;
                break;
            case "version":
                options["modelid"] = this.props.car.selected.model.id;
                options["year"] = this.props.manufacturingDetails.year;
                break;
            default:
                break;
        }
        return options;
    }
    isManufacturingDetailsValid = () =>{
        return validateManufacturingDetails(this.props.manufacturingDetails)
    }
    render() {
        return (
            <div className="car-selection">
                <CarSelectionContainer
                car={this.props.car}
                showCarPopup={this.props.showCarPopup}
                fetchMakes={this.props.fetchMakes}
                carPopupStateSelection={this.props.carPopupStateSelection}
                getApiParameters = {this.getApiParameters}
                />
                <CarSelectionPopup
                car={this.props.car}
                manufacturingDetails={this.props.manufacturingDetails}
                makeSelection={this.props.makeSelection}
                fetchModels={this.props.fetchModels}
                modelSelection={this.props.modelSelection}
                fetchVersions={this.props.fetchVersions}
                versionSelection={this.props.versionSelection}
                carPopupStateSelection={this.props.carPopupStateSelection}
                filterCar={this.props.filterCar}
                getApiParameters = {this.getApiParameters}
                 />
            </div>
        )
    }
    componentDidMount(){
        let {car} = deserialzeQueryStringToObject(window.location.search);
        if(car && !isNaN(car)){
            this.props.preSelectCarDetails({versionId:car});
        }
    }
}

const mapStateToProps = (state) => {
    const {
        car,
        manufacturingDetails
    } = state.usedCar.valuation
    return {
        car,
        manufacturingDetails
    }
}

const mapDispatchToProps = (dispatch,getState) => {
    return {
        showCarPopup : bindActionCreators(showCarPopup,dispatch),
        fetchMakes : bindActionCreators(fetchMakes,dispatch),
        makeSelection: bindActionCreators(makeSelection, dispatch),
        modelSelection: bindActionCreators(modelSelection, dispatch),
        fetchModels: bindActionCreators(fetchModels, dispatch),
        fetchVersions: bindActionCreators(fetchVersions, dispatch),
        versionSelection: bindActionCreators(versionSelection, dispatch),
        carPopupStateSelection: bindActionCreators(carPopupStateSelection, dispatch),
        filterCar: bindActionCreators(filterCar, dispatch),
        preSelectCarDetails:bindActionCreators(preSelectCarDetails,dispatch)
    }
}
export default connect(mapStateToProps,mapDispatchToProps)(CarSelection)
