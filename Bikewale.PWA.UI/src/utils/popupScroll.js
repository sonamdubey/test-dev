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
    popup.head.style.position = 'absolute'
    popup.head.style.height = `${popup.headHeight}px`
  }
}

const handlePopupClose = (popupContent) => {
  popupContent.scrollTo(0,0)
}

const addPopupEvents = (popupContent) => {
  const popup = new Popup(popupContent)

  popupContent.addEventListener('scroll', handlePopupScroll.bind(this, popup))
  popupContent.querySelector('.popup__close').addEventListener('click', handlePopupClose.bind(this, popupContent))
}

const removePopupEvents = (popupContent) => {
  popupContent.removeEventListener('scroll', handlePopupScroll)
}

module.exports = {
  addPopupEvents,
  removePopupEvents
}
