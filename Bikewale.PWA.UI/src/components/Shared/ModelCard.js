import React from 'react';
import PropTypes from 'prop-types';

const ModelCard = (props) => {
  const { model } = props

  return (
    <div className="model-card__placeholder">
      <div className="model-card__content">
        <div className="model-card__image">
          <img src={model.modelImage} alt={model.modelName} />
        </div>
        <div className="model-card__detail">
          {
            model.rating > 0 && (
              <span class="rating-badge-sm" data-rate-bg={Math.floor(model.rating)}>
                <span class="rating-badge__star"></span>
                <span>{model.rating.toFixed(1)}</span>
              </span>
            )
          }
          <h3>
            <span className="model-card__make">{model.makeName}</span>
            <span className="model-card__model">{model.modelName}</span>
          </h3>
        </div>
      </div>
    </div>
  )
}

ModelCard.propTypes = {
  model: PropTypes.shape({
    makeName: PropTypes.string,
    modelName: PropTypes.string,
    modelImage: PropTypes.string,
    rating: PropTypes.number
  })
}

ModelCard.defaultProps = {
  model: {}
}

export default ModelCard;
