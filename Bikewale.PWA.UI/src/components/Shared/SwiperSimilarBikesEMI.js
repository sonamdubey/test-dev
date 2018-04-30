import React from 'react';
import LazyLoad from 'react-lazy-load'
import { Link } from 'react-router-dom'

class SwiperSimilarBikesEMI extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const {
      item
    } = this.props

    return (
      <div className="carousel-slide__card">
        <Link to="" title={item.makeName + ' ' + item.modelName} className="similar-emi-card__target">
          <div className="similar-emi-card__image">
            <LazyLoad>
              <img src={item.modelImage} alt="Honda CB Hornet 160R" />
            </LazyLoad>
          </div>
          <div className="similar-emi-card__detail">
            <h3 className="carousel-card-detail__title">{item.makeName + ' ' + item.modelName}</h3>
            <p className="carousel-card-detail__subtitle">{item.onRoadPriceLabel}</p>
            <p className="carousel-card-detail__amount">&#x20b9; <span className="carousel-text--semibold">{item.onRoadPriceAmount}</span></p>
          </div>
        </Link>
        <div className="similar-emi-card__footer">
          <p className="similar-emi-card-footer__title">
            <span className="carousel-text--semibold">{item.emiLabel}:&nbsp;</span>&#x20b9;&nbsp;<span class="carousel-text--bold">{item.emiStart}</span>&nbsp;Onwards</p>
        </div>
      </div>
    );
  }
}

export default SwiperSimilarBikesEMI;
