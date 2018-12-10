const webpack = require('webpack')
const prodConfig = require('./client.prod')
const ExtractTextPlugin = require("extract-text-webpack-plugin")
const merge = require('webpack-merge')
//const BundleAnalyzerPlugin = require("webpack-bundle-analyzer").BundleAnalyzerPlugin
//const ExtractCssChunks = require("extract-css-chunks-webpack-plugin")
//This is done due to https://github.com/webpack/webpack/issues/1577
// DefinePlugin does not override keys and hence it is filtered out before merging
const overridePlugins = ['DefinePlugin', 'CopyWebpackPlugin', 'ExtractTextPlugin']
prodConfig.plugins = prodConfig.plugins.filter(x => x.constructor && overridePlugins.indexOf(x.constructor.name) === -1)

let stagingConfig = merge(prodConfig, {
    devtool: 'source-map',
    output: {
		publicPath: 'https://stc-staging.aeplcdn.com/staticminv2/'
    },

    plugins: [
        new webpack.DefinePlugin({
            'process.env': {
                'NODE_ENV': JSON.stringify('production')
            },
            'COOKIE_DOMAIN': JSON.stringify('staging.carwale.com'),
            '__PROD__': true,
            '__DEV__': false,
            'BHRIGU_HOST_URL': JSON.stringify('/bhrigu/pixel.gif?t=')
        }),
        new ExtractTextPlugin({
            filename: "css/[name].[contenthash:8].css",
            allChunks: true
        }),
        // new ExtractCssChunks({
        //     filename: 'static/css/prices.bundle.css'
        // }),
        // new BundleAnalyzerPlugin({
        //     openAnalyzer: false,
        //     generateStatsFile: true,
        //     statsFilename: "stats.json"
        // })
    ]
})

module.exports = stagingConfig
