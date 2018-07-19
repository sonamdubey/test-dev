# Input

## Usage
> Important: Make sure to include the [scss file](https://github.com/carwale/oxygen/blob/master/src/Input/style/input.scss) or feel free to create your own.
```js
import { Input } from "oxygen"
// or
import Input from "oxygen/Input"

<Input
  label="Name"
  type="text"
  placeholder="Enter your name"
  value="John Doe"
  onChange={this.handleChange}
/>
```

## API

### Props

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| containerClassName | `string` | - | - | A custom class for input container. |
| defaultValue | oneOf `string`, `number` | - | - | The default value of the `Input` element. |
| disabled | `bool` | - | - |	If `true`, the input will be disabled. |
| id | `string` | - | - | The `id` of the `input` element. |
| inputClassName | `string` | - | - | A custom class for input. |
| label | `string` | - | - | A custom text for label. |
| labelClassName | `string` | - | - | A custom class for label. |
| name | `string` | - | - | Name attribute of the `input` element. |
| placeholder | `string` | - | - | The short hint displayed in the input before the user enters a value. |
| prefix | oneOf `string`, `node` | - | - | The prefix text/icon for the `Input`. |
| prefixClass | `string` | 'oxygen-input' | - | A prefix for `Input` component classes. |
| readOnly | `bool` | - | - | Specify an input field as read-only. |
| required | `bool` | - | - | Set field as required. |
| suffix | oneOf `string`, `node` | - | - | The suffix text/icon for the `Input`. |
| type | oneOf `text`, `number`, `tel`, `email`, `password` | - | Define input type. |
| value | oneOf `string`, `number` | `''` | - | The value of the `input` element. |


### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onBlur | `func` | - | - | Callback fired when input is blurred. |
| onChange | `func` | - | - | Callback fired when the value is changed. |
| onFocus | `func` | - | - | Callback fired when input is focused. |
