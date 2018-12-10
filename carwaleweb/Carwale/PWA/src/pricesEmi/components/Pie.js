import React from 'react'
import PieChart from 'react-minimal-pie-chart'
import PropTypes from 'prop-types';

import {
	isDesktop
} from '../../utils/Common'

const propTypes = {
  //The source data which each element is a segment.
  data: PropTypes.arrayOf(
    PropTypes.shape({
      value: PropTypes.number.isRequired,
      key: PropTypes.oneOfType([
        PropTypes.number,
        PropTypes.string,
      ]),
      color: PropTypes.string,
    })
  ),
  // The angle between two sectors
  paddingAngle: PropTypes.number,
  animate: PropTypes.bool,
  animationDuration: PropTypes.number,
  animationEasing: PropTypes.string,
  reveal: PropTypes.number,
}
const defaultProps = {
  placeHolderData: [{
    value: 1,
    color: '#e1e1e1'
  }],
  radius: 45,
  lineWidth: 20,
  startAngle: 160,
  paddingAngle: 0,
  animate: true,
  animationDuration: 1000,
  animationEasing: 'ease-out',
  reveal: 100
}

export default class Pie extends React.Component {
  constructor(props){
      super(props)
  }
  render() {
    let {
      radius,
      lineWidth,
      startAngle,
      paddingAngle,
      animate,
      animationDuration,
      animationEasing,
      reveal
    } = this.props;

    let data = this.props.data;
    let count = 0;
    let countIndex = 0;
    this.props.data.map((item, index) => {
      if(item.value == 0) {
        count++;
      }
      countIndex = index;
    });
    if (count == countIndex + 1) {
      data = this.props.placeHolderData;
      paddingAngle = 0;
    }
    return (
    <PieChart
      data={data}
      radius={radius}
      lineWidth={lineWidth}
      startAngle={startAngle}
      paddingAngle={paddingAngle}
      animate={animate}
      animationDuration={1000}
      animationEasing={animationEasing}
      reveal={reveal}
    />
    )
  }
}
Pie.propTypes = propTypes;
Pie.defaultProps = defaultProps;
