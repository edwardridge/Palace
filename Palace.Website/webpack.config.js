module.exports = {
    entry: "./Scripts/Game/main.jsx",
    output: {
        path: __dirname + '/Scripts',
        filename: "webpack_bundle.js"
    },
    module: {
        loaders: [
            {
                test: /\.jsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel', // 'babel-loader' is also a legal name to reference
                query: {
                    presets: ['es2015','stage-1','react']
                }
            }
        ]
    }
};