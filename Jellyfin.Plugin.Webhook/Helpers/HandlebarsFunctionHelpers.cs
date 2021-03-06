﻿using System;
using HandlebarsDotNet;

namespace Jellyfin.Plugin.Webhook.Helpers
{
    /// <summary>
    /// Handlebar helpers.
    /// </summary>
    public static class HandlebarsFunctionHelpers
    {
        private static readonly HandlebarsBlockHelper StringEqualityHelper = (output, options, context, arguments) =>
        {
            if (arguments.Length != 2)
            {
                throw new HandlebarsException("{{if_equals}} helper must have exactly two arguments");
            }

            var left = arguments[0] as string;
            var right = arguments[1] as string;
            if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
            {
                options.Template(output, context);
            }
            else
            {
                options.Inverse(output, context);
            }
        };

        private static readonly HandlebarsBlockHelper StringExistHelper = (output, options, context, arguments) =>
        {
            if (arguments.Length != 1)
            {
                throw new HandlebarsException("{{if_equals}} helper must have exactly one argument");
            }

            var arg = arguments[0] as string;
            if (string.IsNullOrEmpty(arg))
            {
                options.Inverse(output, context);
            }
            else
            {
                options.Template(output, context);
            }
        };

        /// <summary>
        /// Register handlebars helpers.
        /// </summary>
        public static void RegisterHelpers()
        {
            Handlebars.RegisterHelper("if_equals", StringEqualityHelper);
            Handlebars.RegisterHelper("if_exist", StringExistHelper);
            Handlebars.RegisterHelper("link_to", (writer, context, parameters) =>
            {
                writer.WriteSafeString($"<a href='{(object)context.url}'>{(object)context.text}</a>");
            });
        }

        /// <summary>
        /// Base 64 decode.
        /// </summary>
        /// <remarks>
        /// The template is stored as base64 in config.
        /// </remarks>
        /// <param name="base64EncodedData">The encoded data.</param>
        /// <returns>The decoded string.</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}