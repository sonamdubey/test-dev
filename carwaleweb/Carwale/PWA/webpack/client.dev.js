const {
  clientCommon,
  PATHS,
  CHUNKS,
  MANIFEST_EXCLUDE_CHUNKS
} = require("./common.config");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const webpack = require("webpack");
const HtmlWebPackPlugin = require("html-webpack-plugin");
const merge = require("webpack-merge");
const { InjectManifest } = require("workbox-webpack-plugin");

const devConfig = merge(clientCommon, {
  devtool: "eval",
  output: {
    filename: "[name].js",
    publicPath: "/",
    chunkFilename: "[name].js"
  },
  devServer: {
    contentBase: PATHS.BUILD_DIR,
    historyApiFallback: {
      rewrites: [{ from: /^\/find-car.*$/, to: "/find-car/index.html" },
                  { from: /^\/best-([a-zA-Z]+)-cars-in-india$/, to: "/find-car/index.html" }]
    },
    compress: true,
    port: 9001,
    publicPath: "/",
    host: "0.0.0.0",
    proxy: [
      {
        context: ["/api/prices/version/**"],
        target: "https://staging.carwale.com:9007",
        changeOrigin: true
      },
      {
        context: ["/api/dealer/inquiries/**"],
        target: "http://localhost",
        changeOrigin: true
      },
      {
        context: ["/api/**", "/webapi/**"],
				target: "http://localhost",
        changeOrigin: true
      },
      {
        context: ["/m/used/**", "/used/**"],
        target: "http://localhost",
        changeOrigin: true
      },
      {
        context: ["/static/**"],
        target: "http://localhost",
        changeOrigin: true
      }
    ]
  },
  plugins: [
    new webpack.DefinePlugin({
      "process.env": {
        NODE_ENV: JSON.stringify("development")
      },
      __DEV__: true,
      __PROD__: false,
      COOKIE_DOMAIN: JSON.stringify("localhost"),
      BHRIGU_HOST_URL: JSON.stringify("/bhrigu/pixel.gif?t=")
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: "vendor",
      minChunks: module => {
        // this assumes your vendor imports exist in the node_modules directory

        return module.context && module.context.indexOf("node_modules") !== -1;
      }
    }),
    new ExtractTextPlugin({
      filename: "[name].css"
    }),
    // new ExtractCssChunks({
    // 	filename: 'prices.bundle.css'
    // }),
    new HtmlWebPackPlugin({
      filename: "find-car/index.html",
      template: "index.html",
      chunks: [CHUNKS.NCF, "vendor", "bootstrap"]
    }),
    new HtmlWebPackPlugin({
      filename: "emiapp/index.html",
      template: "emiapptemplate.html",
      inject: false,
      chunks: [CHUNKS.EMI, "vendor", "bootstrap"]
    }),
    new HtmlWebPackPlugin({
      filename: "used/carvaluation/index.html",
      template: "index.html",
      chunks: [CHUNKS.VALUATION, "vendor", "bootstrap"],
      title: "Check value of your car for FREE - CarWale"
    }),
		new HtmlWebPackPlugin({
			filename: 'filterplugin/index.html',
			template: 'filterPluginTemplate.html',
			inject: false,
			chunks: [CHUNKS.FILTERPLUGIN, 'vendor', 'bootstrap']
		}),
    new InjectManifest({
      swSrc: PATHS.SOURCE_DIR + "/sw.dev.js",
      swDest: PATHS.BUILD_DIR + "/sw.js",
			excludeChunks: MANIFEST_EXCLUDE_CHUNKS,
			exclude:[/emiapp/,/filterplugin/,/lead-form/,/valuation/,/\.html$/,/find-car/,/location/]
    }),
    new HtmlWebPackPlugin({
      filename: "location/index.html",
      template: "template.html",
      inject: false,
      chunks: [CHUNKS.LOCATION, "vendor", "bootstrap"]
    }),

    new HtmlWebPackPlugin({
      filename: "used/buying-assisted/index.html",
      template: "locationtemplate.html",
      inject: false,
      chunks: [CHUNKS.BUYING_ASSISTED_CITY, "vendor", "bootstrap"]
    }),
    new HtmlWebPackPlugin({
      filename: "lead-form/index.html",
      template: "leadFormTemplate.html",
      inject: false,
      chunks: [CHUNKS.LEADFORM, "vendor", "bootstrap"]
    })
  ]
});

module.exports = devConfig;
