import closest from './closestPolyfill'

function focusCollapsible(event) {
  const eventRect = event.currentTarget.getBoundingClientRect();
  const windowScrollTop = window.pageYOffset || document.documentElement.scrollTop;
  const collapsibleTriggerHeight = event.currentTarget.closest('.collapsible').querySelector('.collapsible__trigger').offsetHeight;

  let floatingContentHeight = 0;
  const floatingContent = document.querySelector('.tabs--fixed .tabs__content')
  if (typeof floatingContent !== 'undefined') {
    floatingContentHeight = floatingContent.offsetHeight;
  }

  const positionY = windowScrollTop + eventRect.top - (collapsibleTriggerHeight + floatingContentHeight);

  window.scrollTo(0, positionY)
}

module.exports = {
  focusCollapsible: focusCollapsible
}
