# Estándares de Codificacion: Avalúo
## Indice

 1. **Estilo de Codificación**
 2. **Arquitectura Seleccionada**
 3. **Estructura de carpetas**
 4. **Dependencias, Paquetes y Librerias**
 5. **Estrategia de control de versiones**
 6. **Como integrar tus cambios al repositorio**





## 1. 	Estilo de Codificación
 

### 1.1.	Convenciones de nomenclatura

  

*  **Clases y métodos:** PascalCase.
	```csharp
	Ejemplo: `public class UserService` y `public void GetUserById()`.
	```
*  **Variables y parámetros:** camelCase.
	```csharp
	Ejemplo: `string userName`.
	```
*  **Constantes y Variables publicas:** UPPER_CASE con guiones bajos.
	```csharp
	Ejemplo: `public const int MAX_RETRIES = 3;`.
	```
*  **Interfaces:** Prefijo `I` y se escribe el nombre de la clase que la implementa. En caso de mas de una implementacion, usar un nombre a fin.

	 Ejemplo: 
	```csharp
	// Usamos dos bases de datos
	
		public class MongoDbRepository
		public class SqlRepository
		
	// Como hay mas de una implementacion de la base de datos pues la interfaz 
	// que implementarian se llamaria algo parecido 
	
		public interface IRepository
	```
### 1.2. Comentar el código
		
   **Comentarios en línea**

 -   Explican una línea o fragmento específico de código.
 -   Usa comentarios en línea para aclarar lógica compleja o excepciones al estándar esperado.
 
```csharp
int result = CalculateDiscount(price); // Descuento basado en reglas del cliente.
```
 **Comentarios en bloque**

 -   Usados para describir secciones más grandes o explicaciones detalladas.
 -   Formato:

```csharp
/* Este bloque inicializa los valores predeterminados y 
valida que las configuraciones necesarias estén disponibles. 
*/ 
ValidateConfiguration();
```
**Reglas Generales**

 - **Evita comentar lo obvio**: No comentes líneas de código que se expliquen por sí mismas.
 ```csharp
		// Mala práctica
		int count = users.Count; // Asigna el número de usuarios a la variable count.

		// Buena práctica (sin comentario)
		int count = users.Count;
```
 -    **Actualiza los comentarios**: Siempre sincroniza los comentarios con los cambios en el código.
-   **Lenguaje claro**: Usa un lenguaje claro y conciso. Evita jerga innecesaria.
-   **Consistencia en el idioma**: Español y solo usar ingles para referenciar partes del código.

**Comentarios para Controladores**

 - Documenta las acciones con etiquetas XML para Swagger.
```csharp
	    /// <summary>
	    /// Recupera la lista de usuarios.
	    /// </summary>
	    /// <returns>Una lista de usuarios activos.</returns>
	    [HttpGet]
	    public IEnumerable<User> GetAllUsers() { ... }
```
Buenas Prácticas

 - **Usa comentarios TODO**: Marca áreas que necesitan trabajo o revisión.
 
		 // TODO: Implementar lógica para manejar usuarios inactivos.
 - **Usa comentarios FIX**: Documenta problemas o bugs conocidos
 
		// FIX: Resolver problema con usuarios duplicados en el listado.

## 2. Arquitectura Seleccionada

### 2.1. Descripción general
La arquitectura de capas fue seleccionada por su claridad y separación de responsabilidades. Esto facilita el mantenimiento, la escalabilidad y el entendimiento del sistema por parte del equipo de desarrollo.

### 2.2. Componentes Principales
 **Capas del Sistema**
1. **Capa de Presentación (Presentation Layer)**  
   - Maneja la interfaz de usuario. En este caso son los controladores de la API REST ya que interactura con los cliente externos.
   - **NO** contiene lógica de negocio ni acceso a datos.  

2. **Capa de Aplicación (Application Layer)**  
   - Coordina las operaciones entre las diferentes capas.  
   - Contiene casos de uso específicos del dominio del sistema.  

3. **Capa de Negocio (Business Layer o Domain Layer)**  
   - Implementa las reglas de negocio de la aplicación.  
   - Contiene entidades y servicios que representan conceptos del dominio.

4. **Capa de Datos y Persistencia (Data Access Layer)**  
   - Gestiona el acceso y la persistencia de datos en bases de datos.  
   - Encapsula la lógica de acceso a datos para que el resto del sistema sea independiente de la tecnología de persistencia.  

### 2.3. Principios de Diseño

- **Separación de responsabilidades**: Cada capa debe enfocarse en un propósito único.
- **Independencia entre capas**: Las capas deben depender de contratos (interfaces) en lugar de implementaciones concretas.
- **Reutilización**: Lógica de negocio y acceso a datos desacoplados para facilitar la reutilización en otros proyectos.

### 2.4. Diagrama de Referencia
![Layered Architecture – @hgraca](https://herbertograca.com/wp-content/uploads/2017/07/2010s-layered-architecture.png?w=1100)


### 3. Estructura de Carpetas
Las carpetas y estructura del proyecto será la siguiente:

```sh
    API
      ├── Presentation
      |     └── Controllers  
      ├── Application
      |     ├── Services
      |     |    	└── PeoService  //Por ejemplo    
      |     └── Dtos
      ├── Domain
	  │     ├── Entities
	  |     └── Abstractions
      └── Infrastructure
		    ├── Persistence
		    ├── Authentication
		    └── Integrations
```		    
		    
    
    
   **Aquí el desglose**:

 - **API**: Es la raiz del proyecto. Es una plantilla Web API de .NET Core en donde se crearan las capas.
 
 **1er nivel**:
 - **Presentation**: Es la capa que almacena todo lo que interactua directamente conel cliente externo, en este caso serian los endpoints.
 - **Application**: La capa de aplicacion se encarga de manejar los servicios y todos los procesos que tiene logica de casos de uso de la aplicacion usando las entidades del negocio.
 - **Domain**: Es la capa de Dominio donde se almacenaran todas las entidades y value objects segun el Domain Driven Design. Esta almacena todo lo relacionado con las reglas del negocio.
 - **Infrastructure**: Aqui se maneja la persistencia de los datos y funciones como la autenticacion de los usuarios, integraciones con otras tecnologias y envio de emails. Basicamente todo lo técnico que se maneja por debajo.

            
**2do nivel**:
- **Controllers**: En esta carpeta se almacenan los controladores de las solicitudes http (los endpoints). Ejemplo : StudentController, TeacherController

- **Services**: Aquí se almacenan las carpetas de cada servicio que se usa en la aplicacion.

- **Dtos**: En esta carpeta se guardaran las carpetas de los Data Transfer Objects de cada entidad.
- **Entities**: Aquí se guardan las entidades y los value objects de la capa de dominio. Cada entidad tendra su carpeta con su entidad y sus value objects si es un aggregate.
- **Abstractions**: Aqui se guardan abstracciones de las entidades que nos permitan agregarle ciertos comportamientos.
- **Persistence**: Aqui se almacena todo lo relacionado a las transacciones con la base de datos.
- **Authentication**: Aqui se almacenan lo relacionado con los tokens de ingreso.
- **Integrations**: Aqui se almacenan los servicios que se integran con las diferentes tecnologias o sistemas externos.


### 4. Dependencias, Paquetes y Librerias

 - EntityFramework Core:
 - Simple Injector:

### 5. Estrategia de control de versiones
En el repositorio de Github tendremos:

 - **Rama main**: Aquí estará el código de producción. Solo se puede modificar esta rama con un pull request desde dev y que esté autorizado por el lider de equipo.
 
 - **Rama dev**: Aquí los desarrolladores meteran su codigo en progreso via pull requests desde su rama feature/ o fix/.
 - **Rama feature/**: Esta rama sirve para integrar una nueva funcionalidad al codigo.
 - **Rama fix/**: Esta rama sirve para arreglar algun problema con el código.

Flujo de cambios:
				 
	feature o fix  ---> dev ---> main

### 6. Como integrar tus cambios al repositorio

**6.1. Crear una nueva rama local para el feature**

Primero, asegúrate de estar en la rama `main` y de que esté actualizada:

```sh
git checkout main
git pull origin main
```

Luego, crea y cambia a una nueva rama para tu feature:

```sh
git checkout -b feature/nombre-del-feature
```

**6.2. Realizar los cambios y hacer commits**

Realiza los cambios necesarios para tu feature. Luego, agrega y haz commit de tus cambios:

```sh
git add .
git commit -m "Descripción de los cambios para el feature"
```

**6.3. Subir la rama feature a GitHub**

Empuja la rama feature al repositorio remoto en GitHub:

```sh
git push origin feature/nombre-del-feature
```

**6.4. Crear un Pull Request en GitHub**

Ve a GitHub y navega a tu repositorio. Deberías ver una notificación que te sugiere crear un Pull Request (PR) para tu nueva rama. Haz clic en esa notificación y sigue las instrucciones para crear un PR para integrar tu rama `feature/nombre-del-feature` en `main`.

**6.5. Revisar y fusionar el Pull Request**

Revisa los cambios en el PR y asegúrate de que todo esté correcto. Si tienes colaboradores, pueden revisar el PR y aprobarlo. Una vez que todo esté listo, fusiona el PR en la rama `main`.

**6.6. Actualizar la rama `main` local**

Después de fusionar el PR, es una buena práctica actualizar tu rama `main` local:

```sh
git checkout main
git pull origin main
```

 **Resumen de los comandos**

```sh
# Cambiar a la rama main y actualizarla
git checkout main
git pull origin main

# Crear y cambiar a una nueva rama feature
git checkout -b feature/nombre-del-feature

# Realizar cambios, agregar y hacer commit
git add .
git commit -m "Descripción de los cambios para el feature"

# Subir la rama feature a GitHub
git push origin feature/nombre-del-feature

# (Crear y fusionar el Pull Request en GitHub)

# Cambiar a la rama main y actualizarla después de fusionar el PR
git checkout main
git pull origin main
```

Siguiendo estos pasos, podrás gestionar tus ramas y cambios de manera efectiva, integrando tus nuevas características en la rama principal de tu proyecto en GitHub.

**Guia para unir cambios simultaneos entre compañeros**

Para colaborar eficazmente en un proyecto con Git y GitHub y evitar conflictos al trabajar en el mismo archivo, puedes seguir estos pasos:

1. **Crear Ramas Separadas:**
   Tú y tu amigo deben trabajar en ramas separadas para sus cambios respectivos. Por ejemplo, tu amigo puede trabajar en una rama llamada `feature/post-endpoint`, y tú en una rama llamada `feature/get-endpoint`.

2. **Comitear Cambios Localmente:**
   Ambos deben hacer commits de sus cambios localmente a sus respectivas ramas.

3. **Push Ramas al Remoto:**
   Ambos deben hacer push de sus ramas al repositorio remoto en GitHub.

4. **Pull Request (PR):**
   - Tu amigo crea un pull request desde la rama `feature/post-endpoint` hacia la rama `main` (o la rama de desarrollo principal).
   - Tú haces lo mismo con tu rama `feature/get-endpoint` hacia la rama `main`.

5. **Revisar y Mergear PR de tu Amigo:**
   - El PR de tu amigo debe ser revisado y mergeado primero en la rama principal (`main`).
   - Una vez mergeado, su código estará en `main`.

6. **Actualizar tu Rama:**
   - Una vez que el PR de tu amigo ha sido mergeado, debes actualizar tu rama con los últimos cambios de `main` para evitar conflictos. Puedes hacer esto haciendo un pull de `main` en tu rama:
     ```bash
     git checkout feature/get-endpoint
     git pull origin main
     ```
     Si hay conflictos, Git te informará y podrás resolverlos manualmente.

7. **Resolver Conflictos (si los hay):**
   - Si hay conflictos, resuélvelos en tu editor de texto o IDE y luego haz un commit de los cambios resultantes.

8. **Push tus Cambios Actualizados:**
   - Una vez resueltos los conflictos (si los hubo), haz push de tu rama actualizada:
     ```bash
     git push origin feature/get-endpoint
     ```

9. **Crear PR para tu Rama:**
   - Crea un pull request desde tu rama `feature/get-endpoint` hacia `main` y sigue el mismo proceso de revisión y merge.

**Ejemplo de Comandos**

1. Tu amigo crea su rama y trabaja en el endpoint POST:
   ```bash
   git checkout -b feature/post-endpoint
   # Realiza cambios
   git add .
   git commit -m "Add POST endpoint"
   git push origin feature/post-endpoint
   ```

2. Tú creas tu rama y trabajas en el endpoint GET:
   ```bash
   git checkout -b feature/get-endpoint
   # Realiza cambios
   git add .
   git commit -m "Add GET endpoint"
   ```

3. Tu amigo crea un pull request y lo mergea en `main`.

4. Tú actualizas tu rama con los cambios de `main`:
   ```bash
   git checkout feature/get-endpoint
   git pull origin main
   ```

5. Resuelves cualquier conflicto, haces commit y push:
   ```bash
   git add .
   git commit -m "Resolve merge conflicts"
   git push origin feature/get-endpoint
   ```

6. Finalmente, creas un pull request desde tu rama `feature/get-endpoint` hacia `main`.


