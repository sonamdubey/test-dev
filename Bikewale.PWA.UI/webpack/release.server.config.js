const merge = require('webpack-merge')
var path = require('path');
var webpack = require('webpack');
const commonConfig = require('./common.server.config.js');


const config = merge(commonConfig, {
	plugins: [
        new webpack.DefinePlugin({ //<--key to reduce React's size
            'process.env': {
                'NODE_ENV': JSON.stringify('production'), 
                'SERVER' : true
            }
        }),
        new webpack.optimize.UglifyJsPlugin(),
        new webpack.optimize.AggressiveMergingPlugin()
    ]
})

module.exports = config;

