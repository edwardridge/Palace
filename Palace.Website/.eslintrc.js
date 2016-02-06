module.exports = {
    "parser": "babel-eslint",
    "rules": {
        "indent": [
            0,
            "tab"
        ],
        "quotes": [
            2,
            "double"
        ],
        "linebreak-style": [
            2,
            "windows"
        ],
        "semi": [
            2,
            "always"
        ],
        "no-irregular-whitespace": 0
    },
    "env": {
        "es6": true,
        "browser": true
    },
    "extends": "eslint:recommended",
    "ecmaFeatures": {
        "jsx": true,
        "experimentalObjectRestSpread": true,
        "modules": true,
        "arrowFunctions": true
    },
    "plugins": [
        "react"
    ],
    "globals": {
        "React": true,
        "ReactDOM": true,
        "PalaceConfig": true,
        "$": true
    }
};