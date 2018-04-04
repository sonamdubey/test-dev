/*
 * ScrollTo animation using pure javascript
 * https://gist.github.com/andjosh/6764939
 */

export const scrollTop = (element, to, duration = 500) => {
  const isElementWindow = element === window ? true : false;

  let start = isElementWindow ? (window.pageYOffset || document.documentElement.scrollTop) : element.scrollTop,
    change = to - start,
    currentTime = 0,
    increment = 20

  let animateScroll = function () {
    currentTime += increment
    let val = Math.easeInOutQuad(currentTime, start, change, duration)
    if (isElementWindow) {
      element.scrollTo(0, val)
    }
    else {
      element.scrollTop = val
    }

    if (currentTime < duration) {
      setTimeout(animateScroll, increment)
    }
  }

  animateScroll()
}

//t = current time
//b = start value
//c = change in value
//d = duration
Math.easeInOutQuad = (t, b, c, d) => {
  t /= d / 2
  if (t < 1) {
    return c / 2 * t * t + b
  }
  t--
  return -c / 2 * (t * (t - 2) - 1) + b
}
