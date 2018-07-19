import { configure } from '@storybook/react';
import { setOptions } from '@storybook/addon-options';
import { setDefaults } from '@storybook/addon-info';
import { stylesheet } from './theme';

import '../style/storybook.scss';

// addon-options
setOptions({
  name: 'OXYGEN',
  url: '#',
  addonPanelInRight: true
});

// addon-info
setDefaults({
  inline: true,
  styles: stylesheet
})

const req = require.context('../stories', true, /\.story\.js$/)

function loadStories() {
  require('../stories/Introduction')
  req.keys().forEach((filename) => req(filename))
}

configure(loadStories, module);
