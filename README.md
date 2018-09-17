<h1 align="center">
  <img width="300" src="assets/icons/oxygen.svg" alt="Oxygen">
</h1>

> A collection of React components

### Install

```
npm intall oxygen --save
```

### Components

* [Button](https://github.com/carwale/oxygen/tree/master/src/Button)
* [Checkbox](https://github.com/carwale/oxygen/tree/master/src/Checkbox)
* [Form](https://github.com/carwale/oxygen/tree/master/src/Form)
* [Input](https://github.com/carwale/oxygen/tree/master/src/Input)
* [Select](https://github.com/carwale/oxygen/tree/master/src/Select)

### Usage

`oxygen` is published on private npm proxy registry.
[Link](http://172.16.0.27:4873)

To install this package, please follow these steps:

Step 1. Set your npm registry to private registry
```
npm set registry http://172.16.0.27:4873
```

Step 2. Create an user and log in
```
npm adduser --registry http://172.16.0.27:4873
```
> Note: Above command will ask for username and password; enter your PC's credentials

Step 3. Install `oxygen`
```
npm install oxygen --save
```

### Playground

#### Live

[Link](http://172.16.0.27:9080/)

#### Local

To run demo on your own computer:
* Clone this repository
* `npm install`
* `npm run storybook`
* Visit http://localhost:8001/
