import React from 'react'
import PropTypes from 'prop-types';
import { scrollIntoView } from '../../../utils/ScrollTo'
import { createRipple } from '../../../utils/Ripple'

const propTypes = {
   //on car selection item click
    onInputClick: PropTypes.func
}
const defaultProps = {
    status: 1,
    onInputClick: null
}

class CarSelectionContainer extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
          isValid: true
        }
    }
    setCarSelectionRef = element => {
        this.carSelection = element;
    };
    onPopState = () =>{
        this.props.carPopupStateSelection({ active: false });
        window.removeEventListener('popstate', this.onPopState);
    }
    onInputClick  = (event) => {
        if(event.currentTarget.classList.contains('list-item--active')) {
            createRipple(event)
            scrollIntoView(this.carSelection, event)
            let stateObj = { // used to show appropriate tile on popup
                type:event.currentTarget.getAttribute('data-type'),
                status:Number(event.currentTarget.getAttribute('data-status')),
            };
            if(!this.props.car.state.active){
                stateObj["active"] = true;
                this.props.showCarPopup(stateObj);
                window.addEventListener('popstate', this.onPopState);
                history.pushState("carSelectionPopUp","");
            }
            if(stateObj.type === 'make'){
                 // get api parameters depending on the selection of car button
                let options = this.props.getApiParameters(stateObj.type);
                stateObj["options"] = options;
                this.props.fetchMakes(stateObj);
            }
        }
    }
    render() {
        const validationStatus = this.props.car.selected.isValid ? '' : 'invalid';
        const status = this.props.car.state.status;
        const selected = this.props.car.selected;
        return (
            <div className={"car-selection__container"}>
                <p className="car-selection__title">Car</p>
                <ul ref={this.setCarSelectionRef} className="car-selection__list">
                    <li>
                        <div className={"car-selection-list__item " + ((status >= 1) ? "list-item--active "+(status==1 ? validationStatus :""): "")} onClick={this.onInputClick} data-type="make" data-status="1"> {/* TODO: add done class after selection */}
                            <span className={"car-item__label"}>
                                {selected.make.name ? "make" :"select"}
                            </span>
                            <p className="car-item__value">
                                {selected.make.name ? selected.make.name :"make"}
                            </p>
                        </div>
                    </li>
                    <li>
                        <div className={"car-selection-list__item " + ((status >= 2 || selected.model.id) ? "list-item--active "+(status==2 ? validationStatus :"") : "")} onClick={this.onInputClick} data-type="model" data-status="2"> {/* TODO: add done class after selection */}
                            <span className={"car-item__label"}>
                                {selected.model.name ? "model" :"select"}
                            </span>
                            <p className="car-item__value">
                                {selected.model.name ? selected.model.name :"model"}
                            </p>
                        </div>
                    </li>
                    <li>
                        <div className={"car-selection-list__item " + ((status >= 3 || selected.version.id) ? "list-item--active "+(status==3 ? validationStatus :"") : "")} onClick={this.onInputClick} data-type="version" data-status="3"> {/* TODO: add done class after selection */}
                            <span className={"car-item__label"}>
                                {selected.version.name ? "version" :"select"}
                            </span>
                            <p className="car-item__value">
                                {selected.version.name ? selected.version.name :"version"}
                            </p>
                        </div>
                    </li>
                </ul>
                <span className="error-text">{selected.errorText}</span>
            </div>
        )
    }
}

CarSelectionContainer.propTypes = propTypes
CarSelectionContainer.defaultProps  = defaultProps

export default CarSelectionContainer


