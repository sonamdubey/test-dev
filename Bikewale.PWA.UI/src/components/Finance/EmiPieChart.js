import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { formatToINR } from '../../utils/formatAmount'

export default class PieChart extends React.Component {
	constructor(props){
		super(props)
		this.state = {
			data: [this.props.pieChartObjc.intStatePay, this.props.pieChartObjc.pieLoanAmt]
		}
	}
	componentWillReceiveProps(){
		this.setState({
			data: [this.props.pieChartObjc.intStatePay, this.props.pieChartObjc.pieLoanAmt]
		})
	}
	render(){
		let colors = ['#f45944', '#fed1cd'];
		let finalAmountPay = formatToINR(this.props.pieChartObjc.pieTotalPay)
		return(
			<div className="pie-graph-container">
			<div className="totalpayment">
				<div className="pie-breakup_title">Total Payment</div>
				<div className="pie-center-amount">{(finalAmountPay)}</div>
			</div>
			<Pie
				data={this.state.data}
				radius={70}
				hole={60}
				colors={colors}
				labels={true}
				percent={true}
				strokeWidth={1}
				stroke={'#fff'}
				isAnimation={this.props.isAnimation}
			/>
			</div>
		)
	}
}

let getAnglePoint = (startAngle, endAngle, radius, x, y) => {
	let x1, y1, x2, y2;
	x1 = x + radius * Math.cos(Math.PI * startAngle / 180);
	y1 = y + radius * Math.sin(Math.PI * startAngle / 180);
	x2 = x + radius * Math.cos(Math.PI * endAngle / 180);
	y2 = y + radius * Math.sin(Math.PI * endAngle / 180);
	return { x1, y1, x2, y2 };
}
let getRandomInt = (min, max) => {
	return Math.floor(Math.random() * (max - min)) + min;
}
class Pie extends React.Component{
	render() {
		let colors = this.props.colors,
			colorsLength = colors.length,
			labels = this.props.labels,
			hole = this.props.hole,
			radius = this.props.radius,
			diameter = radius * 2,
			self = this,
			sum, startAngle, d = null;
		sum = this.props.data.reduce(function (carry, current) { return carry + current }, 0);
		startAngle = 150;
		return (
			<svg width={ diameter } height={ diameter } viewBox={ '0 0 ' + diameter + ' ' + diameter } xmlns="http://www.w3.org/2000/svg" version="1.1">
				{ this.props.data.map(function (slice, sliceIndex) {
					let angle, nextAngle, percent;

					nextAngle = startAngle;
					angle = (slice / sum) * 360;
					percent = (slice / sum) * 100;
					startAngle += angle;

					return <Slice
						ref="slice"
						key={sliceIndex}
						value={slice}
						percent={self.props.percent}
						percentValue={percent.toFixed(1)}
						startAngle={nextAngle}
						angle={angle}
						radius={radius}
						hole={radius - hole}
						trueHole={hole}
						showLabel= {labels}
						fill={colors[sliceIndex % colorsLength]}
						stroke={self.props.stroke}
						strokeWidth={self.props.strokeWidth}
						isAnimation = {self.props.isAnimation}
					/>
				}) }

			</svg>
		)
	}
}
class Slice extends React.Component{
	constructor(props){
		super(props)
		this.state = {
			path: '',
			x: 0,
			y: 0
		}
	}
	componentWillReceiveProps (nextProps){
		this.setState({ path: '' });
		if(nextProps.isAnimation){
			this.animate();
		}
	}
	componentDidMount(){
		this.mounted = true;
		this.animate();
	}
	componentWillUnmount(){
		this.mounted = false;
	}
	animate = () =>  {
		this.draw(0);
	}
	draw = (s) =>  {
		if (!this.mounted) {
			return
		}
		let p = this.props, path = [], a, b, c, self = this, step;
		step = p.angle / (100 / 2);
		if (s + step > p.angle) {
			s = p.angle;
		}
		// Get angle points
		a = getAnglePoint(p.startAngle, p.startAngle + s, p.radius, p.radius, p.radius);
		b = getAnglePoint(p.startAngle, p.startAngle + s, p.radius - p.hole, p.radius, p.radius);

		path.push('M' + a.x1 + ',' + a.y1);
		path.push('A'+ p.radius +','+ p.radius +' 0 '+ (s > 180 ? 1 : 0) +',1 '+ a.x2 + ',' + a.y2);
		path.push('L' + b.x2 + ',' + b.y2);
		path.push('A'+ (p.radius- p.hole) +','+ (p.radius- p.hole) +' 0 '+ (s > 180 ? 1 : 0) +',0 '+ b.x1 + ',' + b.y1);

		// Close
		path.push('Z');

		this.setState({ path: path.join(' ') });

		if (s < p.angle) {
			setTimeout(function () { self.draw(s + step) } , 16);
		} else if (p.showLabel) {
			c = getAnglePoint(p.startAngle, p.startAngle + (p.angle / 2), (p.radius / 2 + p.trueHole / 2), p.radius, p.radius);

			this.setState({
				x: c.x2,
				y: c.y2
			});
		}
	}
	render(){
		return (
			<g>
				<path
					d={ this.state.path }
					fill={ this.props.fill }
					stroke={ this.props.stroke }
					strokeWidth={ this.props.strokeWidth ? this.props.strokeWidth : 3 }
				/>
				{/*{ this.props.showLabel && this.props.percentValue > 5 ?
					<text x={ this.state.x } y={ this.state.y } fill="#000" textAnchor="middle">
						{ this.props.percent ? this.props.percentValue + '%' : this.props.value }
					</text>
				: null }*/}
			</g>
		);
	}
}
