/*
 * ScrollTo animation using pure javascript
 * https://gist.github.com/andjosh/6764939
 * 
 * debounce
 * https://davidwalsh.name/javascript-debounce-function
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

export const inView = (element) => {
  let isViewSelected = false;
  [...element.children].forEach(function(item) {

    if(isElementVisible(item.firstChild, 'horizontal', 50)) {
      if(isViewSelected != true) {
        [...item.parentElement.children].forEach(function(child){
          child.classList.remove('inview--active');
        })

        item.classList.add("inview--active");
        isViewSelected = true
      }
    }

  });
    
}

/*
  direction: horizontal, vertical
  threshold: visiblity of card on viewport eg, 50, 75
*/
export const isElementVisible = (element,  direction, threshold) => {
  var elementRect= element.getBoundingClientRect();
  var thresholdValue;
  if(typeof threshold==='number'){
    thresholdValue = threshold ? elementRect.width*(parseInt(threshold)/100) : 0;
  }
  else {
    console.log('please provide threshold value as int in isElementVisible')
  }
  
  switch(direction) {
    case 'horizontal':
      if(elementRect.left + thresholdValue > 0 && elementRect.right - thresholdValue < window.innerWidth) {
        return true;
      }
      else {
        return false;
      }
    
    case 'vertical':
      if(elementRect.top + thresholdValue > 0 && elementRect.bottom - thresholdValue < window.innerHeight) {
        return true;
      }
      else {
        return false;
      }
    
    default:
      if(elementRect.left + thresholdValue > 0 && elementRect.right - thresholdValue < window.innerWidth && elementRect.top + thresholdValue > 0 && elementRect.bottom - thresholdValue < window.innerHeight) {
        return true;
      }
      else {
        return false;
      }    
  }  
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
  if (event == null) {
    return null;
  }
  let elementRect = null;
  if (event.currentTarget != null) {
    elementRect = event.currentTarget.getBoundingClientRect();
  }
  else {
    elementRect = event.getBoundingClientRect();
  }

  if (elementRect.left < 0 || elementRect.right > window.innerWidth) {
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
