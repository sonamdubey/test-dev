import React from 'react';
import Input from '../index';

const inputPrefixClassName = 'oxygen-input';

describe('<Input />', function() {
  describe('render', function() {
    it('should render an input', function() {
      const wrapper = mount(
        <Input />
      );

      expect(wrapper).toMatchSnapshot();
    });
  });

  describe('component props', function() {
    it('should render an input with label', function() {
      const labelText = 'Name';
      const wrapper = mount(
        <Input
          label={labelText}
        />
      );

      const label = wrapper.find('label');
      expect(label).toHaveLength(1);
      expect(label.text()).toEqual(labelText);
      expect(wrapper).toMatchSnapshot();
    })

    it('should render an input with custom classes', function() {
      const labelText = 'Name';
      const containerClassName = 'input-container';
      const labelClassName = 'input-container-label';
      const inputClassName = 'input-container-field';

      const wrapper = mount(
        <Input
          label={labelText}
          containerClassName={containerClassName}
          labelClassName={labelClassName}
          inputClassName={inputClassName}
        />
      );

      expect(wrapper.find(`.${inputPrefixClassName}`).hasClass(containerClassName)).toEqual(true);
      expect(wrapper.find(`.${inputPrefixClassName}-label`).hasClass(labelClassName)).toEqual(true);
      expect(wrapper.find(`.${inputPrefixClassName}-field`).hasClass(inputClassName)).toEqual(true);
    })

    it('should render an input with prefix', function() {
      const mobilePrefix = '+91';
      const wrapper = mount(
        <Input
          type="tel"
          placeholder="Enter mobile number"
          prefix={mobilePrefix}
        />
      )

      const affixContent = wrapper.find(`.${inputPrefixClassName}-affix-content`);
      expect(affixContent).toHaveLength(1);
      const inputPrefix = wrapper.find(`.${inputPrefixClassName}-prefix`);
      expect(inputPrefix).toHaveLength(1);
      expect(inputPrefix.text()).toEqual(mobilePrefix);
      expect(wrapper).toMatchSnapshot();
    })

    it('should render an input with suffix', function() {
      const kilometerSuffix = 'kms';
      const wrapper = mount(
        <Input
          type="tel"
          placeholder="Enter kilometer"
          suffix={kilometerSuffix}
        />
      )

      const affixContent = wrapper.find(`.${inputPrefixClassName}-affix-content`);
      expect(affixContent).toHaveLength(1);
      const inputSuffix = wrapper.find(`.${inputPrefixClassName}-suffix`);
      expect(inputSuffix).toHaveLength(1);
      expect(inputSuffix.text()).toEqual(kilometerSuffix);
      expect(wrapper).toMatchSnapshot();
    })
  });
})
