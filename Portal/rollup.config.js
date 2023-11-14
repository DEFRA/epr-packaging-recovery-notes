import resolve from '@rollup/plugin-node-resolve';
import commonjs from '@rollup/plugin-commonjs';
import json from '@rollup/plugin-json';
import { terser } from 'rollup-plugin-terser';

const path = require('path');
const inject = require("rollup-plugin-inject");

var isProd = false;

export default {
    input: './ClientApp/js/index.js',
    plugins: [
        commonjs(),
        inject({
            modules: {
                '$': 'jquery',
                'jQuery': 'jquery',
            }
        }),
        resolve(),
        terser(),
    ],
    output: {
        file: './wwwroot/js/index.js',
        format: "iife",
        name: "app",
        sourcemap: process.env.NODE_ENV !== 'production'
    }
};