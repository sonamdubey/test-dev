import React from 'react';

import { addTabEvents, removeTabEvents } from '../../utils/scrollSpyTabs';

import EMICalculator from './EMICalculator'

class Tabs extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      selectBikePopupStatus: false
    }
  
    this.setRef = this.setRef.bind(this);
  }

  componentDidMount() {
    addTabEvents(this.tabsContainer);
  }

  componentWillUnmount() {
    removeTabEvents(this.tabsContainer);
  }

  setRef(ref) {
    this.tabsContainer = ref;
  }

  render() {
    return (
      <div ref={this.setRef} className="tabs__container">
        <div className="tabs__placeholder">
          <div className="tabs__content">
            <ul className="tabs__list">
              <li className="tabs-list__item" data-tab="overviewTab">Overview</li>
              <li className="tabs-list__item" data-tab="emiTab">EMI Calculator</li>
              <li className="tabs-list__item" data-tab="loanEligibilityTab">Loan Eligibility Predictor</li>
              <li className="tabs-list__item" data-tab="documentationTab">Documentation</li>
              <li className="tabs-list__item" data-tab="faqTab">FAQ</li>
            </ul>
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="overviewTab">
          <div className="overview__content">
            Overview: Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike. Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike. Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike.
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="emiTab">
          <div className="overview__content">
            <EMICalculator />
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="loanEligibilityTab">
          <div className="overview__content">
            Loan Eligibility Predictor: Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application.
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="documentationTab">
          <div className="overview__content">
            Documentation: Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. 
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="faqTab">
          <div className="overview__content">
            FAQ: Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application. Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application.
          </div>
        </div>

      </div>
    );
  }
}

export default Tabs;