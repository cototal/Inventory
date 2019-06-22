const Encore = require("@symfony/webpack-encore");

Encore
    .setOutputPath("../Inventory.Web/wwwroot/build")
    .setPublicPath("/build")
    .addEntry("app", "./assets/js/app.js")
    .enableSingleRuntimeChunk()
    .cleanupOutputBeforeBuild()
    .enableBuildNotifications()
    .enableSourceMaps(!Encore.isProduction())
    .enableVersioning(Encore.isProduction())
    // This generates a warning in the console
    .configureBabel(null, {
        useBuiltIns: "usage",
        corejs: 3
    })
    .enableSassLoader()
    .autoProvideVariables({
        $: "cash-dom/dist/cash.js"
    })
;

module.exports = Encore.getWebpackConfig();
