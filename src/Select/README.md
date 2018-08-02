# Select

## Usage
> Important: Make sure to include the [scss file](https://github.com/carwale/oxygen/blob/master/src/Select/style/select.scss) or feel free to create your own.
```js
import { Select } from "oxygen"
// or
import Select from "oxygen/lib/Select"

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

<Select
  label="Select fruit"
  options={fruits}
/>
```

## API

### Props

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| containerClassName | `string` | - | - | A custom `class` for select container. |
| disabled | `bool` | - | - |	If `true`, the select dropdown will be disabled. |
| id | `string` | - | - | The `id` of the `select` element. |
| label | `string` | - | - | A custom text for label. |
| labelClassName | `string` | - | - | A custom `class` for label. |
| options | `array` | - | - | Options to select from. |
| optionLabel | `string` | `'label'` | - | A custom property name to access label from `options` prop. |
| optionValue | `string` | `'value'` | - | A custom property name to access value from `options` prop. |
| placeholder | `string` | `'Select option'` | - | The short hint displayed in the select before the user selects a value. |
| prefixClass | `string` | `'oxygen-select'` | - | A prefix for `Select` component classes. |
| required | `bool` | - | - |	If `true`, the select dropdown will be marked as required. |
| selectClassName | `string` | - | - | A custom `class` for select element. |
| value | oneOf `string`, `number` | `-1` | - | The value of the `select` element. |


### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onChange | `func` | - | - | Callback function fired on value change. |
