import React from 'react';
import { storiesOf } from '@storybook/react';
import { withInfo } from '@storybook/addon-info';
import { withKnobs, select, boolean } from '@storybook/addon-knobs';

import Button from '../../src/Button';
import '../../src/Button/style/button.scss';

/* knobs */
// props: type
const typeLabel = 'type';
const typeOptions = {
  primary: 'primary',
  secondary: 'secondary',
  default: 'default',
}
const typeDefaultValue = 'primary';

// props: size
const sizeLabel = 'size';
const sizeOptions = {
  small: 'small',
  large: 'large',
  default: 'default',
}
const sizeDefaultValue = 'default';

function handleButtonClick(event) {
  console.log(event.target);
}

storiesOf('Button', module)
  .addDecorator(withKnobs)
  .add('default',
    withInfo()(() => (
      <Button
        type={select(typeLabel, typeOptions, typeDefaultValue)}
        size={select(sizeLabel, sizeOptions, sizeDefaultValue)}
        ghost={boolean('ghost', false)}
        block={boolean('block', false)}
        disabled={boolean('disabled', false)}
        onClick={handleButtonClick}
      >
        Hello World!
      </Button>
    ))
  )
