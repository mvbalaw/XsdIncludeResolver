XSD Include Resolver
===
### Description

Microsoft's xsd.exe can be used to generate class files from XSD files.
The problem is it doesn't try to resolve include files and there is no
switch to make it do so.  This tool attempts to resolve the include
references in the XSDs you list on the command line. If successful it 
builds a parameter file for use with Microsoft's xsd.exe.  

### How To Build:

1. open src\XsdIncludeResolver.sln with Visual Studio and build the solution

### Usage:

1. open a command window
2. cd to the base directory under which all the included XSDs can be found.
3. specify the namespace and top level XSDs for which you want to generate classes. e.g.

XsdIncludeResolver.exe  your.output.namespace xsdfile1.xsd [additional xsd file names]

this creates parameters.xml in the current directory.

If all necessary include files are found you can then run xsd.exe with:

xsd.exe /c /out:your_output_directory /p:parameters.xml

the class files for all the objects defined by XSDs in parameters.xml will be generated to your_output_directory.

### License

[MIT License][mitlicense]

This project is part of [MVBA's Open Source Projects][MvbaLawGithub].

If you have questions or comments about this project, please contact us at <mailto:opensource@mvbalaw.com>

[MvbaLawGithub]: http://mvbalaw.github.io/
[mitlicense]: http://www.opensource.org/licenses/mit-license.php
