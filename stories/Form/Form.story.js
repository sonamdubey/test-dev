import React from 'react';
import { storiesOf } from '@storybook/react';
import { withInfo } from '@storybook/addon-info';

import Form from '../../src/Form';
import '../../src/Form/style/form-item.scss';
import Input from '../../src/Input';
import '../../src/Input/style/input.scss';
import Button from '../../src/Button';
import '../../src/Button/style/button.scss';

import FormWithValidation from './FormWithValidation';

storiesOf('Form', module)
  .add('default',
    withInfo()(() => (
      <Form>
        <Form.Item>
          <Input
            id="name"
            label="Name"
            placeholder="Enter your name"
            value="John Doe"
          />
        </Form.Item>
        <Form.Item>
          <Input
            id="mobile"
            label="Mobile"
            type="tel"
            placeholder="Enter your mobile"
            value="9876543210"
            prefix="+91"
            maxLength={10}
          />
        </Form.Item>
        <Form.Item>
          <Button type="primary">Submit</Button>
        </Form.Item>
      </Form>
    ))
  )
  .add('with validation', () => (
    <FormWithValidation />
  ))
