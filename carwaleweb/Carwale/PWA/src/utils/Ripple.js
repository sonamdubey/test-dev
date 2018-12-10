/*
 * Adapted from
 * 1. 'Recreating the ripple click effect from Google material design'
 * http://thecodeplayer.com/walkthrough/ripple-click-effect-google-material-design
 * 2. 'Material Design Ripple Effect with JavaScript - Ayo Isaiah'
 * https://codepen.io/ayoisaiah/pen/GWwabJ
 *
 */

const defaultBackgroundColor = "rgba(0, 0, 0, .3)"

export const createRipple = (event, customBackgroundColor) => {
	const targetElement = event.currentTarget

	// create .ink element if it doesn't exist
	if(!targetElement.querySelectorAll(".ink").length) {
		const span = document.createElement("span")

		span.classList.add("ink")
		targetElement.appendChild(span)
	}

	const ink = targetElement.querySelectorAll(".ink")[0]

	// incase of quick double clicks stop the previous animation
	ink.classList.remove("animate")

	// set size of .ink
	if (!ink.offsetHeight && !ink.offsetWidth) {
		// use parent's width or height whichever is larger for the diameter to make a circle which can cover the entire element.
		const dimension = Math.max(targetElement.offsetHeight, targetElement.offsetWidth)

		ink.style.width = dimension + "px"
		ink.style.height = dimension + "px"
	}

	const rect = targetElement.getBoundingClientRect()

	const offset = {
		top: rect.top + (window.pageYOffset || document.documentElement.scrollTop),
		left: rect.left + (window.pageXOffset || document.documentElement.scrollLeft)
	}

	// get click coordinates
	// logic = click coordinates relative to page - parent's position relative to page - half of self height/width to make it controllable from the center;
	const clickPositionX = event.pageX - offset.left - ink.offsetWidth / 2
	const clickPositionY = event.pageY - offset.top - ink.offsetHeight / 2

	ink.style.top = clickPositionY + "px"
	ink.style.left = clickPositionX + "px"
	if (customBackgroundColor) {
		ink.style.backgroundColor = customBackgroundColor
	}
	else {
		ink.style.backgroundColor = defaultBackgroundColor
	}
	ink.classList.add("animate")

	// remove .ink element on animation end
	const detachRipple = () => {
		targetElement.removeEventListener("animationend", detachRipple)
		targetElement && targetElement.contains(ink) && targetElement.removeChild(ink)
	}

	targetElement.addEventListener("animationend", detachRipple)
}
