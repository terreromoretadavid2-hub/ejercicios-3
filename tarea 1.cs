
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiAplicacion
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormularioPrincipal());
        }
    }

    public class FormularioPrincipal : Form
    {
        TextBox txtNombre;
        Button btnAgregar;
        Button btnEliminar;
        Button btnLimpiar;
        ListBox lstNombres;
        Label lblTitulo;
        Label lblCantidad;

        public FormularioPrincipal()
        {
            Text = "Gestor de Nombres";
            Size = new Size(520, 450);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10);

            lblTitulo = new Label();
            lblTitulo.Text = "GESTOR DE NOMBRES";
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(440, 35);
            lblTitulo.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            lblTitulo.ForeColor = Color.DarkBlue;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            txtNombre = new TextBox();
            txtNombre.Location = new Point(30, 75);
            txtNombre.Size = new Size(290, 30);
            txtNombre.PlaceholderText = "Escribe un nombre...";

            btnAgregar = new Button();
            btnAgregar.Text = "Agregar";
            btnAgregar.Location = new Point(335, 73);
            btnAgregar.Size = new Size(125, 35);
            btnAgregar.BackColor = Color.LightGreen;
            btnAgregar.UseVisualStyleBackColor = false;

            lstNombres = new ListBox();
            lstNombres.Location = new Point(30, 125);
            lstNombres.Size = new Size(430, 190);
            lstNombres.Font = new Font("Segoe UI", 11);

            lblCantidad = new Label();
            lblCantidad.Text = "Total de nombres: 0";
            lblCantidad.Location = new Point(30, 325);
            lblCantidad.Size = new Size(250, 25);

            btnEliminar = new Button();
            btnEliminar.Text = "Eliminar seleccionado";
            btnEliminar.Location = new Point(30, 365);
            btnEliminar.Size = new Size(200, 35);
            btnEliminar.BackColor = Color.LightCoral;
            btnEliminar.UseVisualStyleBackColor = false;

            btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar lista";
            btnLimpiar.Location = new Point(250, 365);
            btnLimpiar.Size = new Size(150, 35);

            btnAgregar.Click += (sender, e) =>
            {
                AgregarNombre();
            };

            txtNombre.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AgregarNombre();
                    e.SuppressKeyPress = true;
                }
            };

            btnEliminar.Click += (sender, e) =>
            {
                if (lstNombres.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecciona un nombre para eliminar.");
                    return;
                }

                lstNombres.Items.RemoveAt(lstNombres.SelectedIndex);
                ActualizarContador();
            };

            btnLimpiar.Click += (sender, e) =>
            {
                if (lstNombres.Items.Count == 0)
                {
                    MessageBox.Show("La lista ya está vacía.");
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Quieres eliminar todos los nombres?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.Yes)
                {
                    lstNombres.Items.Clear();
                    ActualizarContador();
                }
            };

            Controls.Add(lblTitulo);
            Controls.Add(txtNombre);
            Controls.Add(btnAgregar);
            Controls.Add(lstNombres);
            Controls.Add(lblCantidad);
            Controls.Add(btnEliminar);
            Controls.Add(btnLimpiar);
        }

        void AgregarNombre()
        {
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Escribe un nombre primero.");
                txtNombre.Focus();
                return;
            }

            foreach (object elemento in lstNombres.Items)
            {
                if (string.Equals(
                    elemento.ToString(),
                    nombre,
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ese nombre ya existe.");
                    txtNombre.SelectAll();
                    txtNombre.Focus();
                    return;
                }
            }

            lstNombres.Items.Add(nombre);
            txtNombre.Clear();
            txtNombre.Focus();

            ActualizarContador();
        }

        void ActualizarContador()
        {
            lblCantidad.Text =
                $"Total de nombres: {lstNombres.Items.Count}";
        }
    }
}
