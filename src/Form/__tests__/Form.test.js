import React from 'react';
import Form from '../index';

describe('<Form />', function() {
  describe('render', function() {
    it('should render a form', function() {
      const wrapper = shallow(
        <Form />
      )

      expect(wrapper).toMatchSnapshot();
    })
  })
})
