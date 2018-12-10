import React from 'react'
import {
    connect
} from 'react-redux'
import {
    bindActionCreators
} from 'redux'
import {
    vehicleData
} from '../reducers/VehicleData'
import {
    vehicleTenure
} from '../reducers/emiTenure'
import TenureSlider from './TenureSlider'
import PieBreakUp from './PieBreakUp'
import Collapsible from '../../components/Collapsible'
import styles from '../../../style/prices.scss';

import {
    updateDownPaymentInput
} from '../actionCreators/emiDownPaymentSlider'

import {
    updateTenureInput
} from '../actionCreators/emiTenureSlider'

import {
    EmiCalculation,
    InterestPayable,
    TotalPrincipalAmt
} from './EmiCalculator'

import {
    lockScroll,
    unlockScroll
} from './../../utils/ScrollLock'

import {
    formatToINR
} from './../../utils/amountFormat'

import {
    showHideBreakup
} from '../containers/EmiPrices'

import {
    getModelData
} from '../utils/Prices'

import {
    isDesktop
} from '../../utils/Common'

import {
    trackingCategory
} from '../constants';

import {
    fireInteractiveTracking
} from '../../utils/Analytics';

import {
    setIsEmiValid
} from '../actionCreators/emiData';

const bodyLock = () => {
    document.body.classList.add('bodylock')
    document.body.classList.remove('bodyunlock')
}
const bodyUnlock = () => {
    document.body.classList.add('bodyunlock')
    document.body.classList.remove('bodylock')
}
class EmiHeader extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            EmiCalculation: EmiCalculation(this.props.sliderDP, this.props.sliderTenure, this.props.sliderInt),
            InterestPayable: InterestPayable(this.props.sliderDP, this.props.sliderTenure, this.props.sliderInt),
            TotalPrincipalAmt: TotalPrincipalAmt(this.props.sliderDP, this.props.sliderTenure, this.props.sliderInt),
            addClass: false,
            toggle: false,
            emiTogglePopUp: false,
            isValid: true,
            collpasibleTitle: '',
            pieReveal: isDesktop ? 100 : 0
        }
    }
    componentWillReceiveProps(nextProps) {
        let isDPMinValid, isDPMaxValid, isTenureMinValid, isTenureMaxValid, isIntMinValid, isIntMaxValid;
        isDPMinValid = nextProps.inputBoxDP.value < nextProps.sliderDP.min;
        isDPMaxValid = nextProps.inputBoxDP.value > nextProps.sliderDP.max;
        isTenureMinValid = nextProps.inputBoxTenure.value < nextProps.sliderTenure.min;
        isTenureMaxValid = nextProps.inputBoxTenure.value > nextProps.sliderTenure.max;
        isIntMinValid = nextProps.inputBoxInt.value < nextProps.sliderInt.min;
        isIntMaxValid = nextProps.inputBoxInt.value > nextProps.sliderInt.max;

        let isDPValid, isTenureValid, isIntValid, inValid;
        isDPValid = !(isDPMinValid || isDPMaxValid);
        isTenureValid = !(isTenureMinValid || isTenureMaxValid);
        isIntValid = !(isIntMinValid || isIntMaxValid);
        inValid = !(isDPValid && isTenureValid && isIntValid);

        let title;
        if (!inValid) {
            title = (isDesktop ? "" : "View") + " Payment Break-Up";
            if (!nextProps.isEmiValid) {
                nextProps.setIsEmiValid(true);
            }
        } else {
            let state, element;
            if (nextProps.isEmiValid) {
                nextProps.setIsEmiValid(false);
            }
            if (!isDPValid) {
                state = (isDPMinValid) ? "increase" : "reduce";
                element = "Down Payment"
            } else if (!isTenureValid) {
                state = (isTenureMinValid) ? "increase" : "reduce";
                element = "Tenure"
            } else if (!isIntValid) {
                state = (isIntMinValid) ? "increase" : "reduce";
                element = "Interest"
            }
            title = "Please " + state + " the " + element + ".";
        }
        this.setState({
            EmiCalculation: inValid ? 0 : EmiCalculation(nextProps.sliderDP, nextProps.sliderTenure, nextProps.sliderInt),
            InterestPayable: inValid ? 0 : InterestPayable(nextProps.sliderDP, nextProps.sliderTenure, nextProps.sliderInt),
            TotalPrincipalAmt: inValid ? 0 : TotalPrincipalAmt(nextProps.sliderDP, nextProps.sliderTenure, nextProps.sliderInt),
            isValid: !inValid,
            collpasibleTitle: title
        });
    }
    yearStringCheck = () => {
        let yeartext = ""
        if ((this.props.tenureValue) > 1) {
            yeartext = "Years"
        } else {
            yeartext = "Year"
        }
        return (
            <span className = "year-text" > {(yeartext)} </span>
        )
    }
    toggleOverlay = () => {
        let toggle, addClass;

        if (!isDesktop) {
            addClass = !this.state.addClass;
            toggle = !this.state.toggle;
        } else {
            toggle = false;
            addClass = false;
        }

        this.setState({
            addClass: addClass,
            toggle: toggle,
        });
        this.pieRevealState();
    }
    removeOverlay = () => {
        this.setState({
            addClass: false,
            toggle: false,
            pieReveal: 0
        });
    }
    pieRevealState = () => {
        let revealDataState = (!isDesktop && this.state.pieReveal === 0) ? 100 : 0;
        this.setState({
            pieReveal: revealDataState
        })
    }
    toggleLockScroll = () => {
        !this.state.toggle ? bodyLock() : bodyUnlock()
        fireInteractiveTracking(trackingCategory, "ViewPaymentBreakup_Link_Clicked", "");
    }
    toggleEmiPopupNew = () => {
        EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.hideEmiPopupState(false))
        this.removeOverlay();
        bodyUnlock();
    }
    render() {
        const PieInterestAmt = this.state.InterestPayable
        const PieTotalAmtPay = this.state.TotalPrincipalAmt
        const PieLoanAmt = !this.state.isValid ? 0 : parseFloat(this.props.sliderDP.max - this.props.sliderDP.values[0]);
        let {
            inputBoxDP
        } = this.props
        const {
            state
        } = this.props;
        let finalEmi = formatToINR(this.state.EmiCalculation)
        let finalIntPayable = formatToINR(parseInt(this.state.InterestPayable))
        let finalTotalPrincipalAmt = formatToINR(this.state.TotalPrincipalAmt)
        let overlayClass = ["black-overlay"];
        if (this.state.addClass) {
            overlayClass.push('overlayactive');
            bodyLock()
        } else {
            bodyUnlock()
        }
        const viewBreakupCollapsibleProps = {
            title: this.state.collpasibleTitle,
            onToggle: this.toggleOverlay,
            open: this.state.addClass ? true : (isDesktop ? true : false),
            classname: !(this.state.isValid) ? 'accordion__head--error' : ''
        }
        return (
            <div className="outer-emi-section">
            <div className="emi-header-container">
                <span className="emi-close white-close-icon" id="emi-close" onClick={this.toggleEmiPopupNew}></span>
                <div className="vehicle-name">
                    <div key={vehicleData.id}>
                        <span>{this.props.makeName}</span>
                        <span>{this.props.modelName}</span>
                        <span>{this.props.versionName}</span>
                    </div>
                </div>
                <div className="emi-cost-container">
                    <div className="emi-text">EMI</div>
                    <div className="emi-total-cost">
                        <span className="final-emi-text">{finalEmi}</span>
                        <span className="emi-total-year">for <span className="emi-total-year-text">{this.props.tenureValue} {this.yearStringCheck()}</span></span>
                    </div>
                </div>
            </div>
            <div className="collapse-section">
                <div className="pie-collapse" onClick={this.toggleLockScroll}><Collapsible {...viewBreakupCollapsibleProps}>
                    <div className="view-breakup-container">
                        <PieBreakUp ref="piebreakup" isAnimation={viewBreakupCollapsibleProps.open} intStatePay={PieInterestAmt} pieLoanAmt={PieLoanAmt} pieTotalPay={PieTotalAmtPay} interestPay={finalIntPayable} TotalPrincipalAmt={finalTotalPrincipalAmt} pieReveal={this.state.pieReveal}/>
                    </div>
                </Collapsible></div>
                <div className={overlayClass.join(' ')} onClick={this.removeOverlay}></div>
            </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    let activeModel = getModelData(state.newEmiPrices)
    const {
        makeName,
        modelName,
        versionName
    } = activeModel.data.vehicleData
    const {
        slider: sliderDP,
        inputBox: inputBoxDP
    } = activeModel.data.vehicleDownPayment
    const {
        slider: sliderTenure,
        inputBox: inputBoxTenure
    } = activeModel.data.vehicleTenure
    const {
        slider: sliderInt,
        inputBox: inputBoxInt
    } = activeModel.data.vehicleInterest
    const {isEmiValid} = activeModel.data
    return {
        makeName,
        modelName,
        versionName,
        sliderDP,
        sliderTenure,
        inputBoxTenure,
        inputBoxDP,
        sliderInt,
        inputBoxInt,
        isEmiValid
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        setIsEmiValid: bindActionCreators(setIsEmiValid, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(EmiHeader)
