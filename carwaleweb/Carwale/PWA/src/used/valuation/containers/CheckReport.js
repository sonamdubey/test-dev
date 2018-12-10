import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import CheckReport from '../components/CheckReport';
import { getValuation } from '../actionCreators/CheckReport';

const mapDispatchToProps = dispatch => {
    return {
        getValuation: bindActionCreators(getValuation, dispatch),
    }
}

export default connect(null, mapDispatchToProps)(CheckReport)
