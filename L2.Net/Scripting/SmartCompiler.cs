using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace L2.Net
{
    /// <summary>
    /// Script files types.
    /// </summary>
    public enum ScriptFileType : byte
    {
        /// <summary>
        /// CSharp file type.
        /// </summary>
        CS,
        /// <summary>
        /// Visual Basic file type.
        /// </summary>
        VB
    }

    /// <summary>
    /// Externally coded scripts compiler.
    /// </summary>
    public static class SmartCompiler
    {
        private const string OutputAssemblyPrefix = "L2.Net.Scripts.";
        private static string m_ScriptsBaseDirectory;
        private static string m_ReferenceAssembliesConfig;

        /// <summary>
        /// Compiled objects types cache.
        /// </summary>
        private static readonly SortedDictionary<Type, List<object>> m_TypesCache = new SortedDictionary<Type, List<object>>();

        /// <summary>
        /// Initializes <see cref="SmartCompiler"/> class.
        /// </summary>
        /// <param name="scriptsDirectory">Base directory, that contains externally coded scripts.</param>
        /// <param name="referenceAssembliesConfig">Path to the file, that contains list of reference assemblies, needed to compile external scripts.</param>
        public static void Initialize( string scriptsDirectory, string referenceAssembliesConfig )
        {
            if ( !scriptsDirectory.StartsWith(AppDomain.CurrentDomain.BaseDirectory) )
                scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptsDirectory);

            m_ScriptsBaseDirectory = scriptsDirectory;

            if ( !referenceAssembliesConfig.StartsWith(m_ScriptsBaseDirectory) )
                referenceAssembliesConfig = Path.Combine(m_ScriptsBaseDirectory, referenceAssembliesConfig);

            m_ReferenceAssembliesConfig = referenceAssembliesConfig;

            EnsureOutputDirectory();
        }

        /// <summary>
        /// Verifies that output directory is empty.
        /// </summary>
        private static void EnsureOutputDirectory()
        {
            string outputPath = Path.Combine(m_ScriptsBaseDirectory, "Output");

            if ( Directory.Exists(outputPath) )
                Directory.Delete(outputPath, true);

            Directory.CreateDirectory(outputPath);
        }


        /// <summary>
        /// Compiles external scripts.
        /// </summary>
        /// <param name="directory">Directory to search scripts in.</param>
        /// <param name="type">Type of files to search.</param>
        /// <param name="recursive">False, if compiler has to search files in top directory only, otherwise true.</param>
        /// <param name="completeDelegate"></param>
        public static void Compile( string directory, ScriptFileType type, bool recursive, EventHandler completeDelegate )
        {
            Assembly assembly = null;

            if ( Compile(directory, type, recursive, out assembly) )
            {
                Type[] types = assembly.GetTypes();

                object obj = null;
                Type t;
                ConstructorInfo[] ctors;
                List<object> ctorparams;
                MethodInfo mi;

                for ( int i = 0; i < types.Length; i++ )
                {
                    t = types[i];

                    try
                    {
                        ctors = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

                        if ( ctors.Length == 0 )
                            obj = Activator.CreateInstance(t);
                        else
                        {
                            ctorparams = new List<object>();

                            foreach ( ParameterInfo pi in ctors[0].GetParameters() )
                                ctorparams.Add(Activator.CreateInstance(pi.ParameterType));

                            obj = ctors[0].Invoke(ctorparams.ToArray());
                        }

                        mi = t.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public);

                        if ( mi != null )
                            obj = mi.Invoke(obj, null);

                        CacheType(t, obj);
                    }
                    catch ( Exception e )
                    {
                        Logger.WriteLine(Source.ScriptsCompiler, "Failed to initialize Type {0}", t);
                        Logger.Exception(e);
                    }
                }
            }

            if ( completeDelegate != null )
                completeDelegate.EndInvoke(null);
        }

        /// <summary>
        /// Adds object to types cache.
        /// </summary>
        /// <param name="t">Object <see cref="Type"/>.</param>
        /// <param name="o"><see cref="Object"/> to add.</param>
        private static void CacheType( Type t, object o )
        {
            if ( !m_TypesCache.ContainsKey(t) )
                m_TypesCache.Add(t, new List<object>() { o });
            else
                m_TypesCache[t].Add(o);
        }

        /// <summary>
        /// Searches for all objects, which parent class <see cref="Type"/> is <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Parent class <see cref="Type"/>.</typeparam>
        /// <returns>Collection of objects, derived from <see cref="Type"/> <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> FindTypeByBase<T>()
        {
            foreach ( KeyValuePair<Type, List<object>> kvp in m_TypesCache )
                if ( kvp.Key.BaseType == typeof(T) )
                    return Convert<T>(kvp.Value);

            return new T[0] { };
        }

        /// <summary>
        /// Searches for all objects, with <see cref="Type"/> is <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of objects to look for.</typeparam>
        /// <returns>Collection of objects of <see cref="Type"/> <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> FindType<T>()
        {
            foreach ( KeyValuePair<Type, List<object>> kvp in m_TypesCache )
                if ( kvp.Key == typeof(T) )
                    return Convert<T>(kvp.Value);

            return new T[0] { };
        }

        /// <summary>
        /// Converts provided objects collection to <see cref="Type"/> <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Result collection objects <see cref="Type"/>.</typeparam>
        /// <param name="objects">Objects collection.</param>
        /// <returns>Collection of <see cref="Type"/> <typeparamref name="T"/>.</returns>
        private static IEnumerable<T> Convert<T>( IEnumerable<object> objects )
        {
            foreach ( object obj in objects )
                yield return ( T )obj;
        }

        /// <summary>
        /// Compiles external scripts.
        /// </summary>
        /// <param name="directory">Directory to search scripts in.</param>
        /// <param name="type">Type of files to search.</param>
        /// <param name="recursive">False, if compiler has to search files in top directory only, otherwise true.</param>
        /// <param name="assembly">Output assembly.</param>
        /// <returns>True, if external scripts compiled witout errors, otherwise false.</returns>
        /// <exception cref="NotImplementedException" />
        private static bool Compile( string directory, ScriptFileType type, bool recursive, out Assembly assembly )
        {
            Logger.WriteLine(Source.ScriptsCompiler, "Compiling {0} scripts in {1}.", type == ScriptFileType.CS ? "C#" : "VB", directory);

            string[] files = GetScripts(directory, type, recursive);

            if ( files.Length == 0 )
            {
                Logger.WriteLine(Source.ScriptsCompiler, "No {0} files found.", type == ScriptFileType.CS ? "C#" : "VB");
                assembly = null;
                return false;
            }

            string path = Path.Combine(m_ScriptsBaseDirectory, "Output");
            path = Path.Combine(path, OutputAssemblyPrefix + directory + "." + DateTime.Now.Ticks.ToString() + ".dll");

            //if ( File.Exists(path) )
            //    File.Delete(path);

            CompilerParameters options = new CompilerParameters(GetReferenceAssemblies(), path, false);
            options.GenerateInMemory = true;
            CompilerResults results;

            Logger.WriteLine(Source.ScriptsCompiler, "Compiling {0} {1} files in {2}", files.Length, type, directory);

            switch ( type )
            {
                case ScriptFileType.CS:
                    {
                        using ( CSharpCodeProvider provider = new CSharpCodeProvider() )
                            results = provider.CompileAssemblyFromFile(options, files);
                        break;
                    }
                case ScriptFileType.VB:
                    {
                        using ( VBCodeProvider provider = new VBCodeProvider() )
                            results = provider.CompileAssemblyFromFile(options, files);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }

            ShowResults(results);

            if ( results.Errors.Count > 0 )
                Logger.WriteLine(Source.ScriptsCompiler, "Warning: not all scripts were compiled!!!");

            if ( results.Errors.Count > 0 )
                assembly = null;
            else
                assembly = results.CompiledAssembly;

            return assembly != null;
        }

        /// <summary>
        /// Displays / logs compilation results.
        /// </summary>
        /// <param name="results">Compiler results object.</param>
        private static void ShowResults( CompilerResults results )
        {
            if ( results.Errors.Count > 0 )
            {
                Dictionary<string, List<CompilerError>> errors = new Dictionary<string, List<CompilerError>>(results.Errors.Count, StringComparer.OrdinalIgnoreCase);
                Dictionary<string, List<CompilerError>> warnings = new Dictionary<string, List<CompilerError>>(results.Errors.Count, StringComparer.OrdinalIgnoreCase);

                foreach ( CompilerError e in results.Errors )
                {
                    Dictionary<string, List<CompilerError>> table = ( e.IsWarning ? warnings : errors );

                    List<CompilerError> list = null;
                    table.TryGetValue(e.FileName, out list);

                    if ( list == null )
                        table[e.FileName] = list = new List<CompilerError>();

                    list.Add(e);
                }

                if ( errors.Count > 0 )
                    Logger.WriteLine(Source.ScriptsCompiler, "Failed ({0} errors, {1} warnings)", errors.Count, warnings.Count);
                else
                    Logger.WriteLine(Source.ScriptsCompiler, "Complete ({0} errors, {1} warnings)", errors.Count, warnings.Count);

                string scriptRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts" + Path.DirectorySeparatorChar));
                Uri scriptRootUri = new Uri(scriptRoot);

                if ( warnings.Count > 0 )
                    Logger.WriteLine(Source.ScriptsCompiler, "ScriptsCompiler warnings:");

                foreach ( KeyValuePair<string, List<CompilerError>> kvp in warnings )
                {
                    string fileName = kvp.Key;
                    List<CompilerError> list = kvp.Value;

                    string fullPath = Path.GetFullPath(fileName);
                    string usedPath = Uri.UnescapeDataString(scriptRootUri.MakeRelativeUri(new Uri(fullPath)).OriginalString);

                    Logger.WriteLine(Source.ScriptsCompiler, " + {0}:", usedPath);

                    foreach ( CompilerError e in list )
                        Logger.WriteLine(Source.ScriptsCompiler, "    {0}: Line {1}: {3}", e.ErrorNumber, e.Line, e.Column, e.ErrorText);
                }

                if ( errors.Count > 0 )
                    Logger.WriteLine(Source.ScriptsCompiler, "ScriptsCompiler errors:");

                foreach ( KeyValuePair<string, List<CompilerError>> kvp in errors )
                {
                    string fileName = kvp.Key;
                    List<CompilerError> list = kvp.Value;

                    if ( !String.IsNullOrEmpty(fileName) )
                    {
                        string fullPath = Path.GetFullPath(fileName);
                        string usedPath = Uri.UnescapeDataString(scriptRootUri.MakeRelativeUri(new Uri(fullPath)).OriginalString);

                        Logger.WriteLine(Source.ScriptsCompiler, " + {0}:", usedPath);
                    }
                    foreach ( CompilerError e in list )
                        Logger.WriteLine(Source.ScriptsCompiler, "    {0}: Line {1}: {3}", e.ErrorNumber, e.Line, e.Column, e.ErrorText);
                }

            }
            else
                Logger.WriteLine(Source.ScriptsCompiler, "Complete (0 errors, 0 warnings)");
        }

        /// <summary>
        /// Returns array of reference assemblies.
        /// </summary>
        /// <returns>Array of reference assemblies.</returns>
        /// <exception cref="FileNotFoundException" />
        private static string[] GetReferenceAssemblies()
        {
            List<string> assemblies = new List<string>();

            assemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName));

            if ( !File.Exists(m_ReferenceAssembliesConfig) )
                throw new FileNotFoundException("Failed to load reference assemblies configuration file.");

            using ( StreamReader sr = new StreamReader(m_ReferenceAssembliesConfig) )
            {
                string assembly;

                while ( ( assembly = sr.ReadLine() ) != null )
                    if ( assembly[0] != '#' )
                        assemblies.Add(assembly);
            }

            return assemblies.ToArray();
        }

        /// <summary>
        /// Retrieves array of scripts file names, found in provided directory.
        /// </summary>
        /// <param name="directory">Directory to search scripts in.</param>
        /// <param name="type">Type of files to search.</param>
        /// <param name="recursive">False, if compiler has to search files in top directory only, otherwise true.</param>
        /// <returns>Array of scripts file names, found in provided directory.</returns>
        private static string[] GetScripts( string directory, ScriptFileType type, bool recursive )
        {
            string path = m_ScriptsBaseDirectory;

            if ( directory != null )
                path = Path.Combine(path, directory);

            List<string> files = new List<string>();

            if ( Directory.Exists(path) )
                files.AddRange(Directory.GetFiles(path, "*." + type.ToString(), recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

            return files.ToArray();
        }
    }
}
