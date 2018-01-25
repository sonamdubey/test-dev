const merge = require('webpack-merge')
var path = require('path');
var webpack = require('webpack');
const commonConfig = require('./common.server.config.js');


const config = merge(commonConfig, {
	plugins: [
        new webpack.DefinePlugin({ //<--key to reduce React's size
            'process.env': {
                'NODE_ENV': JSON.stringify('development'), 
                'SERVER' : true
            }
        })
    ]
})

module.exports = config;

