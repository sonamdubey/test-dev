import React from 'react';

import EMICalculatorHeader from './EMICalculatorHeader'
import DownPaymentSlider from './DownPaymentSlider'
import TenureSlider from './TenureSlider'
import InterestSlider from './InterestSlider'

class EMICalculator extends React.Component {
  constructor(props) {
    super(props);

  }

  render() {
    return (
      <div className="emi-outer-container">
        <div className="emi-calci-container">
           <EMICalculatorHeader />
           <div className="emi-slider-container">
              <DownPaymentSlider/>
              <div className="tenure-interest-slider">
                <div className="slider-input-container tenure-unit">
                  <TenureSlider/>
                </div>
                <div className="slider-input-container interest-unit">
                  <InterestSlider/>
                </div>
              </div>
            </div>
            
        </div>
      </div>
    );
  }
}

export default EMICalculator;
