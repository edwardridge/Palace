module.exports = {
    entry: { main: "./Scripts/Game/main.jsx", create: "./Scripts/CreateGame/CreateGame.jsx" },
    output: {
        path: __dirname + '/Scripts',
        filename: "[name].js"
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
    }
    //,
    //resolve: {
    //    root: [
    //        path.resolve('C:\Users\Ed\Documents\GitHub\Palace\Palace.Website\Scripts')
    //    ]
    //}
};