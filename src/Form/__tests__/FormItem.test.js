import React from 'react';

import FormItem from '../FormItem';
import Input from '../../Input';

describe('<FormItem />', function() {
  describe('render', function() {
    it('should render a form item input', function() {
      const wrapper = shallow(
        <FormItem>
          <Input />
        </FormItem>
      )

      expect(wrapper).toMatchSnapshot();
    })
  })

  describe('props', function() {
    it('should render a form item input', function() {
      const wrapper = mount(
        <FormItem>
          <Input />
        </FormItem>
      )

      expect(wrapper.find('input')).toHaveLength(1);
      expect(wrapper).toMatchSnapshot();
    })

    it('should render a form item with helper text as error', function() {
      const wrapper = shallow(
        <FormItem helperText="Enter a valid name">
          <Input />
        </FormItem>
      )

      //expect(wrapper.find('input')).toHaveLength(1);
      expect(wrapper).toMatchSnapshot();
    })
  })
})
