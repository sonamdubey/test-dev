import React from 'react';
import PropTypes from 'prop-types'
import { fetchSimilarBikes } from '../../actionCreators/SimilarBikesEMI'

const propTypes = {
  // list type
  type: PropTypes.string,
  // heading to show above carousel
  heading: PropTypes.string,
  // carousel data
  data: PropTypes.array,
  // custom carousel card component
  carouselCard: PropTypes.oneOfType([PropTypes.func, PropTypes.string])
}

const defaultProps = {
  type: '',
  heading: '',
  data: null,
  carouselCard: '' //TODO: Add default carousel card
}

class SwiperContainer extends React.Component {
  constructor(props) {
    super(props);
  }

  handleCardClick = (item) => {
    if(item.modelId > 0){
      this.props.onCarouselCardClick(item.modelId);
    }
  }
  getCarouselList = () => {
    const {
      type,
      data,
      carouselCard
    } = this.props

    let CarouselCard = carouselCard

    let list = data.map((item, index) => {
      return (
        <li onClick={this.handleCardClick.bind(this, item)} className="carousel__slide" key={index} >
          <CarouselCard item={item}/>
        </li>
      )
    })

    return (
      <ul className={"carousel__wrapper " + type}>
        { list }
      </ul>
    )
  }

  render() {
    return (
      <div className="carousel__container section-bottom-margin">
        <div className="carousel__title">
          <h2 className="carousel-title__label">{this.props.heading}</h2>
        </div>
        {this.getCarouselList()}
      </div>
    );
  }
}

SwiperContainer.propTypes = propTypes;
SwiperContainer.defaultProps = defaultProps;

export default SwiperContainer;
