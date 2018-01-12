var webpack = require('webpack');
var path = require('path');
var CompressionPlugin = require("compression-webpack-plugin");
// var ExtractTextPlugin = require("extract-text-webpack-plugin");
var OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');
var CleanWebpackPlugin = require('clean-webpack-plugin');
var BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;


var AssetsPlugin = require('assets-webpack-plugin');
const ExtractCssChunks = require("extract-css-chunks-webpack-plugin");

var BUILD_DIR = path.resolve(__dirname, '../../Bikewale.UI/', 'pwa');
var APP_DIR = path.resolve(__dirname, '..', 'src');
var ROOT_DIR = path.resolve(__dirname, '..');
var vendorLibraries = ['react' , 'react-dom', 'redux' , 'react-router-dom' , 'react-redux' , 'redux-thunk' , 'immutable' , 'redux-immutable' , 'react-helmet', 'prop-types','react-lazy-load','es-abstract']


var config ={
	  entry: {
        app : APP_DIR + '/client.js',
        vendor : vendorLibraries
    },
    output : {
    	path : BUILD_DIR
    },
    module: {
        loaders: [
            {
                test : /\.jsx?/,
                include : APP_DIR,
                loader : 'babel-loader',
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
    plugins: [
        new webpack.optimize.CommonsChunkPlugin({
            name : 'vendor',
            minChunks : function(module) {
              return module.context && module.context.indexOf('node_modules') !== -1
            }
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name : 'manifest'
        }),
        new AssetsPlugin({
            prettyPrint: true,
            path: BUILD_DIR
        }),
        new OptimizeCssAssetsPlugin({
          assetNameRegExp: /(.*\.css)\s/g,
          cssProcessor: require('cssnano'),
          cssProcessorOptions: { discardComments: {removeAll: true } },
          canPrint: true
        }),
        new CleanWebpackPlugin(['js','css'], { root: BUILD_DIR })
        /*,
    
		
        new BundleAnalyzerPlugin({
          // Can be `server`, `static` or `disabled`. 
          // In `server` mode analyzer will start HTTP server to show bundle report. 
          // In `static` mode single HTML file with bundle report will be generated. 
          // In `disabled` mode you can use this plugin to just generate Webpack Stats JSON file by setting `generateStatsFile` to `true`. 
          analyzerMode: 'server',
          // Host that will be used in `server` mode to start HTTP server. 
          analyzerHost: '127.0.0.1',
          // Port that will be used in `server` mode to start HTTP server. 
          analyzerPort: 8888,
          // Path to bundle report file that will be generated in `static` mode. 
          // Relative to bundles output directory. 
          reportFilename: 'report.html',
          // Module sizes to show in report by default. 
          // Should be one of `stat`, `parsed` or `gzip`. 
          // See "Definitions" section for more information. 
          defaultSizes: 'parsed',
          // Automatically open report in default browser 
          openAnalyzer: true,
          // If `true`, Webpack Stats JSON file will be generated in bundles output directory 
          generateStatsFile: true,
          // Name of Webpack Stats JSON file that will be generated if `generateStatsFile` is `true`. 
          // Relative to bundles output directory. 
          statsFilename: 'stats.json',
          // Options for `stats.toJson()` method. 
          // For example you can exclude sources of your modules from stats file with `source: false` option. 
          // See more options here: https://github.com/webpack/webpack/blob/webpack-1/lib/Stats.js#L21 
          statsOptions: null,
          // Log level. Can be 'info', 'warn', 'error' or 'silent'. 
          logLevel: 'info'
        })*/
    ]
}

module.exports = config;