export const lockScroll = () => {
  let doc = document.documentElement
  let body = document.body

  if (body.clientHeight > window.innerHeight) {
    let scrollTopPosition = window.pageYOffset || doc.scrollTop

    if (scrollTopPosition < 0) {
      scrollTopPosition = 0
    }

    doc.style.overflowY = "hidden"
    doc.style.position = "fixed"
    doc.style.top = -scrollTopPosition + "px"
  }
}

export const unlockScroll = () => {
  let doc = document.documentElement
  let scrollTopPosition = parseInt(doc.style.top)

  doc.style.overflowY = "visible"
  doc.style.position = ""
  window.scrollTo(0, -scrollTopPosition)
}
