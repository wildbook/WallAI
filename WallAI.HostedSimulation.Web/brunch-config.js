exports.files = {
    javascripts: {
        joinTo: 'app.js'
    },
    stylesheets: {
        joinTo: 'app.css',
    }
};

exports.modules = {
	autoRequire: {
		'app.js': ['App']
    }
};

exports.plugins = {
    babel: {
        presets: ['latest']
    },
    brunchTypescript: {
        target: "es6",
        removeComments: true,
        emitDecoratorMetadata: true,
        experimentalDecorators: true,
    }
};