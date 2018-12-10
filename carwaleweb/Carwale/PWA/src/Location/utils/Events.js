import {
  openPopup,
  closePopup,
  showCrossIcon,
  hideCrossIcon
} from '../actionCreators';
import {
  setCity,
  setLatLong,
  setArea,
  detectLocation,
  selectAreaCities
} from '../../../src/actionCreators/CityAutocomplete';

class Events {
  constructor(store) {
    this.dispatch = store.dispatch;
  }

  openPopup(areaCities) {
    this.dispatch(openPopup());
    if (areaCities && toString.call(areaCities) === "[object Array]") {
      this.dispatch(selectAreaCities(areaCities));
    }
  }

  closePopup() {
    this.dispatch(closePopup());
    }

  setCity(cityObj) {
      this.dispatch(setCity(cityObj));
  }

  setArea(areaObj){
    this.dispatch(setArea(areaObj));
  }

  setLatLong(latLongObj) {
    this.dispatch(setLatLong(latLongObj));
  }

  detectLocation(position) {
    this.dispatch(detectLocation(position));
  }

  hideCrossIcon() {
    this.dispatch(hideCrossIcon());
  }

  showCrossIcon() {
    this.dispatch(showCrossIcon());
  }
}
export default Events;
