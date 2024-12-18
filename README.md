# Sistema de Gestión de Reservas

Este proyecto es una aplicación web diseñada para gestionar la reserva de salas en Mundo Aventura. Proporciona funcionalidades para crear, editar, eliminar y listar reservas y salas, junto con filtros dinámicos para facilitar la búsqueda.

---

## **Instrucciones para Configurar y Ejecutar el Proyecto**

### **Prerrequisitos**

Antes de comenzar, asegúrate de tener instalados los siguientes componentes:

- **Visual Studio** (2019 o superior) con el paquete de desarrollo web .NET.
- **SQL Server** (Express o superior).
- **SQL Server Management Studio (SSMS)** (opcional, pero recomendado).
- **.NET Framework 4.8** (compatible con la versión del proyecto).

### **Pasos para Configurar**

1. **Clona el repositorio:**
   ```bash
   git clone <https://github.com/LMGP2002/prueba-tecnica-coparques>
   cd <directorio del proyecto>
   ```

2. **Configura la base de datos:**
   - Abre SQL Server Management Studio (SSMS).
   - Ejecuta el archivo `tablas_procedimientos.sql`para crear las tablas y procedimientos almacenados necesarios.
     - Incluye tablas como `Salas` y `Reservas`.
     - Verifica que se hayan creado correctamente las relaciones y claves foráneas.

3. **Actualiza la cadena de conexión:**
   - Ve al archivo `web.config` en el proyecto.
   - Modifica la sección `<connectionStrings>` con los datos de tu servidor SQL Server. Ejemplo:
     ```xml
     <connectionStrings>
       <add name="DefaultConnection" connectionString="Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
     </add>
     ```

4. **Restaura paquetes NuGet:**
   - Abre el proyecto en Visual Studio.
   - Ve al **Administrador de Paquetes NuGet** (Tools > NuGet Package Manager > Manage NuGet Packages for Solution).
   - Restaura los paquetes necesarios si no se descargan automáticamente.

5. **Ejecuta el proyecto:**
   - Presiona `F5` o selecciona **Iniciar Depuración** en Visual Studio.
   - La aplicación se ejecutará en tu navegador predeterminado.

---

## **Inicialización de la Base de Datos**

### **Crear Tablas y Procedimientos Almacenados**

1. Localiza el archivo `tablas_procedimientos.sql`.
2. Abre el archivo en SQL Server Management Studio (SSMS).
3. Selecciona y ejecuta todo el script presionando `F5` o el botón **Ejecutar**.
4. Verifica que las tablas y procedimientos se hayan creado correctamente en tu base de datos.

### **Datos de Ejemplo**
- Puedes utilizar las siguientes instrucciones SQL para insertar datos manualmente:
  ```sql
  INSERT INTO Salas (Nombre, Capacidad, Disponibilidad)
  VALUES ('Sala 1', 20, 1), ('Sala 2', 15, 1);

  INSERT INTO Reservas (SalaID, FechaReserva, Usuario)
  VALUES (1, '2024-12-20', 'Juan Pérez');
  ```


