module.exports = {
  verbose: true,
  setupFiles: ["./jest.setup.js"],
  snapshotSerializers: ["enzyme-to-json/serializer"],
  testPathIgnorePatterns: ["/node_modules/", "/build/"]
}
