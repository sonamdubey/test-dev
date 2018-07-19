import React from 'react';
import { storiesOf } from '@storybook/react';
import { withInfo } from '@storybook/addon-info';

import Checkbox from '../../src/Checkbox';
import '../../src/Checkbox/style/checkbox.scss';

import CheckboxGroupExample from './CheckboxGroup';

storiesOf('Checkbox', module)
  .add('default',
    withInfo()(() => (
      <Checkbox value="1">
        Hello World!
      </Checkbox>
    ))
  )
  .add('with Group', () => (
    <CheckboxGroupExample />
  ))
