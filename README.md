# StbVorbisSharp
[![NuGet](https://img.shields.io/nuget/v/StbVorbisSharp.svg)](https://www.nuget.org/packages/StbVorbisSharp/)
[![Build status](https://ci.appveyor.com/api/projects/status/8ixt18rogia0ymiv?svg=true)](https://ci.appveyor.com/project/RomanShapiro/StbVorbisSharp)

C# port of stb_vorbis.c

# Adding Reference
There are two ways of referencing StbVorbisSharp in the project:
1. Through nuget: https://www.nuget.org/packages/StbVorbisSharp/
2. As submodule:
    
    a. `git submodule add https://github.com/StbSharp/StbVorbisSharp.git`
    
    b. Now there are two options:
       
      * Add StbVorbisSharp/src/StbVorbisSharp/StbVorbisSharp.csproj to the solution
       
      * Include *.cs from StbVorbisSharp/src/StbVorbisSharp directly in the project. In this case, it might make sense to add STBSHARP_INTERNAL build compilation symbol to the project, so StbVorbisSharp classes would become internal.

# Usage
See the sample [StbVorbisSharp.MonoGame.Test](https://github.com/StbSharp/StbVorbisSharp/tree/master/samples/StbVorbisSharp.MonoGame.Test)

# License
Public Domain

# Credits
* [stb](https://github.com/nothings/stb)
