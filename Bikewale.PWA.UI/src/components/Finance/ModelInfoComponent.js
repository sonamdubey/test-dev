import React from 'react';
import Dropdown from 'react-dropdown';
import {createImageUrl} from '../Widgets/WidgetsCommon'
class ModelInfo extends React.Component {
  constructor(props) {
    super(props);
  }
  
  handleVersionChange = (option) => {
    // handle version change
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
            <img src={createImageUrl( model.hostUrl, model.originalImagePath, '310x174')} alt={model.modelName}/>
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
          <div className="model-info__col model-info-col--dropdown">
            <p className="model-info-col__label">Version</p>
            <Dropdown
              options={model.versionList}
              value={model.versionList[model.selectedVersionIndex]}
              placeholder="Version"
              placeholderClassName="model-info-col__value"
              arrowClassName="dropdown-version__arrow"
              onChange={this.handleVersionChange}
            />
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
