using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByDesigners.App
{
    public class FormPrincipal : Form
    {
        private Panel panelLateral = null!;
        private Panel panelContenido = null!;

        private Button btnClientes = null!;
        private Button btnMuebles = null!;
        private Button btnCotizaciones = null!;
        private Button btnPagos = null!;
        private Label lblTitulo = null!;

        public FormPrincipal()
        {
            InicializarComponentes();
            // Cargar por defecto la pantalla de clientes al iniciar
            AbrirFormularioEnPanel(new FormCliente());
        }

        private void InicializarComponentes()
        {
            this.Text = "ByDesigners System - FirstSoftwareSolution";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 550);

            // 1. Panel Lateral Izquierdo (Menú de Navegación)
            panelLateral = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(35, 45, 55)
            };

            // Título/Logo en Panel Lateral
            lblTitulo = new Label()
            {
                Text = "🛋️ ByDesigners",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Botón Clientes
            btnClientes = CrearBotonMenu("📋 Clientes", 80);
            btnClientes.Click += (s, e) => AbrirFormularioEnPanel(new FormCliente());

            // Botón Muebles e Ítems
            btnMuebles = CrearBotonMenu("🪑 Muebles & Servicios", 135);
            btnMuebles.Click += (s, e) => AbrirFormularioEnPanel(new FormMueble());

            // Botón Cotizaciones
            btnCotizaciones = CrearBotonMenu("📄 Cotizaciones", 190);
            btnCotizaciones.Click += (s, e) => AbrirFormularioEnPanel(new FormCotizacion());

            // Botón Pagos
            btnPagos = CrearBotonMenu("💳 Registro de Pagos", 245);
            btnPagos.Click += (s, e) => AbrirFormularioEnPanel(new FormPago());

            panelLateral.Controls.Add(btnPagos);
            panelLateral.Controls.Add(btnCotizaciones);
            panelLateral.Controls.Add(btnMuebles);
            panelLateral.Controls.Add(btnClientes);
            panelLateral.Controls.Add(lblTitulo);

            // 2. Panel Contenedor Principal (Donde se muestran los formularios)
            panelContenido = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // Agregar Paneles a la Ventana Principal
            this.Controls.Add(panelContenido);
            this.Controls.Add(panelLateral);
        }

        private Button CrearBotonMenu(string texto, int top)
        {
            Button btn = new Button()
            {
                Text = texto,
                Top = top,
                Left = 10,
                Width = 200,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 65, 80);
            return btn;
        }

        private void AbrirFormularioEnPanel(Form formHijo)
        {
            if (panelContenido.Controls.Count > 0)
                panelContenido.Controls.RemoveAt(0);

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(formHijo);
            panelContenido.Tag = formHijo;
            formHijo.Show();
        }
    }
}