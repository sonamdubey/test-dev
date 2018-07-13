import React from 'react';
import { storiesOf } from '@storybook/react';
import { withInfo } from '@storybook/addon-info';
import { withKnobs, select, text } from '@storybook/addon-knobs';

import Input from '../../src/Input';
import '../../src/Input/style/input.scss';

/* knobs */
// prop: type
const typeLabel = 'type';
const typeOptions = {
  text: 'text',
  tel: 'tel',
  number: 'number',
  email: 'email',
  password: 'password'
};
const typeDefaultValue = 'text';


storiesOf('Input', module)
  .addDecorator(withKnobs) // TODO: make this a global decoration and move to config file [https://storybook.js.org/addons/introduction/]
  .add('default',
    withInfo()(() => (
      <Input
        id="name"
        label={text('label', 'Name')}
        type={select(typeLabel, typeOptions, typeDefaultValue)}
        placeholder={text('placeholder', 'Enter your name')}
      />
    ))
  )
  .add('with affix',
    withInfo()(() => (
      <Input
        id="mobileNumber"
        label={text('label', 'Mobile Number')}
        type="tel"
        placeholder={text('placeholder', 'Enter your mobile number')}
        value={text('value', '9876543210')}
        maxLength="10"
        prefix="+91"
      />
    ))
  )
