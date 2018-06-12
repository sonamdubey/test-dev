import React from 'react';

import Accordion from '../Shared/Accordion';
import NoResult from './NoResult';
import SpinnerRelative from '../Shared/SpinnerRelative';
import { unlockScroll } from '../../utils/scrollLock';
import { addPopupEvents,removePopupEvents, focusCollapsible, focusPopupContent } from '../../utils/popupScroll';
import { closePopupWithHash } from '../../utils/popUpUtils';
import { triggerGA } from '../../utils/analyticsUtils';



class SelectBikePopup extends React.Component {
  constructor(props) {
    super(props);
    this.filterMakeModelList = this.filterMakeModelList.bind(this);
    this.handleClear = this.handleClear.bind(this);
    this.handleCloseClick = this.handleCloseClick.bind(this);
    this.closePopup = this.closePopup.bind(this);
    this.resetSearchMode = this.resetSearchMode.bind(this);
    this.state = { currentModelId: this.props.data.Selection.modelId, modelValue: this.props.data.Selection.modelName, makeModelList: this.props.data.MakeModelList };
  }

  componentWillReceiveProps(nextProps) {
    this.setState({ currentModelId: nextProps.data.Selection.modelId, modelValue: nextProps.data.Selection.modelName, makeModelList: nextProps.data.MakeModelList });
  }

  componentWillMount() {
    if (!this.props.data.MakeModelList || !this.props.data.MakeModelList.length) {
      this.props.fetchMakeModelList();
    }
  }

  componentDidMount() {
    addPopupEvents(this.popupContent)
  }

  componentWillUnmount() {
    removePopupEvents(this.popupContent)
  }
  
  shouldComponentUpdate(nextProps) {
    if (nextProps.isActive === this.props.isActive) {
      return false;
    }

    if (nextProps.isActive) {
      this.popupContent.scrollTop = this.popupScrollPosition
    }

    return true;
  }

  closePopup = () => {
    this.popupScrollPosition = this.popupContent.scrollTop;

    if (this.props.onCloseClick) {
      closePopupWithHash(this.props.onCloseClick);
    }
  }

  resetSearchMode = () => {
    this.setState({ ...this.state, searchMode: false });
  }

  handleCloseClick = () => {
    if (gaObj != undefined) {
      triggerGA(gaObj.name, 'Model_Popup_Cross_Clicked', this.state.modelValue); 
    }
    this.closePopup();
  }

  handleBikeSelection = (chosenModel) => {
    this.closePopup();
    if (gaObj != undefined) {
      triggerGA(gaObj.name, 'Model_Selected', chosenModel.modelName + '_' + this.state.modelValue); 
    }
    if(this.state.currentModelId > 0) {
      if (gaObj != undefined) {
        triggerGA(gaObj.name, 'Model_Selected_On_Edit_Flow', 'Existing Model - '+ this.props.data.Selection.modelName); 
      }
    }
    if (this.state.currentModelId != chosenModel.modelId) {
      this.props.onBikeClick(chosenModel);
      this.setState({ ...this.state, currentModelId: chosenModel.modelId, modelValue: chosenModel.modelName });
    }
  }

  handleClear = () => {
    this.setState({ ...this.state, modelValue: '', makeModelList: this.props.data.MakeModelList, searchMode: false });
  }

  filterMakeModelList = (event) => {
    let inputName = event.target.value;
    let modelList = event && !event.data ? this.props.data.MakeModelList : this.state.makeModelList;
    let searchMode = event && inputName != null && inputName.length > 0 ? true : false;
    let makeModelList = [];
    if (modelList != null) {
      makeModelList = modelList.map(function (item) {
        let modelList = item.models.filter(function (bike) {
          return bike.modelName.toLowerCase().indexOf(
            inputName.toLowerCase())!== -1; 
        });
        if (modelList && modelList.length > 0) {
          return {
            ...item, models: modelList
          };
        }
        else {
          return null;
        }
      }).filter(n => n);
    }
    this.setState({
      ...this.state,
      makeModelList: makeModelList,
      modelValue: inputName,
      searchMode: searchMode
    }, () => {
      focusPopupContent(this.popupContent);
    });
  }
  
  handleAccordionClick = (event) => {
    focusCollapsible(this.popupContent, event)
  }

  getList = (makeModelList) => {
    if (!makeModelList || !makeModelList.length) {
      return [];
    }
    let list = makeModelList.map(function (item) {
      if (!item) {
        return null;
      }
      return (
        <div data-trigger={item.make.makeName} data-onOpen={this.handleAccordionClick.bind(this)}>
          <ul className="panel-body__list">
            {item.models.map(function (bike) {
              let eleClassName = `panel-bike-list__item ${(bike.modelId == this.state.currentModelId ? " bike-list-item--active" : "")}`;
              return (
                <li key={bike.modelId} className={eleClassName} onClick={this.handleBikeSelection.bind(this, bike)}>
                  <p className="bike-list-item__label">{bike.modelName}</p>
                </li>
              );
            }, this)}
          </ul>
        </div>
      );
    }, this);

    return (
      list
    )
  }

  setContentRef = (ref) => {
    this.popupContent = ref
  }


  render() {
    const {
      isActive,
      data
    } = this.props
    const MakeModelList = this.state.makeModelList;
    const popupActiveClassName = isActive ? 'popup--active' : ''
    const popupClasses = `select-bike-popup popup-content ${popupActiveClassName}`
    let result = null;
    if (MakeModelList != undefined && MakeModelList.length != 0) {
      result = <Accordion
        closeable={true}
        allOpen={this.state.searchMode}
        transitionCloseTime={1}
        items={this.getList(MakeModelList)}
      />;
    }
    else if (data.MakeModelList != undefined && data.MakeModelList.length == 0) {
      result = <SpinnerRelative />;
    }
    else {
      result = <NoResult
        type="select-bike__no-bike-content"
        imageClass="select-bike__no-bike"
        title="No Matching Bikes Found"
      />;
    }
    return (
      <div className={popupClasses}>
        <div ref={this.setContentRef} className="select-bike-popup__content">
          <div className="popup__head">
            <div className="popup-head__content">
              <span onClick={this.handleCloseClick} className="popup__close"></span>
              <div className="popup__search-box">
                <p className="popup-search__title">Select Make and Model</p>
                <div className="autocomplete-box">
                  <div className="autocomplete-field">
                    <input type="text" value={this.state.modelValue} className="form-control"
                      placeholder="Type to select Make and Model" onChange={this.filterMakeModelList} onFocus={this.filterMakeModelList} onBlur={this.resetSearchMode}/>
                    <span className="autocomplete-box__clear" onClick={this.handleClear} >Clear</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="select-bike__body select-bike__accordion">
            {result}
          </div>
        </div>
      </div>
    );
  }
}

export default SelectBikePopup;
