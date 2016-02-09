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
                loader: 'babel',
                query: {
                    presets: ['es2015','stage-1','react']
                }
            }
        ]
    },
    resolve: {
        root: [
            path.resolve('C:\Users\Ed\Documents\GitHubVisualStudio\Palace\Palace.Website\Scripts')
        ]
    }
};