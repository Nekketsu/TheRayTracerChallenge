# The Ray Tracer Challenge
This is my implementation of [Jamis Buck](https://github.com/jamis)'s amazing book [The Ray Tracer Challenge](https://pragprog.com/titles/jbtracer) using C#.

## Demo :rocket:
Enjoy the demo here: [TheRayTracerChallenge](https://nekketsu.github.io/TheRayTracerChallenge).

## Goals:
- [x] Implement the book as a C# console application.
- [x] Implement the book as a [Blazor WebAssembly](https://blazor.net) PWA application.
- [x] Share as much code as possible between the C# console application and the [Blazor WebAssembly](https://blazor.net) application.
- [x] Host the [Blazor WebAssembly](https://blazor.net) in [GitHub Pages](https://pages.github.com): [TheRayTracerChallenge](https://nekketsu.github.io/TheRayTracerChallenge).

## Structure
The source code is in the `src` folder, and the demo, [TheRayTracerChallenge](https://nekketsu.github.io/TheRayTracerChallenge), is in the `docs` folder as required by [GitHub Pages](https://pages.github.com).

## Solution
The solution, as described above, is in the `src` folder, and it contains the following important items:
- `RayTracer` project: It is the most important piece of code as it implements the data structures and algorithms described in the book. It is shared between the C# console applications and [Blazor WebAssembly](https://blazor.net) application.
- `RayTracer.Tests` project: It implements all the unit tests described in the book.
- `RayTracer.Demos` project: It implements, as a class library, all the demos of the solution. It makes use of the `RayTracer` project. It is shared between the C# console applications and [Blazor WebAssembly](https://blazor.net) application.
- `RayTracer.Demos.Tests` projects: It implements some unit tests that make sure that some needed constraints are followed.
- `Demos` folder: Contains several projects implementing the `RayTracer.Demos` demos as C# console applications. These demos make use of the `RayTracer` and `RayTracer.Demos` projects.
- `RayTracer.Demos.Browser`: It is a C# console application used as a container to execute all the demos. It makes use of `RayTracer` and `RayTracer.Demos` projects.
- `RayTracer.Blazor` Project: It is the [Blazor WebAssembly](https://blazor.net) implementation of the demos in `RayTracer.Demos` project. It makes use of `RayTracer` and `RayTracer.Demos` projects.
- `Mazes.Services` Project: It implements some services and helper methods used by other projects.
- `Mazes.Services.Test`: It is a C# console applications used as a container of all the demos. It allows us to select and execute any demo.

## Final thoughts
It has been an awesome experience to share code between the C# the console applications and the [Blazor WebAssembly](https://blazor.net) application, making it very straightforward to convert to a web application the implementation of the book with very little effort.

Please, feel free to send issues or pull requests. Any feedback is always welcome!

## Examples
<table>
  <tr>
    <td><img src="images/01-Projectile.png" alt="01-Projectile"></td>
    <td><img src="images/02-Clock.png" alt="02-Clock"></td>
    <td><img src="images/03-Spheres.png" alt="03-Spheres"></td>
    <td><img src="images/04-Spheres3D.png" alt="04-Spheres3D"></td>
  </tr>
  <tr>
    <td><img src="images/05-Scenes.png" alt="05-Scenes"></td>
    <td><img src="images/06-Shadows.png" alt="06-Shadows"></td>
    <td><img src="images/07-Planes.png" alt="07-Planes"></td>
    <td><img src="images/08-Patterns.png" alt="08-Patterns"></td>
  </tr>
  <tr>
    <td><img src="images/09-Reflections.png" alt="09-Reflections"></td>
    <td><img src="images/10-Refractions.png" alt="10-Refractions"></td>
    <td><img src="images/11-Fresnel.png" alt="11-Fresnel"></td>
    <td><img src="images/12-Cubes.png" alt="12-Cubes"></td>
  </tr>
  <tr>
    <td><img src="images/13-Cylinders.png" alt="13-Cylinders"></td>
    <td><img src="images/14-Cones.png" alt="14-Cones"></td>
    <td><img src="images/15-Groups.png" alt="15-Groups"></td>
    <td><img src="images/16-Triangles.png" alt="16-Triangles"></td>
  </tr>
  <tr>
    <td><img src="images/17-CSG.png" alt="17-CSG"></td>
    <td><img src="images/18-Cover.png" alt="18-Cover"></td>
    <td><img src="images/TestResults.png" alt="TestResults"></td>
    <td><img src="images/Web.png" alt="Web"></td>
  </tr>
</table>
