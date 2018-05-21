import React from 'react';
import ExpandCollapse from 'react-expand-collapse';

import { addTabEvents, removeTabEvents } from '../../utils/scrollSpyTabs';

import EMITab from './EMITabContainer'
import Documentation from './Documentation'
import FAQ from './FAQ'

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
            <ExpandCollapse previewHeight="66px" expandText="Read more">
              Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike. Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike. Bike loan EMI calculation was never this easy. Just select the bike you wish to avail loan for and which city do you wish to purchase the bike.
            </ExpandCollapse>
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="emiTab">
          <div className="emi-calculator">
            <EMITab />
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="documentationTab">
          <div className="finance-documentation">
            <Documentation />
          </div>
        </div>

        <div className="tabs-panel__item" data-tab-panel="faqTab">
          <div className="finance-faq">
            <FAQ />
          </div>
        </div>

      </div>
    );
  }
}

export default Tabs;