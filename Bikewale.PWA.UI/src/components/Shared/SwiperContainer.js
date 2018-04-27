import React from 'react';
import PropTypes from 'prop-types'

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
  carouselCard: ''
}

class SwiperContainer extends React.Component {
  constructor(props) {
    super(props);
  }

  getCarouselList = () => {
    const {
      type,
      data,
      carouselCard
    } = this.props

    let CarouselCard = carouselCard

    let list = data.map((item) => {
      return (
        <li className="carousel__slide" key={item.modelId}>
          <CarouselCard item={item} />
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
