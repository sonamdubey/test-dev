import React from 'react';
import { storiesOf } from '@storybook/react';
import { withInfo } from '@storybook/addon-info';

import Select from '../../src/Select';
import '../../src/Select/style/select.scss';

const fruits = [
  {
    label: "Apple",
    value: 1
  },
  {
    label: "Banana",
    value: 2
  },
  {
    label: "Cherry",
    value: 3
  },
  {
    label: "Dragon Fruit",
    value: 4
  }
]

storiesOf('Select', module)
  .add('default',
    withInfo()(() => (
      <Select
        label="Select fruit"
        options={fruits}
      />
    ))
  )
