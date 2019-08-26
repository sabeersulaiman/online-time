const { dest, src, series, parallel, watch } = require('gulp');
const browserSync = require('browser-sync');

function initBrowserSync() {
    browserSync.init({
        server: './dist'
    });
}

function watchForChanges() {
    watch('app/*.html', processHTMLfiles).on('change', function() {
        browserSync.reload();
    });
}

function processHTMLfiles() {
    return src('app/*.html').pipe(dest('dist/'));
}

exports.default = series(
    processHTMLfiles,
    parallel(watchForChanges, initBrowserSync)
);
