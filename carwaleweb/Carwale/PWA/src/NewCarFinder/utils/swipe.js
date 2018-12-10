function Position(event, popupContent) {
  let self = this
  self.positionX = 0
  self.positionY = 0
  self.nextPositionX = 0
  self.nextPositionY = 0
  self.touchStart = function(event) {
      if(event.touches) {
        self.positionX = event.touches[0].clientX,
        self.positionY = event.touches[0].clientY
      }
  }
  self.touchMove = function(event) {
    if(event.touches) {
      self.nextPositionX = event.touches[0].clientX,
      self.nextPositionY = event.touches[0].clientY
    }

  }
}
let position
const addTouchStartEvent = (event, popupContent) => {
  position = new Position(event, popupContent)
  position.touchStart(event)
}
const addTouchMoveEvent = (event, onLeftSwipe, onRightSwipe) => {
  position.touchMove(event)
  let posXDiff = position.positionX - position.nextPositionX
  let posYDiff = position.positionY - position.nextPositionY
  if(Math.abs(posXDiff) > Math.abs(posYDiff)) { //horizontal swipe
    if(posXDiff > 0) {
      onRightSwipe(event)
    }
    else if (posXDiff < 0) {
      onLeftSwipe(event)
    }
  }

}

module.exports = {
  addTouchStartEvent,
  addTouchMoveEvent
}
