﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public static class Constants
    {
        public static class SpecFlow
        {
            public const string TABLE_KEY = "Property";
            public const string TABLE_VALUE = "Value";
            public const string TABLE_FORMSTATE = "State";
            public const string TABLE_FORMNOTIFICATION_MESSAGE = "Message";
            public const string TABLE_FORMNOTIFICATION_LEVEL = "Level";

            public const string TARGET_API = "API";
            public const string TARGET_Chrome = "Chrome";
            public const string TARGET_Firefox = "Firefox";
            public const string TARGET_InternetExplorer = "IE";
            public const string TARGET_Edge = "Edge";
        }

        public static class ErrorCodes
        {
            public const int ERRORMESSAGE_NOT_FOUND = 1;
            public const int FORMLOAD_SCRIPT_ERROR_ON_FORM = 2;
            public const int BROWSER_OT_SUPPORTED_FOR_API_TEST = 3;
            public const int N_N_RELATIONSHIP_NOT_FOUND = 4;
            public const int RECORD_NOT_FOUND = 5;
            public const int UNKNOWN_TAG = 6;
            public const int API_NOT_SUPPORTED_FOR_BROWSER_ONLY_COMMANDS = 7;
            public const int CURRENT_BUSINESS_PROCESS_STAGE_NOT_FOUND = 8;
            public const int BUSINESS_PROCESS_STAGE_NOT_IN_ACTIVE_PATH = 9;
            public const int BUSINESS_PROCESS_STAGE_CANNOT_BE_LAST = 10;
            public const int LANGUAGECODE_MUST_BE_INTEGER = 11;
            public const int FORM_SAVE_FAILED = 12;
            public const int FORM_SAVE_TIMEOUT = 13;
            public const int DUPLICATE_RECORD_DETECTED = 14;
            public const int FIELD_NOT_ON_FORM = 15;
            public const int TWO_OPTION_FIELDS_CANT_BE_CLEARED = 16;
            public const int MORE_COMMANDS_NOT_FOUND = 17;
            public const int UNKNOWN_FORM_NOTIFICATION_TYPE = 18;
            public const int INVALID_FORM_STATE = 19;
            public const int APP_SETTINGS_REQUIRED = 20;
            public const int LABEL_NOT_TRANSLATED = 21;
            public const int ATTRIBUTE_DOESNT_EXIST = 22;
            public const int LOOKUP_NOT_FOUND = 23;
            public const int OPTION_NOT_FOUND = 24;
            public const int CANT_START_BROWSER_FOR_API_TESTS = 25;
            public const int FORM_LOAD_TIMEOUT = 26;
            public const int UNABLE_TO_LOGIN = 27;
            public const int FORM_NOT_FOUND = 28;
            public const int APP_NOT_FOUND = 29;
            public const int INVALID_DATATYPE = 30;
            public const int VALUE_NULL = 31;
            public const int ELEMENT_NOT_INTERACTABLE = 32;
            public const int BUSINESS_PROCESS_ERROR_WHEN_LOADING = 33;
            public const int ASYNC_TIMEOUT = 34;
            public const int MULTIPLE_ATTRIBUTES_FOUND = 35;
            public const int APPLICATIONUSER_CANNOT_LOGIN = 36;
            public const int NO_CONTROL_FOUND = 37;
            public const int RIBBON_NOT_FOUND = 38;
            public const int ALIAS_DOESNT_EXIST = 39;
            public const int APP_UNSELECTED = 40;
            public const int FIELD_INVISIBLE = 41;
            public const int FIELD_READ_ONLY = 42;
            public const int RECORD_DOESNT_MATCH = 43;
        }

        public static class AppSettings
        {
            public const string ADMIN_PASSWORD = "AdminPassword";
            public const string ADMIN_USERNAME = "AdminUsername";
            public const string APP_NAME = "AppName";
            public const string ASYNC_JOB_TIMEOUT = "AsyncJobTimeoutInSeconds";
            public const string AUTH_TYPE = "AuthType";
            public const string DATE_FORMAT = "DateFormat";
            public const string LANGUAGE_CODE = "LanguageCode";
            public const string PASSWORD = "Password";
            public const string LOGIN_TYPE = "LoginType";
            public const string TARGET = "Target";
            public const string TIME_FORMAT = "TimeFormat";
            public const string URL = "Url";
            public const string USERNAME = "Username";
        }

        internal static class JavaScriptCommands
        {            
            internal const string GET_FORM_ATTRIBUTES = @"
                return Xrm.Page.data.entity.attributes.getAll()
                    .map(function(a) { 
                        return { 
                            name: a.getName(), 
                            controls: a.controls.getAll().map(function(c) { 
                                return c.getName()
                            }) 
                        } 
                    })";

            internal const string GET_FORM_ID = "return Xrm.Page.ui.formSelector.getCurrentItem().getId()";

            internal const string GET_FORM_STRUCTURE = @"
                return Xrm.Page.ui.tabs.getAll()
                    .map(function(t) {
                        return {
                            name: t.getName(),
                            label: t.getLabel(),
                            sections: t.sections.getAll().map(function(s) {
                                return {
                                    name: s.getName(),
                                    label: s.getLabel(),
                                    controls: s.controls.getAll().map(function(c) {
                                        return {
                                            name: c.getName(),
                                            type: c.getControlType()
                                        }
                                    })
                                }
                            })
                        }	
                    })";

        }
    }
}