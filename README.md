Compilador desarrollado en el visual studio code e interfaz implementada en windows forms.

Se trata de un proyecto llamado pixel Wall-E, al cual se le introducen comandos y se deben ejecutar 
sobre un canvas de tamaño previamente escogido. Este proyecto cuenta con varias clases, organizadas
en carpetas de AST, que contiene todas las clases de funciones, métodos y expresiones. 

Dentro de las expresiones se definen las aritméticas, booleanas y de comparación. En la carpeta
statement se definen todos los métodos y funciones que debe tener reservado el lenguaje. 

De las carpetas más importantes está el Lexer, donde se hace todo el análisis sintáctico y se guarda
en una lista de Tokens, todos los que resulten válidos.

Otras clases importantes son las ProgramNode y Canvas. La primera es la encargada de desarrollar 
toda la lógica del compilador, la segunda se encarga de hacer todos los cambios precisos en el canvas.

Otra clase es el Parser que se encarga de parsear todas las declaraciones y expresiones. Para las
expresiones se utilizó el descenso recursivo para garantizar que se respete el orden operacional y las
operaciones entre paréntesis.

Requisitos Técnicos:
Plataforma: Windows

IDE: Visual Studio

Tecnología: Windows Forms (.NET Framework)

Configuración del Entorno de Desarrollo
Instalación de Visual Studio
Descarga Visual Studio Community (gratuito):
[Descargar Visual Studio](https://visualstudio.microsoft.com/es/downloads/)

Durante la instalación, selecciona:

Desktop development with C# (para Windows Forms)

.NET desktop development

Extensiones Recomendadas
Editor Enhancements:

Productivity Power Tools https://marketplace.visualstudio.com/items?itemName=VisualStudioPlatformTeam.ProductivityPowerPack2022

VSColorOutput https://marketplace.visualstudio.com/items?itemName=MikeWard-AnnArbor.VSColorOutput

Windows Forms Designer:

Asegúrate de habilitar el diseñador en:
Herramientas > Opciones > Diseñador de Windows Forms
