"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b)
            if (b.hasOwnProperty(p))
                d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

"use strict";
var f14;
(function (f14) {
    var L10n;
    (function (L10n) {
        var DefaultL10NProvider = /** @class */ (function () {
            function DefaultL10NProvider() {
                this._locales = {};
                this._currentLang = navigator.language;
            }
            DefaultL10NProvider.prototype.AddLocale = function (localeKey, localeData) {
                // Createss new locale if not exist.
                if (!this._locales[localeKey]) {
                    this._locales[localeKey] = {};
                }
                // Get locale for given locale key.
                var currentLocalData = this._locales[localeKey];
                // Apply given locale data to existing.
                for (var key in localeData) {
                    currentLocalData[key] = localeData[key];
                }
            };
            DefaultL10NProvider.prototype.GetString = function (stringKey) {
                return this.GetStringForLocale(this._currentLang, stringKey);
            };
            DefaultL10NProvider.prototype.GetStringForLocale = function (localeKey, stringKey) {
                // Tries to find target string for the given local key.
                var stringData = this.getLocalizedString(localeKey, stringKey);
                if (stringData) {
                    return stringData;
                }
                // Tries to find target string for the local key by default.
                if (L10n.Config.DefaultLocale) {
                    stringData = this.getLocalizedString(L10n.Config.DefaultLocale, stringKey);
                    if (stringData) {
                        return stringData;
                    }
                }
                // Tries to find target string in all available locales.
                for (var lk in this._locales) {
                    stringData = this.getLocalizedString(lk, stringKey);
                    if (stringData) {
                        return stringData;
                    }
                }
            };
            DefaultL10NProvider.prototype.GetLocales = function () {
                return this._locales;
            };
            DefaultL10NProvider.prototype.getLocalizedString = function (localeKey, stringKey) {
                if (this._locales[localeKey]) {
                    return this._locales[localeKey][stringKey];
                }
            };
            return DefaultL10NProvider;
        }());
        /**
         * Configuration for f14-l10n.
         */
        var Configuration = /** @class */ (function () {
            function Configuration() {
                /**
                 * Trun on\off debug mode. Default: false.
                 */
                this.DEBUG = false;
                /**
                 * Sets the IL10NProvider as window property with property name WindowPartProperty.
                 */
                this.AsWindowPart = false;
                /**
                 * This name using if AsWindowPart is true. Default: l10nProvider.
                 */
                this.WindowPartProperty = "l10nProvider";
                /**
                 * Specifed IL10NProvider. Default: DefaultL10NProvider.
                 */
                this.L10nProvider = new DefaultL10NProvider();
            }
            return Configuration;
        }());
        L10n.Configuration = Configuration;
        /**
         * Configuration instance.
         */
        L10n.Config = new Configuration();
        /**
         * Setup the L10NProvider.
         * @param settings User configuration.
         */
        function Setup(settings) {
            if (settings) {
                for (var p in settings) {
                    L10n.Config[p] = settings[p];
                }
            }
            if (!L10n.Config.L10nProvider) {
                console.log("L10nProvider is undefined or null. Create default l10nProvider.");
                L10n.Config.L10nProvider = new DefaultL10NProvider();
            }
            if (L10n.Config.AsWindowPart) {
                if (!L10n.Config.WindowPartProperty || L10n.Config.WindowPartProperty.length === 0) {
                    throw "L10nProvider setting up as window property, but window property name is empty or undefined.";
                }
                window[L10n.Config.WindowPartProperty] = L10n.Config.L10nProvider;
            }
        }
        L10n.Setup = Setup;
        /**
         * Returns configured IL10NProvider or throws exception.
         */
        function Localizer() {
            if (!L10n.Config.L10nProvider) {
                throw "L10NProvider not initialized. Call 'Setup' method to do this.";
            }
            return L10n.Config.L10nProvider;
        }
        L10n.Localizer = Localizer;
    })(L10n = f14.L10n || (f14.L10n = {}));
})(f14 || (f14 = {}));
