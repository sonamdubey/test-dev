# Form

## Usage
> Important: Make sure to include the [scss file](https://github.com/carwale/oxygen/blob/master/src/Form/style/form-item.scss) or feel free to create your own.
```js
import { Form, Input } from "oxygen"
// or
import Form from "oxygen/lib/Form"
import Input from "oxygen/lib/Input"
const FormItem = Form.Item

<Form>
  <FormItem
    validateStatus={this.state.usernameValidateStatus}
    helperText={this.state.usernamehelperText}
  >
    <Input
      id="name"
      label="Name"
      placeholder="Enter your name"
      value={this.state.username}
      onChange={this.handleUserNameChange}
    />
  </FormItem>
  <FormItem
    validateStatus={this.state.mobileValidateStatus}
    helperText={this.state.mobilehelperText}
  >
    <Input
      id="mobile"
      label="Mobile"
      type="tel"
      placeholder="Enter your mobile"
      value={this.state.mobile}
      onChange={this.handleMobileChange}
      prefix="+91"
      maxLength={10}
    />
  </FormItem>
  <FormItem>
    <button
      type="button"
      onClick={this.handleSubmit}
    >
      Submit
    </button>
  </FormItem>
</Form>
```

## API

### Form

#### Props

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| className | `string` | - | - | A custom class for `form` element. |
| prefixClass | `string` | 'oxygen-form' | - | A prefix for `Form` component classes. |

#### Events

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| onSubmit | `func` | - | - | Callback fired when form is submitted. |

### FormItem

#### Props

| propName | propType | defaultValue | isRequired | description |
| -------- | -------- | ------------ | ---------- | ----------- |
| helperText | `string` | - | - | The helper message. |
| prefixClass | `string` | 'oxygen-form-item' | - | A prefix for `FormItem` component classes. |
| validateStatus | oneOf `error`, `''` | - | - | The validation status. |
