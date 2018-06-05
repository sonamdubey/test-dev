import closest from './closestPolyfill'

function Popup(popupContent) {
  let self = this;

  self.content = popupContent
  self.head = self.content.querySelector('.popup__head')
  self.headHeight = self.head.clientHeight
  self.searchBoxHeight = self.content.querySelector('.popup__search-box').clientHeight
  self.heightThreshold = self.headHeight - self.searchBoxHeight
}

const handlePopupScroll = (popup) => {
  if (popup.content.scrollTop > popup.heightThreshold) {
    popup.head.style.position = 'fixed'
    popup.head.style.height = `${popup.searchBoxHeight}px`
  }
  else {
    resetPopupHead(popup)
  }
}

const resetPopupHead = (popup) => {
  popup.head.style.position = ''
  popup.head.style.height = ''
}

const handlePopupClose = (popupContent, popup) => {
  popupContent.scrollTop = 0;
  resetPopupHead(popup)
}

// scroll popup to the clicked collapsible element
const focusCollapsible = (popupContent, event) => {
  const eventRect = event.currentTarget.getBoundingClientRect();
  const collapsibleTriggerHeight = event.currentTarget.closest('.collapsible').querySelector('.collapsible__trigger').offsetHeight;
  const searchBoxHeight = popupContent.querySelector('.popup__search-box').offsetHeight;
  const autocompleteBoxHeight = popupContent.querySelector('.autocomplete-box').offsetHeight;

  popupContent.scrollTop = eventRect.top + popupContent.scrollTop - (searchBoxHeight + collapsibleTriggerHeight + (autocompleteBoxHeight / 1.8))
}

// scroll popup content on autocomplete search
const focusPopupContent = (popupContent) => {
  const popupHead = popupContent.querySelector('.popup__head');
  const autocompleteBox = popupHead.querySelector('.autocomplete-box');
  const autocompleteBoxHeight = autocompleteBox ? autocompleteBox.offsetHeight : 0;

  if (popupContent.scrollTop > popupHead.offsetHeight) {
    popupContent.scrollTop = popupHead.offsetHeight - autocompleteBoxHeight;
  }
}

const addPopupEvents = (popupContent) => {
  const popup = new Popup(popupContent)

  resetPopupHead(popup);
  popupContent.addEventListener('scroll', handlePopupScroll.bind(this, popup))
  popupContent.querySelector('.popup__close').addEventListener('click', handlePopupClose.bind(this, popupContent, popup))
}

const removePopupEvents = (popupContent) => {
  popupContent.removeEventListener('scroll', handlePopupScroll)
  popupContent.querySelector('.popup__close').removeEventListener('click', handlePopupClose)
}

module.exports = {
  addPopupEvents,
  removePopupEvents,
  focusCollapsible,
  focusPopupContent
}
