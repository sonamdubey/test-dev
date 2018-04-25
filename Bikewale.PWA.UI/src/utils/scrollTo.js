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

export const inView = (element, elementContainer) => {
  let children = element.childNodes;
  let isViewSelected = false;
  children.forEach(function(item) {

    if(isElementVisible(item)) {
      if(isViewSelected != true) {
        item.classList.add("inview--active");
        isViewSelected = true
      }
      else {
        item.classList.remove("inview--active");
      }
    }
    else {
      item.classList.remove("inview--active");
    }

    
    // if(isViewSelected != true) {
    //   if(isElementVisible(item)) {
    //     item.classList.add("inview--active");
    //     isViewSelected = true
    //   }
    // }
  });
    
}

export const isElementVisible = (el) => {
  var rect     = el.getBoundingClientRect(),
  vWidth   = window.innerWidth || doc.documentElement.clientWidth,
  vHeight  = window.innerHeight || doc.documentElement.clientHeight,
  efp      = function (x, y) { return document.elementFromPoint(x, y) };     

// Return false if it's not in the viewport
if (rect.right < 0 || rect.bottom < 0 
      || rect.left > vWidth || rect.top > vHeight)
  return false;

// Return true if any of its four corners are visible
return (
    el.contains(efp(rect.left,  rect.top))
||  el.contains(efp(rect.right, rect.top))
||  el.contains(efp(rect.right, rect.bottom))
||  el.contains(efp(rect.left,  rect.bottom))
);
  
}
export const scrollLeft = (element, to, duration = 500) => {
	let start = element.scrollLeft,
		change = to - start,
		currentTime = 0,
		increment = 20

	let animateScroll = function () {
		currentTime += increment
		let val = Math.easeInOutQuad(currentTime, start, change, duration)
		element.scrollLeft = val

		if (currentTime < duration) {
			setTimeout(animateScroll, increment)
		}
	}

	animateScroll()
}

export const scrollIntoView = (element, event) => {
  let elementRect = null
  event.currentTarget 
    ? 
      elementRect=event.currentTarget.getBoundingClientRect() 
    : 
      elementRect=event.getBoundingClientRect()
	if(elementRect.left < 0 || elementRect.right > window.innerWidth) {
		let leftPosition = element.scrollLeft + elementRect.left - 20

		scrollLeft(element, leftPosition)
	}
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
