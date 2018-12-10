export const jsScreenClass = {
  container: 'js-screen-container',
  header: 'js-screen-head',
  body: 'js-screen-body',
  footer: 'js-screen-footer'
}

export function setScreenBodyHeight(container) {
  let availableHeight = window.innerHeight;
  let header = container.querySelector(`.${jsScreenClass.header}`);
  let body = container.querySelector(`.${jsScreenClass.body}`);
  let footer = container.querySelector(`.${jsScreenClass.footer}`);

  if (header) {
    availableHeight = availableHeight - header.offsetHeight;
  }

  if (footer) {
    availableHeight = availableHeight - footer.offsetHeight
  }

  if (body) {
    body.style.maxHeight = `${availableHeight}px`;
  }
}
