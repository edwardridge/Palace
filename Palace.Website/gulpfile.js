const gulp = require('gulp');
const del = require('del');
const cpy = require('cpy');
const plugins = require('gulp-load-plugins')();

var jslibraries = [
    './wwwroot/lib/react/react.js',
    './wwwroot/lib/react/react-dom.js',
    './wwwroot/lib/jquery/dist/jquery.js',
    './wwwroot/lib/bootstrap/dist/js/bootstrap.js',
    './wwwroot/lib/underscore/underscore.js',
]

var csslibraries = [
    './wwwroot/lib/bootstrap/dist/css/bootstrap.css'
];

gulp.task('moveScripts', ['sass'], () => {
    cpy(jslibraries, './Scripts/Libraries', function (err) {
        if (err) { console.log(err) };
    });

    cpy(csslibraries, './Content/Libraries', function (err) {
        if (err) { console.log(err) };
    });
});

gulp.task('sass', () => {
    return gulp.src('./Content/SASS/*.scss')
        .pipe(plugins.sourcemaps.init())
        .pipe(plugins.sass())
        .pipe(plugins.autoprefixer())
        .pipe(plugins.sourcemaps.write())
        .pipe(gulp.dest('./Content/Site'))
        .pipe(plugins.livereload())
    ;
});

gulp.task('watch-sass', () => {
    //plugins.livereload.listen();
    gulp.watch('./Content/*.scss', ['sass']);
});

gulp.task('transpile', () => {
    	return gulp.src('Scripts/**/*.jsx')
                .pipe(plugins.babel({
                    presets: ['es2015','react']
                }))
                .pipe(gulp.dest('Scripts/dist'));
});

gulp.task('default', ['sass', 'transpile']);
