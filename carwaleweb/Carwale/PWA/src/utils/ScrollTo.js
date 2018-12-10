/*
 * ScrollTo animation using pure javascript
 * https://gist.github.com/andjosh/6764939
 */

export const scrollTop = (element, to, duration = 500) => {
	let start = element.scrollTop,
		change = to - start,
		currentTime = 0,
		increment = 20

	let animateScroll = function () {
		currentTime += increment
		let val = Math.easeInOutQuad(currentTime, start, change, duration)
		element.scrollTop = val

		if (currentTime < duration) {
			setTimeout(animateScroll, increment)
		}
	}

	animateScroll()
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
	let elementRect = event.currentTarget.getBoundingClientRect()

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
