﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ligg.Base.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class BaseRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BaseRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ligg.Base.Resources.BaseRes", typeof(BaseRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Days.
        /// </summary>
        public static string Days {
            get {
                return ResourceManager.GetString("Days", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hours.
        /// </summary>
        public static string Hours {
            get {
                return ResourceManager.GetString("Hours", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MilliSeconds.
        /// </summary>
        public static string MilliSeconds {
            get {
                return ResourceManager.GetString("MilliSeconds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minutes.
        /// </summary>
        public static string Minutes {
            get {
                return ResourceManager.GetString("Minutes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seconds.
        /// </summary>
        public static string Seconds {
            get {
                return ResourceManager.GetString("Seconds", resourceCulture);
            }
        }
    }
}
