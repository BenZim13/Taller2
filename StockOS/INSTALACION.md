\# Guía de Instalación - StockOS



Esta guía detalla los pasos necesarios para instalar y ejecutar el sistema StockOS en un entorno de desarrollo nuevo.



\## 1. Requisitos Previos

\* Visual Studio 2022 o superior.

\* .NET 8 SDK.

\* SQL Server (LocalDB, Express o Developer).

\* SQL Server Management Studio (SSMS) o Azure Data Studio.



\## 2. Configuración de la Base de Datos

1\. Abrir SSMS y conectarse al servidor de base de datos local.

2\. Crear una base de datos vacía llamada `StockOS`:

&#x20;  ```sql

&#x20;  CREATE DATABASE StockOS;

3\. Ingresar a StockOS\\DatabaseScripts y ejecutar el script 00\_CreacionCompleta.sql

