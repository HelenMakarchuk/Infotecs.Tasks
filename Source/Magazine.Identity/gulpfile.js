/// <binding BeforeBuild='less' />
"use strict";

var gulp = require("gulp"),
    less = require("gulp-less");

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("less", function () {
    return gulp.src('wwwroot/less/styles.less')
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + '/css'))
});
