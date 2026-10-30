using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ByDesigners.App
{
    public class FormPago : Form
    {
        private ComboBox cmbCotizaciones = null!;
        private Label lblTotalCotizacion = null!;
        private Label lblSaldoPendiente = null!;

        private TextBox txtMontoPago = null!;
        private ComboBox cmbTipoPago = null!;

        // Campos Transferencia
        private Label lblBanco = null!;
        private TextBox txtBanco = null!;
        private Label lblSpei = null!;
        private TextBox txtSpei = null!;

        // Campos Efectivo
        private Label lblRecibo = null!;
        private TextBox txtRecibo = null!;

        private Button btnRegistrarPago = null!;

        private decimal totalCot = 0m;
        private decimal abonosPrevios = 0m;
        private decimal saldoPendiente = 0m;

        public FormPago()
        {
            InicializarComponentes();
            CargarCotizaciones();
        }

        private void InicializarComponentes()
        {
            this.Text = "Registro de Pagos y Abonos";
            this.Size = new Size(820, 580);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTituloModulo = new Label()
            {
                Text = "Gestión Financiera - Registro de Pagos y Abonos",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                Left = 40,
                Top = 25,
                Width = 550,
                Height = 35
            };

            int top = 80;

            // Seleccionar Cotización
            Label lblCot = new Label() { Text = "Cotización:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            cmbCotizaciones = new ComboBox() { Left = 190, Top = top, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbCotizaciones.SelectedIndexChanged += CmbCotizaciones_SelectedIndexChanged;
            top += 45;

            // Información de Saldos
            lblTotalCotizacion = new Label() { Text = "Monto Total Cotización: $0.00", Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.DimGray };
            top += 30;
            lblSaldoPendiente = new Label() { Text = "Saldo Pendiente: $0.00", Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkRed };
            top += 45;

            // Monto a Pagar
            Label lblMonto = new Label() { Text = "Monto a Abonar ($):", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtMontoPago = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            // Tipo de Pago
            Label lblTipo = new Label() { Text = "Método de Pago:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            cmbTipoPago = new ComboBox() { Left = 190, Top = top, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbTipoPago.Items.AddRange(new string[] { "Transferencia", "Efectivo" });
            cmbTipoPago.SelectedIndex = 0;
            cmbTipoPago.SelectedIndexChanged += CmbTipoPago_SelectedIndexChanged;
            top += 50;

            // --- Campos Transferencia ---
            lblBanco = new Label() { Text = "Banco Origen:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtBanco = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            lblSpei = new Label() { Text = "Clave Rastreo SPEI:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtSpei = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };

            // --- Campos Efectivo ---
            lblRecibo = new Label() { Text = "Recibido Por (Nombre):", Left = 40, Top = top - 45, Width = 140, Visible = false, Font = new Font("Segoe UI", 10) };
            txtRecibo = new TextBox() { Left = 190, Top = top - 45, Width = 340, Visible = false, Font = new Font("Segoe UI", 10) };

            top += 60;

            btnRegistrarPago = new Button()
            {
                Text = "💳 Registrar Pago en MySQL",
                Left = 190,
                Top = top,
                Width = 240,
                Height = 42,
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegistrarPago.FlatAppearance.BorderSize = 0;
            btnRegistrarPago.Click += BtnRegistrarPago_Click;

            this.Controls.Add(lblTituloModulo);
            this.Controls.Add(lblCot); this.Controls.Add(cmbCotizaciones);
            this.Controls.Add(lblTotalCotizacion);
            this.Controls.Add(lblSaldoPendiente);
            this.Controls.Add(lblMonto); this.Controls.Add(txtMontoPago);
            this.Controls.Add(lblTipo); this.Controls.Add(cmbTipoPago);

            this.Controls.Add(lblBanco); this.Controls.Add(txtBanco);
            this.Controls.Add(lblSpei); this.Controls.Add(txtSpei);
            this.Controls.Add(lblRecibo); this.Controls.Add(txtRecibo);

            this.Controls.Add(btnRegistrarPago);
        }

        private void CargarCotizaciones()
        {
            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();
                    string query = "SELECT c.id_cot, CONCAT('Cotización #', c.id_cot, ' - ', cl.nom_cli) AS info " +
                                   "FROM Cotizacion c INNER JOIN Cliente cl ON c.id_cli = cl.id_cli " +
                                   "WHERE c.estado_cot != 'Pagada'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbCotizaciones.DataSource = dt;
                    cmbCotizaciones.DisplayMember = "info";
                    cmbCotizaciones.ValueMember = "id_cot";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cotizaciones: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCotizaciones_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbCotizaciones.SelectedValue != null && int.TryParse(cmbCotizaciones.SelectedValue.ToString(), out int idCot))
            {
                try
                {
                    using (var con = ConexionBD.ObtenerConexion())
                    {
                        con.Open();
                        // 1. Obtener Total de la Cotización
                        string queryTot = "SELECT total_cot FROM Cotizacion WHERE id_cot = @id";
                        MySqlCommand cmdTot = new MySqlCommand(queryTot, con);
                        cmdTot.Parameters.AddWithValue("@id", idCot);
                        totalCot = Convert.ToDecimal(cmdTot.ExecuteScalar());

                        // 2. Obtener Suma de Abonos Anteriores
                        string queryPagos = "SELECT IFNULL(SUM(monto_pago), 0) FROM Pago WHERE id_cot = @id";
                        MySqlCommand cmdPagos = new MySqlCommand(queryPagos, con);
                        cmdPagos.Parameters.AddWithValue("@id", idCot);
                        abonosPrevios = Convert.ToDecimal(cmdPagos.ExecuteScalar());

                        saldoPendiente = totalCot - abonosPrevios;

                        lblTotalCotizacion.Text = $"Monto Total Cotización: ${totalCot:N2}";
                        lblSaldoPendiente.Text = $"Saldo Pendiente: ${saldoPendiente:N2}";
                        txtMontoPago.Text = saldoPendiente.ToString("F2");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al consultar saldo: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CmbTipoPago_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool esTransf = cmbTipoPago.SelectedItem?.ToString() == "Transferencia";

            lblBanco.Visible = esTransf; txtBanco.Visible = esTransf;
            lblSpei.Visible = esTransf; txtSpei.Visible = esTransf;

            lblRecibo.Visible = !esTransf; txtRecibo.Visible = !esTransf;
        }

        private void BtnRegistrarPago_Click(object? sender, EventArgs e)
        {
            if (cmbCotizaciones.SelectedValue == null || string.IsNullOrWhiteSpace(txtMontoPago.Text))
            {
                MessageBox.Show("Por favor ingresa un monto válido a abonar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal montoAbono = Convert.ToDecimal(txtMontoPago.Text);

            if (montoAbono <= 0 || montoAbono > saldoPendiente)
            {
                MessageBox.Show($"El monto a abonar debe ser mayor a 0 y no puede superar el saldo pendiente (${saldoPendiente:N2}).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        int idCot = Convert.ToInt32(cmbCotizaciones.SelectedValue);

                        // 1. Insertar en Tabla Padre: Pago
                        string queryPadre = "INSERT INTO Pago (monto_pago, fecha_pago, tipo_pago, id_cot, estatus_pago) " +
                                            "VALUES (@monto, NOW(), @tipo, @idCot, 'Registrado'); SELECT LAST_INSERT_ID();";

                        MySqlCommand cmdPadre = new MySqlCommand(queryPadre, con, trans);
                        cmdPadre.Parameters.AddWithValue("@monto", montoAbono);
                        cmdPadre.Parameters.AddWithValue("@tipo", cmbTipoPago.SelectedItem?.ToString());
                        cmdPadre.Parameters.AddWithValue("@idCot", idCot);

                        int idPagoGenerado = Convert.ToInt32(cmdPadre.ExecuteScalar());

                        // 2. Insertar en Tabla Hija (Polimorfismo en BD)
                        if (cmbTipoPago.SelectedItem?.ToString() == "Transferencia")
                        {
                            string queryHija = "INSERT INTO Pago_Transferencia (id_pago, banco_origen, clave_rastreo_spei) VALUES (@id, @banco, @spei);";
                            MySqlCommand cmdHija = new MySqlCommand(queryHija, con, trans);
                            cmdHija.Parameters.AddWithValue("@id", idPagoGenerado);
                            cmdHija.Parameters.AddWithValue("@banco", txtBanco.Text);
                            cmdHija.Parameters.AddWithValue("@spei", txtSpei.Text);
                            cmdHija.ExecuteNonQuery();
                        }
                        else
                        {
                            string queryHija = "INSERT INTO Pago_Efectivo (id_pago, recibo_por) VALUES (@id, @recibo);";
                            MySqlCommand cmdHija = new MySqlCommand(queryHija, con, trans);
                            cmdHija.Parameters.AddWithValue("@id", idPagoGenerado);
                            cmdHija.Parameters.AddWithValue("@recibo", txtRecibo.Text);
                            cmdHija.ExecuteNonQuery();
                        }

                        // 3. Actualizar Estatus de la Cotización
                        decimal nuevoSaldo = saldoPendiente - montoAbono;
                        string nuevoEstatus = (nuevoSaldo <= 0) ? "Pagada" : "Abonada";

                        string queryUpd = "UPDATE Cotizacion SET estado_cot = @estatus WHERE id_cot = @idCot";
                        MySqlCommand cmdUpd = new MySqlCommand(queryUpd, con, trans);
                        cmdUpd.Parameters.AddWithValue("@estatus", nuevoEstatus);
                        cmdUpd.Parameters.AddWithValue("@idCot", idCot);
                        cmdUpd.ExecuteNonQuery();

                        trans.Commit();

                        MessageBox.Show($"¡Pago registrado correctamente en MySQL!\nEstatus de la cotización: {nuevoEstatus}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Recargar lista y limpiar
                        CargarCotizaciones();
                        txtBanco.Clear();
                        txtSpei.Clear();
                        txtRecibo.Clear();
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
                MessageBox.Show("Error al registrar pago en MySQL: " + ex.Message, "Error BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}