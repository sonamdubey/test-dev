import closest from './closestPolyfill';
import { scrollTop } from './scrollTo';

function centerTabLink(targetLink) {
  let tabList = targetLink.closest('.tabs__list');
  let listWidth = tabList.offsetWidth;
  let targetLinkWidth = targetLink.offsetWidth;
  let targetLinkIndex = Array.prototype.indexOf.call(tabList.children, targetLink);

  let tabLinks = [...tabList.querySelectorAll('.tabs-list__item')];
  let offsetLeft = 0;

  // Add up the width of all the elements before target link
  for(var i = 0; i < targetLinkIndex; i++) {
    offsetLeft += tabLinks[i].offsetWidth;
  }

  let scrollXPosition = Math.max(0, offsetLeft - (listWidth - targetLinkWidth) / 2);
  tabList.closest('.tabs__content').scrollLeft = scrollXPosition
}

function handleTabLinkClick(targetLink) {
  let tabName = targetLink.getAttribute('data-tab');
  centerTabLink(targetLink);

  let tabsContainer = targetLink.closest('.tabs__container');
  let tabListHeight = tabsContainer.querySelector('.tabs__content').offsetHeight;

  let targetPanel = tabsContainer.querySelector('.tabs-panel__item[data-tab-panel="' + tabName + '"]');
  let panelOffsetTop = targetPanel.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - tabListHeight;
  let scrollPosition = Math.ceil(panelOffsetTop);

  scrollTop(window, scrollPosition);
}

function handleStickyTabs(tabsContainer) {
  const tabsRect = tabsContainer.getBoundingClientRect();
  const tabListHeight = tabsContainer.querySelector('.tabs__content').offsetHeight;

  if (tabsRect.top < 0 && (tabsRect.height + tabsRect.top - (tabListHeight * 2)) > 0) {
    tabsContainer.classList.add('tabs--fixed');
  }
  else {
    tabsContainer.classList.remove('tabs--fixed');
  } 
}

function handleTabLinkHighlight(tabsContainer) {
  const tabPanels = [...tabsContainer.querySelectorAll('.tabs-panel__item')];

  tabPanels.forEach(function (panel) {
    let windowScrollPosition = window.pageYOffset || document.documentElement.scrollTop;

    const tabListHeight = tabsContainer.querySelector('.tabs__content').offsetHeight;
    let panelTopPosition = (panel.getBoundingClientRect().top + windowScrollPosition) - tabListHeight;
    let panelBottomPosition = panelTopPosition + panel.offsetHeight;

    if (windowScrollPosition >= panelTopPosition && windowScrollPosition <= panelBottomPosition) {
      let prevActiveTabLink = tabsContainer.querySelector('.tab--active');
      if (prevActiveTabLink) {
        prevActiveTabLink.classList.remove('tab--active');
      }

      let panelName = panel.getAttribute('data-tab-panel');
      let nextActiveTabLink = tabsContainer.querySelector('.tabs-list__item[data-tab="' + panelName + '"]');
      nextActiveTabLink.classList.add('tab--active');
      centerTabLink(nextActiveTabLink);
    }
  })
}

function setDefaultTabLink(tabsContainer) {
  if (!tabsContainer.querySelector('.tab--active')) {
    tabsContainer.querySelector('.tabs-list__item').classList.add('tab--active');
  }
}

function scrollEvents(tabsContainer) {
  handleStickyTabs(tabsContainer);
  handleTabLinkHighlight(tabsContainer);
}

function addTabEvents(tabsContainer) {
  // Attach tab links click event
  let tabLinks = [...tabsContainer.querySelectorAll('.tabs-list__item')];

  tabLinks.forEach(function (tab) {
    tab.addEventListener('click', function () {
      handleTabLinkClick(this);
    });
  });

  setDefaultTabLink(tabsContainer);

  // Attach sticky tabs and tab link highlight event
  window.addEventListener('scroll', scrollEvents.bind(this, tabsContainer));
}

function removeTabEvents(tabsContainer) {
  window.removeEventListener('scroll', scrollEvents.bind(this, tabsContainer));
}

module.exports = {
  addTabEvents,
  removeTabEvents
}
