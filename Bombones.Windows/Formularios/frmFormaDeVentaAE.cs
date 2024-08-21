using Bombones.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bombones.Windows.Formularios
{
    public partial class frmFormaDeVentaAE : Form
    {
        private FormaVenta formaVenta;
        public frmFormaDeVentaAE()
        {
            InitializeComponent();
        }

        public FormaVenta GetForma()
        {
            return formaVenta;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (formaVenta is null)
                {
                    formaVenta = new FormaVenta();
                }
                formaVenta.Descripcion = txtForma.Text;
                DialogResult = DialogResult.OK;
            }

        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtForma.Text))
            {
                valido = false;
                errorProvider1.SetError(txtForma, "La forma de venta es necesaria");

            }
            return valido;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void SetTipo(FormaVenta forma)
        {
            formaVenta = forma;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(formaVenta != null)
            {
                txtForma.Text = formaVenta.Descripcion;
            }
        }
    }
}
