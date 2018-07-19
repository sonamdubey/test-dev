import React from 'react';
import { storiesOf } from '@storybook/react';
import { doc } from 'storybook-readme';

import IntroductionReadMe from '../README.md'

storiesOf('Introduction', module)
  .add('Getting started', doc(IntroductionReadMe))
