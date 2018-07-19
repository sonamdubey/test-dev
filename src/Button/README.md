# Button

## Usage
> Important: Make sure to include the [scss file](https://github.com/carwale/oxygen/blob/master/src/Button/style/button.scss) or feel free to create your own.
```js
import { Button } from "oxygen"
// or
import Button from "oxygen/Button"

<Button type="primary">
  Hello World!
</Button>
```

## API

### Props

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| block | `bool` | - | - | Make button occupy width of its container. |
| children | `node` | - | - | The content of the button. |
| className | `string` | - | - | A custom class for `button` component. |
| disabled | `bool` | - | - | Disabled state of button. |
| ghost | `bool` | - | - | Invert button. |
| href | `string` | - | - | Redirect url of link button. |
| htmlType | oneOf `button`, `reset`, `submit` | `button` | - | Defines HTML button `type` attribute. |
| prefixClass | `string` | `oxygen-btn` | - | The prefix for component classes. |
| size | oneOf `small`, `default`, `large` | `default` | - | The size of the button. |
| target | `string` | - | - | Target attribute for anchor. |
| type | oneOf `default`, `primary`, `secondary` | `default` | - | - | Button style variant. |


### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onClick | `func` | - | - | Callback fired when the button is clicked. |
