const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const { InjectManifest } = require("workbox-webpack-plugin");
const UglifyJSPlugin = require("uglifyjs-webpack-plugin");
const CompressionPlugin = require("compression-webpack-plugin");
const BrotliPlugin = require("brotli-webpack-plugin");
const HtmlWebPackPlugin = require("html-webpack-plugin");
const {
  clientCommon,
  PATHS,
  CHUNKS,
  MANIFEST_EXCLUDE_CHUNKS,
  workBoxLibMap
} = require("./common.config");
const merge = require("webpack-merge");

let prodConfig = merge(clientCommon, {
  devtool: 'source-map',
  output: {
    filename: "js/[name].[chunkhash:9].js",
    chunkFilename: "js/[name].[chunkhash:9].js",
    publicPath: "https://stc.aeplcdn.com/staticminv2/"
  },
  plugins: [
    new webpack.DefinePlugin({
      "process.env": {
        NODE_ENV: JSON.stringify("production")
      },
      COOKIE_DOMAIN: JSON.stringify("carwale.com"),
      __PROD__: true,
      __DEV__: false,
      BHRIGU_HOST_URL: JSON.stringify("/bhrigu/pixel.gif?t=")
    }),

    new webpack.HashedModuleIdsPlugin(),

    new webpack.optimize.CommonsChunkPlugin({
      name: "vendor",
      minChunks: module => {
        // this assumes your vendor imports exist in the node_modules directory

        return module.context && module.context.indexOf("node_modules") !== -1;
      }
    }),

    new webpack.optimize.CommonsChunkPlugin({
      name: "bootstrap",
      minChunks: Infinity
    }),

    // new webpack.optimize.CommonsChunkPlugin({
    //     name: "emiappbootstrap",
    //     minChunks: Infinity,
    //     chunks:['emiapp']
    // }),

    new ExtractTextPlugin({
      filename: "css/[name].[contenthash:9].css"
    }),

    new UglifyJSPlugin({
      parallel: true,
      sourceMap: true,
      extractComments: true,
      uglifyOptions: {
        ecma: 6,
        compress: {
          dead_code: true,
          drop_debugger: true,
          hoist_funs: true,
          inline: true,
          join_vars: true,
          reduce_vars: true,
          warnings: true,
          drop_console: true,
          keep_infinity: true
        }
      }
    }),

    // new CompressionPlugin({
    //     test: /\.(html|css)$|^(?!sw).*\.js$/,
    //     threshold: 10240,
    //     minRatio: 0.8,
    //     algorithm: "gzip"
    // }),

    // new BrotliPlugin({
    //     test: /\.(css|html)$|^(?!sw).*\.js$/,
    //     threshold: 10240,
    //     minRatio: 0.8
    // }),

    new HtmlWebPackPlugin({
      filename: "find-car/index.html",
      chunks: [CHUNKS.NCF, "vendor", "bootstrap"],
      template: "index.html",
      title: "New Car Finder - CarWale.com"
    }),

    new HtmlWebPackPlugin({
      filename: "used/valuation/index.html",
      chunks: [CHUNKS.VALUATION, "vendor", "bootstrap"],
      template: "index.html",
      title: "Check value of your car for FREE - CarWale",
      keywords:
        "car valuations india, instant valuation, value of your car, my car value, car assessment, used car actual price, used car price",
      description: "Check value of your car for absolutely free on CarWale.com"
    }),
    new HtmlWebPackPlugin({
      filename: "emiapp/index.html",
      template: "emiapptemplate.html",
      inject: false,
      chunks: [CHUNKS.EMI, "vendor", "bootstrap"]
    }),

    new HtmlWebPackPlugin({
      filename: "location/index.html",
      template: "template.html",
      inject: false,
      chunks: [CHUNKS.LOCATION, 'vendor', 'bootstrap'],
    }),
    new HtmlWebPackPlugin({
      filename: 'filterplugin/index.html',
      template: 'filterPluginTemplate.html',
      inject: false,
      chunks: [CHUNKS.FILTERPLUGIN, 'vendor', 'bootstrap']
    }),
    new HtmlWebPackPlugin({
      filename: "lead-form/index.html",
      template: "leadFormTemplate.html",
      inject: false,
      chunks: [CHUNKS.LEADFORM, "vendor", "bootstrap"]
    }),

    new InjectManifest({
      swSrc: PATHS.SOURCE_DIR + "/sw.js",
      swDest: PATHS.BUILD_DIR + "/sw.js",
      excludeChunks: MANIFEST_EXCLUDE_CHUNKS,
      exclude: [
        /emiapp/,
        /valuation/,
        /location/,
        /\.map$/,
        /\.LICENSE$/,
        /\.html$/,
        /lead-form/,
        /leadform/,
        /filterplugin/
      ],
      importsDirectory: "js"
    })
  ]
});

module.exports = prodConfig;
