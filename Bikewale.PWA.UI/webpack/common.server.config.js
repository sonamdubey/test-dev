var webpack = require('webpack');
var path = require('path');
var CleanWebpackPlugin = require('clean-webpack-plugin');

var BUILD_DIR = path.resolve(__dirname, '../../Bikewale.UI/UI/', 'pwa');
var APP_DIR = path.resolve(__dirname, '..','src');


var config = {
    entry: {
        server : APP_DIR + '/server.js',
    },
    output: {
        path : BUILD_DIR,
        filename: 'server/[name].bundle.js'
    },
    module: {
        loaders: [
            {
                test: /\.jsx?/,
                include: APP_DIR,
                loader: 'babel-loader',
                query : {
                  presets : ["es2015","react","stage-2"]
                }
            }
        ]
    },
    resolve : {
        extensions: ['.js','.jsx'],
        alias : {
          "react" : "preact-compat",
          "react-dom" : "preact-compat"
        }
    },
    performance: {
       maxAssetSize: 100000,
       maxEntrypointSize: 300000,
       hints: 'warning'
    },
    plugins:[
        new CleanWebpackPlugin(['server'], { root: BUILD_DIR })
    ]
};

module.exports = config;
