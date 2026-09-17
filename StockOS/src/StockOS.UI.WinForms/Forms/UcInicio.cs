using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockOS.Domain.Entities;
using StockOS.Domain.Enums;

namespace StockOS.UI.WinForms.Forms
{
    public partial class UcInicio : UserControl
    {
        private Empleado? _usuarioActual;
        private FlowLayoutPanel _flpContenedor;

        public UcInicio()
        {
            InitializeComponent();
            
            _flpContenedor = new FlowLayoutPanel();
            _flpContenedor.Dock = DockStyle.Fill;
            _flpContenedor.AutoScroll = true;
            _flpContenedor.Padding = new Padding(40);
            _flpContenedor.FlowDirection = FlowDirection.TopDown;
            _flpContenedor.WrapContents = false;
            
            this.Controls.Add(_flpContenedor);
        }

        public void SetUsuario(Empleado usuario)
        {
            _usuarioActual = usuario;
            CargarContenido();
        }

        private void CargarContenido()
        {
            _flpContenedor.Controls.Clear();
            
            if (_usuarioActual == null) return;
            
            int idRol = _usuarioActual.IdRol;
            
            if (idRol == (int)RolUsuario.Gerente || idRol == (int)RolUsuario.EncargadoDeposito || idRol == (int)RolUsuario.Repositor || idRol == (int)RolUsuario.Cajero)
            {
                AgregarSeccion("Inventario", "Gestión de productos y control de stock.\nSubsecciones: Lista de Productos, Nuevo Producto e Ingreso de Stock.");
            }
            
            if (idRol == (int)RolUsuario.Gerente || idRol == (int)RolUsuario.Cajero)
            {
                AgregarSeccion("Ventas", "Punto de venta y armado de tickets de productos.");
            }
            
            if (idRol == (int)RolUsuario.Gerente)
            {
                AgregarSeccion("Reportes", "");
                AgregarSeccion("Usuarios", "Administración de cuentas de empleados y asignación de roles.\nSubsecciones: Listar Usuarios y Registrar Usuarios.");
                AgregarSeccion("Config.", "");
            }
        }
        
        private void AgregarSeccion(string titulo, string descripcion)
        {
            var pnl = new RoundedPanel();
            pnl.BackColor = Color.FromArgb(51, 65, 85); // Cuadro con sombreado gris
            pnl.BorderRadius = 15;
            pnl.AutoSize = true;
            pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnl.Margin = new Padding(0, 0, 0, 25);
            pnl.MinimumSize = new Size(800, 0);
            // El padding asegura el margen interno abajo y a la derecha
            pnl.Padding = new Padding(0, 0, 20, 20); 
            
            var lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold | FontStyle.Underline);
            lblTitulo.ForeColor = Color.White; 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            
            pnl.Controls.Add(lblTitulo);
            
            if (!string.IsNullOrEmpty(descripcion))
            {
                var lblDesc = new Label();
                lblDesc.Text = descripcion;
                lblDesc.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                lblDesc.ForeColor = Color.LightGray;
                lblDesc.AutoSize = true;
                lblDesc.MaximumSize = new Size(760, 0);
                lblDesc.Location = new Point(20, 55); 
                
                pnl.Controls.Add(lblDesc);
            }
            
            _flpContenedor.Controls.Add(pnl);
        }
    }
}

