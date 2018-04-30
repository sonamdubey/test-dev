import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { formatToINR } from '../../utils/formatAmount'

class EMICalculatorHeader extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
        <div className="emi-calci-header">
           <div className="emi-calci-top-header">
             <div className="emi-calci__orp-text">On-road price, Navi Mumbai</div>
             <div className="emi-calci__orp-data">
               <span>{formatToINR(1378592)}</span>
               <a href="" className="price-link">View detailed price</a>
             </div>
           </div>
        </div>
    );
  }
}

export default EMICalculatorHeader;
