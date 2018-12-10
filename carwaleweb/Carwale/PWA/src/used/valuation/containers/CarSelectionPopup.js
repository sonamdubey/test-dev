import React from 'react'
import PropTypes from 'prop-types'
import CarSelectionFilterToolbar from './CarSelectionFilterToolbar'
import { createRipple } from '../../../utils/Ripple'
import SpeedometerLoader from '../../../../src/components/SpeedometerLoader'
import {trackForMobile, trackingActionType} from '../utils/valuationTracking'

const propTypes = {
    //popup visibility
    active: PropTypes.bool,
    //title name
    title: PropTypes.string
}
const defaultProps = {
    active: false,
    title: 'Make'
}
class CarSelectionPopup extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            selectionListHeight: '400px',
            input :''
        }
    }
    setSelectionPopupInputRef = element => {
        this.carSelectionPopupInput = element;
    };
    setSelectionPopupList = element => {
        this.carSelectionList = element;
    };
    //function to filter data
    onInputChange = (event) => {
        let value = event.currentTarget.value;
        //update filtered data to store.
        this.state.input = value;
        this.props.filterCar({ type: this.props.car.state.type, data: value,isFilterApplied:(value && value.length > 0) });
    }
    componentDidMount = () => { // TODO: this should remove after popup open logic gets implemented
        if (this.props.car.state.active) {
            this.carSelectionPopupInput.focus();
        }
        this.calculateSelectionContainerHeight();
    }
    calculateSelectionContainerHeight = () => {
        this.setState({
            selectionListHeight: window.innerHeight - this.carSelectionList.offsetTop - 100 + 'px' //here 100 is bottom free space after list open
        });
    }
    fetchNextItem = (type,status,nextStep) =>{
        let stateObj = {};
        this.clearInput();
        this.removeFilter();
        setTimeout(() => { // since reading form the store immediate write, better to schedule to avoid store miss
            stateObj["type"] = type;
            stateObj["status"] = status;
            stateObj["options"] = this.props.getApiParameters(type);
            nextStep(stateObj);
        }, 100);
    }
    handleListItemClick = (event) => {
        let id = event.currentTarget.getAttribute('data-id');
        let name = event.currentTarget.getAttribute('data-name');
        let stateObj = {};
        switch (this.props.car.state.type) {
            case "make":
                trackForMobile(trackingActionType.makeSelect, '')
                this.props.makeSelection({
                    id: id,
                    name: name
                });
                this.fetchNextItem("model",2,this.props.fetchModels);
               break;
            case "model":
                trackForMobile(trackingActionType.modelSelect, '')
                this.props.modelSelection({
                    id: id,
                    name: name,
                    rootName: event.currentTarget.getAttribute('data-rootname')
                });
                this.fetchNextItem("version",3,this.props.fetchVersions);
                break;
            case "version":
                trackForMobile(trackingActionType.versionSelect, '')
                this.props.versionSelection({ id: id, name: name, state: stateObj });
                this.carSelectionPopupBackClick();
                break;
            default:
                break;
        }
    }
    getHtmlListFromArray = (arr) => {
        if (arr.length) {
            let listElement = arr.map((item, index) => {
                return (
                    <li data-id={item.id}
                        data-name={item.name}
                        data-rootname= {item.rootName? item.rootName:''}
                        key={item.id}
                        onClick={this.handleListItemClick}
                    >
                        {item.name}
                    </li>
                );
            });
            return listElement;
        }
        else{
            return (<li><span>No record found</span></li>)
        }
    }
    getListDataToRender = () => {
        let data = [];
        // if filter applied, show data from filter
        if (this.props.car.data[this.props.car.state.type].isFilterApplied) {
            data = this.props.car.data[this.props.car.state.type].filteredData;
            return (
                <ul className="selection-filter__list">
                    {this.getHtmlListFromArray(data)}
                </ul>
            )
        }
        else if (this.props.car.state.type === "make") {
            let popularMakes = this.props.car.data[this.props.car.state.type].popular;
            let allMakes = this.props.car.data[this.props.car.state.type].all;
            return (
                <div>
                    <span className="selection-filter__title">Popular make </span>
                    <ul className="selection-filter__list">
                        {this.getHtmlListFromArray(popularMakes)}
                    </ul>
                    <span className="selection-filter__title">other make</span>
                    <ul className="selection-filter__list">
                        {this.getHtmlListFromArray(allMakes)}
                    </ul>
                </div>
            )
        } else {
            data = this.props.car.data[this.props.car.state.type].data;
            return (
                <ul className="selection-filter__list">
                    {this.getHtmlListFromArray(data)}
                </ul>
            )
        }
    }
    clearInput = () => {
        this.state.input  = '';
    }
    isFetchingData = () => {
        return this.props.car && (this.props.car.data.make.isFetching || this.props.car.data.model.isFetching || this.props.car.data.version.isFetching);
    }
    removeFilter = () =>{
        this.props.filterCar({ type: this.props.car.state.type, data: "",isFilterApplied:false });
    }
    carSelectionPopupBackClick = () => {
       this.removeFilter();
       this.clearInput();
       history.back();
    }
    getPopupHtml = () => {
        if (this.isFetchingData()) {
            return <SpeedometerLoader />
        } else {
            const percentage = this.state.selectionListHeight;
            const title = this.props.car ? this.props.car.data[this.props.car.state.type].title : this.state.title;
            let makeName = "", modelName = "";
            if (this.props.car) {
                makeName = this.props.car.selected.make.name;
                modelName = this.props.car.selected.model.name;
            }
            return (
                <div>
                    <div className="car-selection__popup-head">
                        <span className="car-selection-popup__back-arrow" onClick={this.carSelectionPopupBackClick}></span>
                        <p className="car-selection__popup-title">
                            select {title}
                        </p>
                    </div>
                    <div className="selection-popup__container">
                        <CarSelectionFilterToolbar
                            makeName={makeName}
                            modelName={modelName}
                            makeSelection={this.props.makeSelection}
                            modelSelection={this.props.modelSelection}
                            filterCar={this.props.filterCar}
                            clearInput ={this.clearInput}
                        />
                        <div
                            className="filter-container"
                            ref={this.setSelectionPopupList}>
                            <div className="selection-popup__input-container">
                                <input type="text"
                                    className="selection-popup__input"
                                    name="selection-popup__input"
                                    id="carSelection"
                                    value={this.state.input}
                                    placeholder={"Select " + title}
                                    onChange={this.onInputChange}
                                    ref={this.setSelectionPopupInputRef} />
                                <label
                                    htmlFor="carSelection"
                                    className="selection-popup__label"
                                >
                                </label>
                            </div>
                            <div className="selection-popup__filter-container" style={{ height: percentage }}>
                                {this.getListDataToRender()}
                            </div>
                        </div>
                    </div>
                </div>
            )
        }
    }

    render() {
        const active = this.props.car.state && this.props.car.state.active ? "selection-popup--active" : "";
        const title = this.props.car ? this.props.car.data[this.props.car.state.type].title : this.state.title;
        return (
            <div className={"car-selection__popup " + active}>
                {this.getPopupHtml()}
            </div>
        );
    }
}

CarSelectionPopup.propTypes = propTypes
CarSelectionPopup.defaultProps = defaultProps

export default CarSelectionPopup
