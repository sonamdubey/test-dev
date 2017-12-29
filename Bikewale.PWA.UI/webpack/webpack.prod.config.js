const merge = require('webpack-merge')
var webpack = require('webpack');
const commonConfig = require('./common.config.js');
const ExtractCssChunks = require("extract-css-chunks-webpack-plugin");
var UglifyJsPlugin = require('uglifyjs-webpack-plugin');
var cssChunksPublicPath = '/';

var extractVideoSass = new ExtractCssChunks( {filename : "css/videos/videosBundle.[chunkhash].css" , publicPath : cssChunksPublicPath } );
var extractAppSass = new ExtractCssChunks( {filename : "css/app.[chunkhash].css" , publicPath : cssChunksPublicPath } );
var extractNewsSass = new ExtractCssChunks( {filename : "css/news/newsBundle.[chunkhash].css" , publicPath : cssChunksPublicPath } );

const config = merge(commonConfig, {
	output : {
		filename: 'js/[name].bundle.[chunkhash].js',
        chunkFilename : 'js/[name].bundle.[chunkhash].js',
        publicPath : 'https://stb.aeplcdn.com/bikewale/pwa/'
	},
    module: {
        loaders: [
            {
              test: /(video\.sass)/, // /\.css$/,
              loader : extractVideoSass.extract({
                use : ['css-loader','sass-loader'],
                fallback : 'style-loader'
              })
            },
            {
              test: /(app\.sass)/, // /\.css$/,
              loader : extractAppSass.extract({
                use : ['css-loader','sass-loader'],
                fallback : 'style-loader'
              })
            },
            {
              test: /(news\.sass)/, // /\.css$/,
              loader : extractNewsSass.extract({
                use : ['css-loader','sass-loader'],
                fallback : 'style-loader'
              })
            }
            
        ]
    },
	plugins : [
		new webpack.DefinePlugin({ //<--key to reduce React's size
            'process.env': {
                'NODE_ENV': JSON.stringify('production'),
                'SERVER' : false
            }
        }),
        extractAppSass,
        extractNewsSass,
        extractVideoSass,
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true
        }),
        // new UglifyJsPlugin({
        //     parallel : true,
        //     sourceMap: true,
        //     extractComments : true,
        //     uglifyOptions : {
        //         compress : {
        //             dead_code : true,
        //             drop_console : true,
        //             drop_debugger : true,
        //             reduce_vars: true,
        //             warnings : true
        //         }    
        //     }
        // }),
        new webpack.optimize.AggressiveMergingPlugin(),
	]
})

module.exports = config;

