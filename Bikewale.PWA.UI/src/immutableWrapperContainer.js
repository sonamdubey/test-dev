import React from 'react';
import { Iterable } from 'immutable';
import entries from 'object.entries'

export const toJS = (WrappedComponent) =>
   (wrappedComponentProps) => {
       const KEY = 0;
       const VALUE = 1;
            
       if(Object.entries == null || Object.entries == undefined) {
          Object.entries = entries;

       }

       const propsJS = Object.entries(wrappedComponentProps)
            .reduce((newProps, wrappedComponentProp)  => {
       
                newProps[wrappedComponentProp[KEY]] = 
                    Iterable.isIterable(wrappedComponentProp[VALUE]) 
                        ? wrappedComponentProp[VALUE].toJS() 
                        : wrappedComponentProp[VALUE];
                return newProps;
            }, {});

       return <WrappedComponent {...propsJS} />
   };
