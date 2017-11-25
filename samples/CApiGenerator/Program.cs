using System;
using NClang;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CApiGenerator
{
	/*
	 * How to populate C API from C++ library
	 * 
	 * - For each C++ class:
	 *   - for each constructor:
	 *     - generate `{ns}_{type}_new ({args})` function.
	 *       - for constructor-less type, just create `{ns}_{type}_new ()` function.
	 *       - these functions return a pointer to the "implementable" derived class explained below.
	 *   - for each non-pure-virtual function:
	 *     - generate `(default_{ns}_{type}_{func}_with_{args}) ({type}* instance, {args})` function, to invoke this C++ function. 
	 *   - for each method:
	 *     - generate `{ns}_{type}_{func}_with_{args} ({type}* instance, {args})` function, and implement it by invoking `on_foobar()` function.
	 *   - generate an "implementable" derived class
	 *     - for each *virtual* method:
	 *       - generate `(on_{ns}_{type}_{func}_with_{args}) ({type}* instance, {args})` function pointer field
	 *         - implement the method, which is to run `on_{ns}_{type}_{func}_with_{args}) ()` function pointer.
	 *       - for non-pure virtual functions, assign `default_foobar()` function to `on_foobar()` function pointer.
	 *   - generate a "delegate invoker" derived class
	 *     FIXME: do we really neet this? The returned object cannot override methods anyways.
	 *     - it can be instantiated from within the generated C API implementation. Not further inheritable.
	 *     - it has one and only constructor, and it takes a pointer to an instance of the class.
	 *     - each public field has a getter function and optionally a setter function.
	 *     - each public method has a wrapper method that invokes the delegated method.
	 *   - Every use of non-pointer non-primitives should change to a use of pointer.
	 * 
	 * Concerns:
	 * - generating function names as "_with_foo_bar" is likely tricky.
	 * - Exception handling.
	 * - What if C++ library returns a new concrete instance and we are supposed to use it?
	 *   - We cannot assume that our instance is of our own generated class.
	 * - Templates.
	 */
	class Driver
	{
		public static void Main (string [] args)
		{
			new Driver ().Run (args);
		}

		void Run (string [] args)
		{
			var idx = ClangService.CreateIndex ();
			var tus = new List<ClangTranslationUnit> ();
			TextWriter output = Console.Out;
			var opts = new CApiGeneratorOptions ();

			// We are going to parse C++ sources.
			opts.ClangArgs.Add ("-x");
			opts.ClangArgs.Add ("c++");
			opts.ClangArgs.Add ("-std=c++1y");

			foreach (var arg in args) {
				if (arg == "--help" || arg == "-?") {
					Console.Error.WriteLine ($"[USAGE] {GetType ().Assembly.GetName ().CodeBase} [options] [inputs]");
					Console.Error.WriteLine (@"options:
	--out:[filename]	output source file name.
	--lib:[library]		library name specified on [DllImport].
	--match:[regex]		process only matching files to [regex].
	--arg:[namespace]	compiler arguments to parse the sources.");
					return;
				} else if (arg.StartsWith ("--out:", StringComparison.Ordinal))
					output = File.CreateText (arg.Substring (6));
				else if (arg.StartsWith ("--lib:", StringComparison.Ordinal))
					opts.LibraryName = arg.Substring (6);
				else if (arg.StartsWith ("--arg:", StringComparison.Ordinal))
					opts.ClangArgs.Add (arg.Substring (6));
				else if (arg == "--only-explicit")
					opts.OnlyExplicit = true;
				else if (arg.StartsWith ("--match:", StringComparison.Ordinal))
					opts.FileMatches.Add (new Regex (arg.Substring (8)));
				else if (arg.Contains (Path.DirectorySeparatorChar) || arg.Contains (Path.AltDirectorySeparatorChar))
					foreach (var file in Directory.GetFiles (Path.GetDirectoryName (arg), Path.GetFileName (arg)))
						opts.Sources.Add (file);
				else
					opts.Sources.Add (arg);
			}


			foreach (var source in opts.Sources) {
				if (!File.Exists (source))
					throw new ArgumentException ("File not found: " + source);
				tus.Add (idx.ParseTranslationUnit (source, opts.ClangArgs.ToArray (), null, TranslationUnitFlags.None));
			}

			var members = new HeaderParser ().Run (opts, tus);
			new CCodeWriter ().Write (output, members, opts);
			output.Close ();
		}
	}

	class CApiGeneratorOptions
	{
		public string LibraryName;
		public List<string> ClangArgs = new List<string> ();
		public bool OnlyExplicit;
		public List<string> Sources = new List<string> ();
		public List<Regex> FileMatches = new List<Regex> ();

		public virtual bool ShouldGenerateCodeFor (NamedConstruct obj)
		{
			return ShouldParse (obj.SourceFileName);
		}

		public virtual bool ShouldParse (string filename)
		{
			if (OnlyExplicit)
				return Sources.Contains (filename);
			else
				return !FileMatches.Any () || FileMatches.Any (fm => fm.IsMatch (filename));
		}
	}
}
