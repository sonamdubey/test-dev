# Checkbox

## Usage
> Important: Make sure to include the [scss file](https://github.com/carwale/oxygen/blob/master/src/Checkbox/style/checkbox.scss) or feel free to create your own.
```js
import { Checkbox } from "oxygen"
// or
import Checkbox from "oxygen/Checkbox"

<Checkbox value="1">
  Hello World!
</Checkbox>
```

### CheckboxGroup
```js
import CheckboxGroup from "oxygen"
// or
import { CheckboxGroup } from "oxygen/Checkbox"

const checkboxOptions = [
  {
    value: 1,
    label: "Apple"
  },
  {
    value: 2,
    label: "Banana"
  },
  {
    value: 3,
    label: "Cherry"
  },
  {
    value: 4,
    label: "Dragon Fruit"
  }
]

<CheckboxGroup
  options={checkboxOptions}
  defaultValue={[1]}
  name="fruits"
/>
```

## API

### Props

#### Checkbox

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| alignIcon | oneOf `left`, `right` | `left` | - | Align `checkbox` icon. |
| checked | `bool` | - | - | If `true`, the checkbox will be checked. |
| children | `node` | - | - | The content of the checkbox. |
| className | `string` | - | - | A custom class for `checkbox` component. |
| defaultChecked | `bool` | - | - | The default value of `checkbox` element. |
| disabled | `bool` | - | - | Disabled state of button. |
| name | `string` | - | - | The `name` attribute of `checkbox` element. |
| prefixClass | `func` | `oxygen-checkbox` | - | The prefix for `checkbox` component. |

### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onChange | `func` | - | - | Callback fired when checkbox value is changed. |

#### CheckboxGroup

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| className | `string` | - | - | A custom class for `CheckboxGroup` component. |
| defaultValue | `array` | - | - | Default selected value. |
| name | `string` | - | - | The `name` propery of all `input`. |
| options | `array` | - | - | Set children. |
| prefixClass | `func` | `oxygen-checkbox-group` | - | The prefix for `checkbox` component. |
| type | oneOf `default`, `pill` | `default` | - | Type of checkbox. |
| value | `array` | - | - | Set currently selected value. |

### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onChange | `func` | - | - | Callback fired when checkbox value is changed. |
