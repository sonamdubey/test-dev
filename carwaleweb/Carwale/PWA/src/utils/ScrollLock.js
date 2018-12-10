export const lockScroll = () => {
	let doc = document.documentElement
	let body = document.body

	let scrollTopPosition = window.pageYOffset || doc.scrollTop

	if (scrollTopPosition < 0) {
		scrollTopPosition = 0
	}

	doc.style.overflowY = "hidden"
	doc.style.width = "100%" // make document 100% for fixed position
	doc.style.position = "fixed"
	doc.style.top = -scrollTopPosition + "px"
}

export const unlockScroll = () => {
	let doc = document.documentElement
	let scrollTopPosition = parseInt(doc.style.top)

	doc.style.overflowY = ""
	doc.style.position = ""
	window.scrollTo(0, -scrollTopPosition)
	doc.style.width = ""
}
