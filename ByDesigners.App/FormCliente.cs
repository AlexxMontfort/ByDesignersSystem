using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ByDesigners.App
{
    public class FormCliente : Form
    {
        private TextBox txtNombre = null!;
        private TextBox txtDireccion = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtEmail = null!;
        private ComboBox cmbTipoCliente = null!;

        private Label lblEspecial = null!;
        private TextBox txtEspecial1 = null!;
        private Label lblEspecial2 = null!;
        private TextBox txtEspecial2 = null!;

        private Button btnGuardar = null!;

        public FormCliente()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Registro de Clientes";
            this.Size = new Size(820, 580);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTituloModulo = new Label()
            {
                Text = "Gestión y Registro de Clientes",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                Left = 40,
                Top = 25,
                Width = 500,
                Height = 35
            };

            int top = 80;

            Label lblNombre = new Label() { Text = "Nombre Comercial:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtNombre = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            Label lblDir = new Label() { Text = "Dirección:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtDireccion = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            Label lblTel = new Label() { Text = "Teléfono:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtTelefono = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            Label lblEmail = new Label() { Text = "Correo Electrónico:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtEmail = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            Label lblTipo = new Label() { Text = "Tipo de Cliente:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            cmbTipoCliente = new ComboBox() { Left = 190, Top = top, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbTipoCliente.Items.AddRange(new string[] { "Particular", "Empresa" });
            cmbTipoCliente.SelectedIndex = 0;
            cmbTipoCliente.SelectedIndexChanged += CmbTipoCliente_SelectedIndexChanged;
            top += 50;

            lblEspecial = new Label() { Text = "Nombre Completo:", Left = 40, Top = top, Width = 140, Font = new Font("Segoe UI", 10) };
            txtEspecial1 = new TextBox() { Left = 190, Top = top, Width = 340, Font = new Font("Segoe UI", 10) };
            top += 45;

            lblEspecial2 = new Label() { Text = "Razón Social:", Left = 40, Top = top, Width = 140, Visible = false, Font = new Font("Segoe UI", 10) };
            txtEspecial2 = new TextBox() { Left = 190, Top = top, Width = 340, Visible = false, Font = new Font("Segoe UI", 10) };
            top += 55;

            btnGuardar = new Button()
            {
                Text = "💾 Guardar Cliente",
                Left = 190,
                Top = top,
                Width = 200,
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
            this.Controls.Add(lblDir); this.Controls.Add(txtDireccion);
            this.Controls.Add(lblTel); this.Controls.Add(txtTelefono);
            this.Controls.Add(lblEmail); this.Controls.Add(txtEmail);
            this.Controls.Add(lblTipo); this.Controls.Add(cmbTipoCliente);
            this.Controls.Add(lblEspecial); this.Controls.Add(txtEspecial1);
            this.Controls.Add(lblEspecial2); this.Controls.Add(txtEspecial2);
            this.Controls.Add(btnGuardar);
        }

        private void CmbTipoCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbTipoCliente.SelectedItem?.ToString() == "Empresa")
            {
                lblEspecial.Text = "RFC:";
                lblEspecial2.Visible = true;
                txtEspecial2.Visible = true;
            }
            else
            {
                lblEspecial.Text = "Nombre Completo:";
                lblEspecial2.Visible = false;
                txtEspecial2.Visible = false;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor ingresa el nombre comercial del cliente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var con = ConexionBD.ObtenerConexion())
                {
                    con.Open();

                    string queryPadre = "INSERT INTO Cliente (nom_cli, dir_cli, tel_cli, email_cli, fecha_reg, tipo_cliente) " +
                                        "VALUES (@nom, @dir, @tel, @email, NOW(), @tipo); SELECT LAST_INSERT_ID();";

                    MySqlCommand cmdPadre = new MySqlCommand(queryPadre, con);
                    cmdPadre.Parameters.AddWithValue("@nom", txtNombre.Text);
                    cmdPadre.Parameters.AddWithValue("@dir", txtDireccion.Text);
                    cmdPadre.Parameters.AddWithValue("@tel", txtTelefono.Text);
                    cmdPadre.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmdPadre.Parameters.AddWithValue("@tipo", cmbTipoCliente.SelectedItem?.ToString());

                    int idGenerado = Convert.ToInt32(cmdPadre.ExecuteScalar());

                    if (cmbTipoCliente.SelectedItem?.ToString() == "Particular")
                    {
                        string queryHija = "INSERT INTO Cliente_Particular (id_cliente, nombre_completo) VALUES (@id, @nomComp);";
                        MySqlCommand cmdHija = new MySqlCommand(queryHija, con);
                        cmdHija.Parameters.AddWithValue("@id", idGenerado);
                        cmdHija.Parameters.AddWithValue("@nomComp", txtEspecial1.Text);
                        cmdHija.ExecuteNonQuery();
                    }
                    else
                    {
                        string queryHija = "INSERT INTO Cliente_Empresa (id_cliente, rfc, razon_social, contacto_puesto) VALUES (@id, @rfc, @razon, @contacto);";
                        MySqlCommand cmdHija = new MySqlCommand(queryHija, con);
                        cmdHija.Parameters.AddWithValue("@id", idGenerado);
                        cmdHija.Parameters.AddWithValue("@rfc", txtEspecial1.Text);
                        cmdHija.Parameters.AddWithValue("@razon", txtEspecial2.Text);
                        cmdHija.Parameters.AddWithValue("@contacto", "Contacto General");
                        cmdHija.ExecuteNonQuery();
                    }

                    MessageBox.Show("¡Cliente guardado exitosamente en la base de datos MySQL!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtEspecial1.Clear();
            txtEspecial2.Clear();
        }
    }
}