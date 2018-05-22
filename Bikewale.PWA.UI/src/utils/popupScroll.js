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
  popup.head.style.position = 'absolute'
  popup.head.style.height = `${popup.headHeight}px`
}

const handlePopupClose = (popupContent, popup) => {
  popupContent.scrollTo(0,0)
  resetPopupHead(popup)
}

const handlePopupTransitionEnd = (popupContent, popup, event) => {
  const targetElement = event.currentTarget;

  if (targetElement.classList.contains('popup-content') && !targetElement.classList.contains('popup--active')) {
    handlePopupClose(popupContent, popup);
  }
}

const addPopupEvents = (popupContent) => {
  const popup = new Popup(popupContent)

  popupContent.addEventListener('scroll', handlePopupScroll.bind(this, popup))
  popupContent.querySelector('.popup__close').addEventListener('click', handlePopupClose.bind(this, popupContent, popup))
  
  popupContent.closest('.popup-content').addEventListener('transitionend', handlePopupTransitionEnd.bind(this, popupContent, popup))
}

const removePopupEvents = (popupContent) => {
  popupContent.removeEventListener('scroll', handlePopupScroll)
  popupContent.closest('.popup-content').removeEventListener('transitionend', handlePopupTransitionEnd)
}

module.exports = {
  addPopupEvents,
  removePopupEvents
}
