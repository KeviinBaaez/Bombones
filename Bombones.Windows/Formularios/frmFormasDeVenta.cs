using Bombones.Entidades.Entidades;
using Bombones.Servicios.Servicios;
using Bombones.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class frmFormasDeVenta : Form
    {
        private readonly IServiciosFormaVenta? _servicio;
        private List<FormaVenta>? lista;
        public frmFormasDeVenta(IServiceProvider? serviceProvider)
        {
            InitializeComponent();
            _servicio = serviceProvider?.GetService<IServiciosFormaVenta>();
        }

        private void frmFormasDeVenta_Load(object sender, EventArgs e)
        {
            if (_servicio is null)
            {
                throw new ApplicationException("Dependencias no cargadas");
            }
            lista = _servicio.GetLista();
            MostrarDatosEnGrilla();
        }

        private void MostrarDatosEnGrilla()
        {
            if (lista is null)
            {
                return;
            }
            GridHelper.LimpiarGrilla(dgvDatos);
            foreach (var item in lista)
            {
                var r = GridHelper.ConstruirFila(dgvDatos);
                GridHelper.SetearFila(r, item);
                GridHelper.AgregarFila(r, dgvDatos);
            }
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmFormaDeVentaAE frm = new frmFormaDeVentaAE() { Text = "Nueva forma de venta" };
            DialogResult dr = frm.ShowDialog(this);
            FormaVenta forma = frm.GetForma();

            try
            {
                if (_servicio is null)
                {
                    throw new ApplicationException("Dependencias no cargadas");
                }
                if (dr == DialogResult.Cancel) return;
                if (!_servicio.Existe(forma))
                {
                    _servicio.Guardar(forma);
                    var r = GridHelper.ConstruirFila(dgvDatos);
                    GridHelper.SetearFila(r, forma);
                    GridHelper.AgregarFila(r, dgvDatos);
                    MessageBox.Show("Registro Agregado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro Existente",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                throw;
            }
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];

            FormaVenta forma = null!;
            if (r.Tag != null)
            {
                forma = (FormaVenta)r.Tag;

            }
            try
            {
                DialogResult dr = MessageBox.Show($"¿Desea dar de baja la forma de venta {forma.Descripcion}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.No)
                {
                    return;
                }
                if (_servicio is null)
                {
                    throw new ApplicationException("Dependencias no cargadas");
                }
                if (!_servicio.EstaRelacionado(forma.FormaDeVentaId))
                {
                    _servicio.Borrar(forma.FormaDeVentaId);

                    GridHelper.QuitarFila(r, dgvDatos);
                    MessageBox.Show("Registro eliminado satisfactoriamente",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Registro relacionado",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                var r = dgvDatos.SelectedRows[0];
                FormaVenta forma;
                if (r.Tag != null)
                {
                    forma = (FormaVenta)r.Tag;
                    frmFormaDeVentaAE frm = new frmFormaDeVentaAE() { Text = "Editar forma de venta" };
                    frm.SetTipo(forma);
                    DialogResult dr = frm.ShowDialog(this);

                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                    forma = frm.GetForma();
                    if (_servicio is null)
                    {
                        throw new ApplicationException("Dependencias no cargadas");
                    }
                    if (!_servicio.Existe(forma))
                    {
                        _servicio.Guardar(forma);
                        GridHelper.SetearFila(r, forma);
                        MessageBox.Show("Registro modificado",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Registro existente",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                throw;
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
