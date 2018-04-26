import React from 'react';

class ModelInfo extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const {
      model,
      openSelectBikePopup,
      openSelectCityPopup
    } = this.props

    if(!model) {
      return null;
    }

    return (
      <div className="model-info-card">
        <div className="model-info-card__head">
          <div className="model-info-card__image">
            <img src={model.modelImage} alt={model.modelName}/>
          </div>
          <div className="model-info-card__detail">
            <h3>
              <span className="model-info-card__make">{model.makeName}</span>
              <span className="model-info-card__model">{model.modelName}</span>
            </h3>
          </div>
          <span onClick={openSelectBikePopup} className="model-info-card__edit"></span>
        </div>
        <div className="model-info-card__body">
          <div className="model-info__col">
            <p className="model-info-col__label">Version</p>
            <p className="model-info-col__value">Standard</p>
          </div>
          <div className="model-info__col">
            <p className="model-info-col__label">City</p>
            <p onClick={openSelectCityPopup} className="model-info-col__value model-info-col__city">Mumbai</p>
          </div>
        </div>
      </div>
    )
  }
}

export default ModelInfo;
