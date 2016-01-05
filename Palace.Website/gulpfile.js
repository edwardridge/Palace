/// <binding AfterBuild='moveScripts' ProjectOpened='watch-sass' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');
var cpy = require('cpy');
var plugins = require('gulp-load-plugins')();

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

gulp.task('moveScripts', ['sass'], function () {
    //del('./Scripts/Libraries');
    //del('./Scripts/Libraries');
    cpy(jslibraries, './Scripts/Libraries', function (err) {
        if (err) { console.log(err) };
    });

    cpy(csslibraries, './Content/Libraries', function (err) {
        if (err) { console.log(err) };
    });
});

gulp.task('sass', function () {
    return gulp.src('./Content/SASS/*.scss')
        .pipe(plugins.sourcemaps.init())
        .pipe(plugins.sass())
        .pipe(plugins.autoprefixer())
        .pipe(plugins.sourcemaps.write())
        .pipe(gulp.dest('./Content/Site'))
        .pipe(plugins.livereload())
    ;
});

gulp.task('watch-sass', function () {
    //plugins.livereload.listen();
    //gulp.watch('./Content/*.scss', ['sass']);
});

gulp.task('default', ['watch-sass']);
