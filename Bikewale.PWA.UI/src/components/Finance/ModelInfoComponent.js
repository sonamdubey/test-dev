import React from 'react';
import Dropdown from 'react-dropdown';
import SpinnerRelative from '../Shared/SpinnerRelative';
import { openPopupWithHash } from '../../utils/popUpUtils'
import {createImageUrl} from '../Widgets/WidgetsCommon'
import { triggerGA } from '../../utils/analyticsUtils'

class ModelInfo extends React.Component {
  constructor(props) {
    super(props);
    this.handleVersionChange = this.handleVersionChange.bind(this);
    this.handleModelPopupClick = this.handleModelPopupClick.bind(this);
    this.handelCityPopupClick = this.handelCityPopupClick.bind(this);
  }
  
  handleVersionChange = (option) => {
    this.props.selectBikeVersion(option.value);
  }
  handleModelPopupClick = () => {
    if (gaObj != undefined) {
      triggerGA(gaObj.name, 'Model_Change_Initiated', 'Existing Model - ' + this.props.model.modelName); 
    }
    openPopupWithHash(this.props.openSelectBikePopup, this.props.closeSelectBikePopup, "SelectBike");
  }
  handelCityPopupClick = () => {
    if (gaObj != undefined) {
      triggerGA(gaObj.name, 'City_Change_Initiated', 'Existing City - ' + this.props.city.cityName); 
    }
    openPopupWithHash(this.props.openSelectCityPopup, this.props.closeSelectCityPopup, "SelectCity");
  }

  render() {
    const {
      model,
      city,
      openSelectCityPopup,
      openSelectBikePopup,
      closeSelectCityPopup,
      closeSelectBikePopup,
      isLoaderShown
    } = this.props

    if(!model) {
      return null;
    }
    if(isLoaderShown) {
      return <SpinnerRelative />;
    }
    const imageHostUrl = model.versionList != null && model.versionList.length > 0 && model.selectedVersionIndex > -1 ? model.versionList[model.selectedVersionIndex].hostUrl: model.hostUrl;
    const originalImagePath = model.versionList != null && model.versionList.length > 0 && model.selectedVersionIndex > -1 ? model.versionList[model.selectedVersionIndex].originalImagePath: model.originalImagePath; 
    return (
      <div className="model-info-card">
        <div className="model-info-card__head">
          <div className="model-info-card__image">
            <img src={createImageUrl( imageHostUrl, originalImagePath, '310x174')} alt={model.modelName}/>
          </div>
          <div className="model-info-card__detail">
            <h3>
              <span className="model-info-card__make">{model.makeName}</span>
              <span className="model-info-card__model">{model.modelName}</span>
            </h3>
          </div>
          <span onClick={this.handleModelPopupClick} className="model-info-card__edit"></span>
        </div>
        <div className="model-info-card__body">
          <div className="model-info__col model-info-col--dropdown">
            <p className="model-info-col__label">Version</p>
            <Dropdown
              options={model.versionList}
              value={model.versionList != null && model.versionList.length > 0 && model.selectedVersionIndex > -1 ? model.versionList[model.selectedVersionIndex].label : ""}
              placeholder="Version"
              placeholderClassName="model-info-col__value"
              arrowClassName="dropdown-version__arrow"
              onChange={this.handleVersionChange}
            />
          </div>
          <div className="model-info__col">
            <p className="model-info-col__label">City</p>
            <p onClick={this.handelCityPopupClick} className="model-info-col__value model-info-col__city">{city.cityName}</p>
          </div>
        </div>
      </div>
    )
  }
}

export default ModelInfo;
