const merge = require('webpack-merge')
var path = require('path');
var webpack = require('webpack');
const commonConfig = require('./common.client.config.js');
const ExtractCssChunks = require("extract-css-chunks-webpack-plugin");

var cssChunksPublicPath = '/';


var extractVideoSass = new ExtractCssChunks( {filename : "css/videos/videosBundle.css" , publicPath : cssChunksPublicPath } );
var extractAppSass = new ExtractCssChunks( {filename : "css/app.css" , publicPath : cssChunksPublicPath } );
var extractNewsSass = new ExtractCssChunks( {filename : "css/news/newsBundle.css" , publicPath : cssChunksPublicPath } );

var extractFinanceSass = new ExtractCssChunks({ filename: "css/finance/financeBundle.css", publicPath: cssChunksPublicPath });

const config = merge(commonConfig, {
	output : {
    filename: 'js/[name].bundle.js',
        chunkFilename: 'js/[name].bundle.js',
        publicPath : '/pwa/'
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
						},
						{
              test: /(finance\.sass)/,
							loader: extractFinanceSass.extract({
                use: ['css-loader','sass-loader'],
                fallback : 'style-loader'
              })
            }            
        ]
    },
	plugins : [
		new webpack.DefinePlugin({ //<--key to reduce React's size
            'process.env': {
                'NODE_ENV': JSON.stringify('development'),
                'SERVER' : false
            }
        }),
        extractAppSass,
        extractNewsSass,
				extractVideoSass,
				extractFinanceSass
	]
})

module.exports = config;

