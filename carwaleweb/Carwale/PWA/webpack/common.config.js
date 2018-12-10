const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanPlugin = require("clean-webpack-plugin");
const fs = require("fs");
const joinPath = pathname => path.join(__dirname, "..", pathname);

const PATHS = {
  SOURCE_DIR: joinPath("src"),
  BUILD_DIR: joinPath("build"),
  ROOT_DIR: joinPath(""),
  NODE_MODULES: joinPath("node_modules")
};

const CHUNKS = {
  NCF: 'ncfapp',
  EMI: 'emiapp',
  VALUATION: 'valuationapp',
  LEADFORM: 'leadformapp',
  FILTERPLUGIN: 'filterplugin',
  LOCATION: 'locationapp'
};

const MANIFEST_EXCLUDE_CHUNKS = [CHUNKS.VALUATION, CHUNKS.EMI, CHUNKS.LOCATION, CHUNKS.LEADFORM, CHUNKS.FILTERPLUGIN];

let clientCommon = {
  entry: {
    leadformapp: PATHS.SOURCE_DIR + "/LeadForm/entry.js",
    ncfapp: PATHS.SOURCE_DIR + "/NewCarFinder/entry.js",
    emiapp: PATHS.SOURCE_DIR + "/pricesEmi/entry.js",
    valuationapp: PATHS.SOURCE_DIR + "/used/entry.js",
    locationapp: PATHS.SOURCE_DIR + "/Location/entry.js",
    filterplugin: PATHS.SOURCE_DIR + '/filterPlugin/entry.js'
  },
  output: {
    path: PATHS.BUILD_DIR,
    filename: "[name].js",
    chunkFilename: "[name].js"
  },
  module: {
    rules: [
      {
        test: /\.jsx?/,
        include: PATHS.SOURCE_DIR,
        use: [
          {
            loader: "babel-loader"
          },
          {
            loader: "eslint-loader",
            options: {
              failOnError: true
            }
          }
        ]
      },
      {
        test: /\.(sass|scss)$/,
        loader: ExtractTextPlugin.extract(["css-loader", "sass-loader"])
      }
    ]
  },

  resolve: {
    extensions: [".js", ".jsx"],
    alias: {
      GlobalComponent: path.resolve(__dirname, "..", "src/components/"),
      GlobalReducer: path.resolve(__dirname, "..", "src/reducers/"),
      GlobalStyle: path.resolve(__dirname, "..", "style/"),
      GlobalActionCreator: path.resolve(__dirname, "..", "src/actionCreators"),
      GlobalUtils: path.resolve(__dirname, "..", "src/utils/")
    }
  },

  plugins: [
    new CleanPlugin([PATHS.BUILD_DIR], {
      root: PATHS.ROOT_DIR
    })
  ]
};

/**
 * @const workBoxLibMap
 * @description This constant holds an array of objects containing all the workbox libs
 * 				required and in a format compatible for copy-webpack-plugin as all the
 * 				libs are copied from `node_modules` folder to the `public` folder.
 *
 */
const workBoxLibMap = env => {
  return fs
    .readdirSync(PATHS.NODE_MODULES)
    .filter(name => /^workbox/.test(name) && !/webpack|build/.test(name)) // only pick workbox-sw directory
    .map(name => {
      const libPath = `${PATHS.NODE_MODULES}/${name}/build/importScripts/`;
      const libFileName = fs
        .readdirSync(libPath)
        .filter(name => new RegExp(env).test(name))
        .map(fileName => {
          return {
            from: `${libPath}${fileName}`,
            to: "workbox-sw.js"
          };
        });

      return libFileName;
    })
    .reduce((acc, val) => acc.concat(val), []);
};

module.exports = {
  clientCommon,
  PATHS,
  CHUNKS,
  MANIFEST_EXCLUDE_CHUNKS,
  workBoxLibMap
};
