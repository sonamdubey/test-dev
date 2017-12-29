var webpack = require('webpack');
var path = require('path');

var BUILD_DIR = path.resolve(__dirname,'..', 'build');
var APP_DIR = path.resolve(__dirname, '..','src');


var config = {
    entry: {
        server : APP_DIR + '/server.js',
    },
    output: {
        path: BUILD_DIR+'/server/',
        filename: '[name].bundle.js'
    },
    module: {
        loaders: [
            {
                test: /\.jsx?/,
                include: APP_DIR,
                loader: 'babel-loader',
                query : {
                  presets : ["es2015","react"]
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
};

module.exports = config;
