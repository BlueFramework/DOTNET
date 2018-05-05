﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFramework.Common.Logger
{
    /// <summary>
    /// logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// write information
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// write error message
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// write warnning message
        /// </summary>
        /// <param name="meeeage"></param>
        void Warn(string meeeage);
    }
}