using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ByDesigners.App
{
    public class FormMueble : Form
    {
        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;
        private TextBox txtPrecioBase = null!;
        private TextBox txtStock = null!;
        private ComboBox cmbTipoItem = null!;

        // Campos Mueble a Medida
        private Label lblMaterial = null!;
        private TextBox txtMaterial = null!;
        private Label lblDimensiones = null!;
        private TextBox txtAlto = null!;
        private TextBox txtAncho = null!;
        private TextBox txtProfundidad = null!;
        private Label lblColor = null!;
        private TextBox txtColor = null!;

        // Campos Servicio Mantenimiento
        private Label lblCategoria = null!;
        private TextBox txtCategoria = null!;
        private Label lblUnidadCobro = null!;
        private TextBox txtUnidadCobro = null!;

        private Button btnGuardar = null!;

        public FormMueble()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Registro de Ítems";
            this.Size = new Size(820, 580);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTituloModulo = new Label()
            {
                Text = "Catálogo de Muebles a Medida y Servicios",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                Left = 40,
                Top = 25,
                Width = 500,
                Height = 35
            };

            int top = 80;

            Label lblNombre = new Label() { Text = "Nombre del Ítem:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtNombre = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 40;

            Label lblDesc = new Label() { Text = "Descripción:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtDescripcion = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 40;

            Label lblPrecio = new Label() { Text = "Precio Base ($):", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtPrecioBase = new TextBox() { Left = 190, Top = top, Width = 160, Font = new Font("Segoe UI", 10) };

            Label lblStock = new Label() { Text = "Stock Inicial:", Left = 360, Top = top, Width = 80, Font = new Font("Segoe UI", 10) };
            txtStock = new TextBox() { Left = 440, Top = top, Width = 90, Text = "1", Font = new Font("Segoe UI", 10) };
            top += 40;

            Label lblTipo = new Label() { Text = "Tipo de Ítem:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            cmbTipoItem = new ComboBox() { Left = 190, Top = top, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbTipoItem.Items.AddRange(new string[] { "Mueble", "Servicio" });
            cmbTipoItem.SelectedIndex = 0;
            cmbTipoItem.SelectedIndexChanged += CmbTipoItem_SelectedIndexChanged;
            top += 45;

            // --- Sección Mueble ---
            lblMaterial = new Label() { Text = "Material Principal:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtMaterial = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 40;

            lblDimensiones = new Label() { Text = "Alto x Ancho x Prof (m):", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 9) };
            txtAlto = new TextBox() { Left = 190, Top = top, Width = 100, Text = "1.0", Font = new Font("Segoe UI", 10) };
            txtAncho = new TextBox() { Left = 300, Top = top, Width = 100, Text = "1.0", Font = new Font("Segoe UI", 10) };
            txtProfundidad = new TextBox() { Left = 410, Top = top, Width = 120, Text = "0.5", Font = new Font("Segoe UI", 10) };
            top += 40;

            lblColor = new Label() { Text = "Color / Acabado:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtColor = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };

            // --- Sección Servicio ---
            lblCategoria = new Label() { Text = "Categoría Servicio:", Left = 40, Top = top, Width = 140, Visible = false, Font = new Font("Segoe UI", 10) };
            txtCategoria = new TextBox() { Left = 190, Top = top, Width = 340, Visible = false, Font = new Font("Segoe UI", 10) };

            lblUnidadCobro = new Label() { Text = "Unidad de Cobro:", Left = 40, Top = top + 40, Width = 140, Visible = false, Font = new Font("Segoe UI", 10) };
            txtUnidadCobro = new TextBox() { Left = 190, Top = top + 40, Width = 340, Visible = false, Font = new Font("Segoe UI", 10) };

            top += 55;

            btnGuardar = new Button()
            {
                Text = "💾 Guardar en Catálogo",
                Left = 190,
                Top = top,
                Width = 220,
                Height = 42,
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            this.Controls.Add(lblTituloModulo);
            this.Controls.Add(lblNombre); this.Controls.Add(txtNombre);
            this.Controls.Add(lblDesc); this.Controls.Add(txtDescripcion);
            this.Controls.Add(lblPrecio); this.Controls.Add(txtPrecioBase);
            this.Controls.Add(lblStock); this.Controls.Add(txtStock);
            this.Controls.Add(lblTipo); this.Controls.Add(cmbTipoItem);

            this.Controls.Add(lblMaterial); this.Controls.Add(txtMaterial);
            this.Controls.Add(lblDimensiones); this.Controls.Add(txtAlto); this.Controls.Add(txtAncho); this.Controls.Add(txtProfundidad);
            this.Controls.Add(lblColor); this.Controls.Add(txtColor);

            this.Controls.Add(lblCategoria); this.Controls.Add(txtCategoria);
            this.Controls.Add(lblUnidadCobro); this.Controls.Add(txtUnidadCobro);

            this.Controls.Add(btnGuardar);
        }

        private void CmbTipoItem_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool esMueble = cmbTipoItem.SelectedItem?.ToString() == "Mueble";

            lblMaterial.Visible = esMueble; txtMaterial.Visible = esMueble;
            lblDimensiones.Visible = esMueble; txtAlto.Visible = esMueble; txtAncho.Visible = esMueble; txtProfundidad.Visible = esMueble;
            lblColor.Visible = esMueble; txtColor.Visible = esMueble;

            lblCategoria.Visible = !esMueble; txtCategoria.Visible = !esMueble;
            lblUnidadCobro.Visible = !esMueble; txtUnidadCobro.Visible = !esMueble;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecioBase.Text))
            {
                MessageBox.Show("Por favor ingresa el nombre y precio base del ítem.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();

                    // 1. Insertar en Tabla Padre: Item_Cotizable
                    string queryPadre = "INSERT INTO Item_Cotizable (nombre_item, descripcion, precio_base, tipo_item, stock, fecha_alta) " +
                                        "VALUES (@nom, @desc, @precio, @tipo, @stock, NOW()); SELECT LAST_INSERT_ID();";

                    MySqlCommand cmdPadre = new MySqlCommand(queryPadre, con);
                    cmdPadre.Parameters.AddWithValue("@nom", txtNombre.Text);
                    cmdPadre.Parameters.AddWithValue("@desc", txtDescripcion.Text);
                    cmdPadre.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecioBase.Text));
                    cmdPadre.Parameters.AddWithValue("@tipo", cmbTipoItem.SelectedItem?.ToString());
                    cmdPadre.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));

                    int idGenerado = Convert.ToInt32(cmdPadre.ExecuteScalar());

                    // 2. Insertar en Tabla Hija según el tipo
                    if (cmbTipoItem.SelectedItem?.ToString() == "Mueble")
                    {
                        string queryHija = "INSERT INTO Mueble_Medida (id_item, material_principal, alto, ancho, profundidad, color_acabado) " +
                                            "VALUES (@id, @mat, @alto, @ancho, @prof, @color);";
                        MySqlCommand cmdHija = new MySqlCommand(queryHija, con);
                        cmdHija.Parameters.AddWithValue("@id", idGenerado);
                        cmdHija.Parameters.AddWithValue("@mat", txtMaterial.Text);
                        cmdHija.Parameters.AddWithValue("@alto", Convert.ToDouble(txtAlto.Text));
                        cmdHija.Parameters.AddWithValue("@ancho", Convert.ToDouble(txtAncho.Text));
                        cmdHija.Parameters.AddWithValue("@prof", Convert.ToDouble(txtProfundidad.Text));
                        cmdHija.Parameters.AddWithValue("@color", txtColor.Text);
                        cmdHija.ExecuteNonQuery();
                    }
                    else
                    {
                        string queryHija = "INSERT INTO Servicio_Mantenimiento (id_item, categoria, unidad_cobro) VALUES (@id, @cat, @unidad);";
                        MySqlCommand cmdHija = new MySqlCommand(queryHija, con);
                        cmdHija.Parameters.AddWithValue("@id", idGenerado);
                        cmdHija.Parameters.AddWithValue("@cat", txtCategoria.Text);
                        cmdHija.Parameters.AddWithValue("@unidad", txtUnidadCobro.Text);
                        cmdHija.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Ítem registrado correctamente en MySQL!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en MySQL: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecioBase.Clear();
            txtMaterial.Clear();
            txtColor.Clear();
            txtCategoria.Clear();
            txtUnidadCobro.Clear();
        }
    }
}