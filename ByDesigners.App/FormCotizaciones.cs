using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ByDesigners.App
{
    public class FormCotizacion : Form
    {
        private ComboBox cmbClientes = null!;
        private ComboBox cmbItems = null!;
        private TextBox txtCantidad = null!;
        private TextBox txtPrecioUnitario = null!;
        private Button btnAgregarPartida = null!;

        private DataGridView dgvDetalle = null!;
        private Label lblTotal = null!;
        private Button btnGuardarCotizacion = null!;

        private decimal totalAcumulado = 0m;
        private DataTable tablaDetalle = null!;

        public FormCotizacion()
        {
            InicializarComponentes();
            CargarClientes();
            CargarItems();
        }

        private void InicializarComponentes()
        {
            this.Text = "Generador de Cotizaciones";
            this.Size = new Size(820, 580);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTituloModulo = new Label()
            {
                Text = "Generador de Cotizaciones y Presupuestos",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                Left = 40,
                Top = 20,
                Width = 500,
                Height = 30
            };

            // Selección de Cliente
            Label lblCliente = new Label() { Text = "Cliente:", Left = 40, Top = 65, Width = 80, Font = new Font("Segoe UI", 10) };
            cmbClientes = new ComboBox() { Left = 120, Top = 65, Width = 380, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            // Selección de Ítem
            Label lblItem = new Label() { Text = "Ítem:", Left = 40, Top = 105, Width = 80, Font = new Font("Segoe UI", 10) };
            cmbItems = new ComboBox() { Left = 120, Top = 105, Width = 380, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbItems.SelectedIndexChanged += CmbItems_SelectedIndexChanged;

            // Cantidad y Precio
            Label lblCant = new Label() { Text = "Cantidad:", Left = 40, Top = 145, Width = 80, Font = new Font("Segoe UI", 10) };
            txtCantidad = new TextBox() { Left = 120, Top = 145, Width = 90, Text = "1", Font = new Font("Segoe UI", 10) };

            Label lblPrecio = new Label() { Text = "Precio Unit ($):", Left = 230, Top = 145, Width = 100, Font = new Font("Segoe UI", 10) };
            txtPrecioUnitario = new TextBox() { Left = 330, Top = 145, Width = 100, Font = new Font("Segoe UI", 10) };

            btnAgregarPartida = new Button()
            {
                Text = "➕ Agregar",
                Left = 445,
                Top = 143,
                Width = 110,
                Height = 32,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAgregarPartida.FlatAppearance.BorderSize = 0;
            btnAgregarPartida.Click += BtnAgregarPartida_Click;

            // DataGridView de Detalle
            dgvDetalle = new DataGridView()
            {
                Left = 40,
                Top = 190,
                Width = 720,
                Height = 250,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Estructura de la Tabla Memoria
            tablaDetalle = new DataTable();
            tablaDetalle.Columns.Add("IdItem", typeof(int));
            tablaDetalle.Columns.Add("Descripción", typeof(string));
            tablaDetalle.Columns.Add("Cantidad", typeof(int));
            tablaDetalle.Columns.Add("Precio Unitario", typeof(decimal));
            tablaDetalle.Columns.Add("Subtotal", typeof(decimal));
            dgvDetalle.DataSource = tablaDetalle;

            // Label de Total
            lblTotal = new Label()
            {
                Text = "Total: $0.00",
                Left = 520,
                Top = 450,
                Width = 240,
                Height = 35,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Botón Guardar Cotización
            btnGuardarCotizacion = new Button()
            {
                Text = "💾 Guardar Cotización en MySQL",
                Left = 40,
                Top = 490,
                Width = 260,
                Height = 40,
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardarCotizacion.FlatAppearance.BorderSize = 0;
            btnGuardarCotizacion.Click += BtnGuardarCotizacion_Click;

            this.Controls.Add(lblTituloModulo);
            this.Controls.Add(lblCliente); this.Controls.Add(cmbClientes);
            this.Controls.Add(lblItem); this.Controls.Add(cmbItems);
            this.Controls.Add(lblCant); this.Controls.Add(txtCantidad);
            this.Controls.Add(lblPrecio); this.Controls.Add(txtPrecioUnitario);
            this.Controls.Add(btnAgregarPartida);
            this.Controls.Add(dgvDetalle);
            this.Controls.Add(lblTotal);
            this.Controls.Add(btnGuardarCotizacion);
        }

        private void CargarClientes()
        {
            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_cli, nom_cli FROM Cliente";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbClientes.DataSource = dt;
                    cmbClientes.DisplayMember = "nom_cli";
                    cmbClientes.ValueMember = "id_cli";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarItems()
        {
            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT id_item, nombre_item, precio_base FROM Item_Cotizable";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbItems.DataSource = dt;
                    cmbItems.DisplayMember = "nombre_item";
                    cmbItems.ValueMember = "id_item";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ítems: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbItems_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbItems.SelectedItem is DataRowView row)
            {
                txtPrecioUnitario.Text = Convert.ToDecimal(row["precio_base"]).ToString("F2");
            }
        }

        private void BtnAgregarPartida_Click(object? sender, EventArgs e)
        {
            if (cmbItems.SelectedItem == null || string.IsNullOrWhiteSpace(txtCantidad.Text)) return;

            DataRowView itemSel = (DataRowView)cmbItems.SelectedItem;
            int idItem = Convert.ToInt32(itemSel["id_item"]);
            string nombre = itemSel["nombre_item"].ToString()!;
            int cant = Convert.ToInt32(txtCantidad.Text);
            decimal precio = Convert.ToDecimal(txtPrecioUnitario.Text);
            decimal subtotal = cant * precio;

            tablaDetalle.Rows.Add(idItem, nombre, cant, precio, subtotal);
            totalAcumulado += subtotal;
            lblTotal.Text = $"Total: ${totalAcumulado:N2}";
        }

        private void BtnGuardarCotizacion_Click(object? sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue == null || tablaDetalle.Rows.Count == 0)
            {
                MessageBox.Show("Selecciona un cliente y agrega al menos un ítem a la cotización.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();
                    MySqlTransaction trans = con.BeginTransaction();

                    try
                    {
                        // 1. Guardar Cabecera de Cotización
                        string queryCot = "INSERT INTO Cotizacion (fecha_cot, estado_cot, total_cot, id_cli, id_trab, obs_cot) " +
                                           "VALUES (NOW(), 'Pendiente', @total, @idCli, 1, 'Generada desde sistema local'); SELECT LAST_INSERT_ID();";

                        MySqlCommand cmdCot = new MySqlCommand(queryCot, con, trans);
                        cmdCot.Parameters.AddWithValue("@total", totalAcumulado);
                        cmdCot.Parameters.AddWithValue("@idCli", cmbClientes.SelectedValue);

                        int idCotGenerada = Convert.ToInt32(cmdCot.ExecuteScalar());

                        // 2. Guardar Detalle de Cotización
                        foreach (DataRow fila in tablaDetalle.Rows)
                        {
                            string queryDet = "INSERT INTO Detalle_Cotizacion (id_cot, id_item, cantidad, precio_unit, subtotal) " +
                                              "VALUES (@idCot, @idItem, @cant, @precio, @subtotal);";

                            MySqlCommand cmdDet = new MySqlCommand(queryDet, con, trans);
                            cmdDet.Parameters.AddWithValue("@idCot", idCotGenerada);
                            cmdDet.Parameters.AddWithValue("@idItem", fila["IdItem"]);
                            cmdDet.Parameters.AddWithValue("@cant", fila["Cantidad"]);
                            cmdDet.Parameters.AddWithValue("@precio", fila["Precio Unitario"]);
                            cmdDet.Parameters.AddWithValue("@subtotal", fila["Subtotal"]);
                            cmdDet.ExecuteNonQuery();
                        }

                        trans.Commit();
                        MessageBox.Show($"¡Cotización #{idCotGenerada} registrada exitosamente en MySQL!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Limpiar
                        tablaDetalle.Rows.Clear();
                        totalAcumulado = 0m;
                        lblTotal.Text = "Total: $0.00";
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar cotización: " + ex.Message, "Error MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}