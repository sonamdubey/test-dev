import React from 'react';
import { Iterable } from 'immutable';

export const toJS = (WrappedComponent) =>
   (wrappedComponentProps) => {
            
       const propsJS = Object.keys(wrappedComponentProps)
           .reduce((newProps, key) => {

               newProps[key] =
                   Iterable.isIterable(wrappedComponentProps[key])
                   ? wrappedComponentProps[key].toJS()
                   : wrappedComponentProps[key];
               return newProps;
           }, {});

       return <WrappedComponent {...propsJS} />
   };
