import React from 'react'
import PropTypes from 'prop-types'

class RefineResult extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<div className="refine-result__content">
				<div className="refine-result__head">
					<h3 className="refine-result__title">Refine your results</h3>
					<p className="refine-result__subtitle">Select a fuel type based on your driving pattern</p>
				</div>
				<div className="refine-result__body">
					<ul className="refine-result__list">
						<li className="refine-result-list__item item--active">
							<div className="refine-result-item__content">
								<p className="result-item__title">Petrol</p>
								<p className="result-item__subtitle">Monthly running > 1200kms</p>
							</div>
						</li>
						<li className="refine-result-list__item">
							<div className="refine-result-item__content">
								<p className="result-item__title">Diesel</p>
								<p className="result-item__subtitle">Monthly running > 1500kms</p>
							</div>
						</li>
						<li className="refine-result-list__item">
							<div className="refine-result-item__content">
								<p className="result-item__title">Electric</p>
								<p className="result-item__subtitle">Lorem Ipsum</p>
							</div>
						</li>
						<li className="refine-result-list__item">
							<div className="refine-result-item__content">
								<p className="result-item__title">CNG</p>
								<p className="result-item__subtitle">Lorem Ipsum</p>
							</div>
						</li>
					</ul>
				</div>
				<div className="refine-result__footer">
					<button type="button" className="btn btn-secondary">Apply</button>
				</div>
			</div>
		)
	}
}

export default RefineResult
