using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace App_Discos
{
    public partial class FrmDiscos : Form
    {
        private List<Discos> listaDiscos;
        public FrmDiscos()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Id");
            cboCampo.Items.Add("Titulo");
            cboCampo.Items.Add("Genero");
        }

        private void cargar()
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                listaDiscos = negocio.Listado();
                dgvListado.DataSource = listaDiscos;
                ocultarColumna();
                cargarImagen(listaDiscos[0].UrlImagenTapa);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ocultarColumna()
        {
            dgvListado.Columns["UrlImagenTapa"].Visible = false;
        }

        private void dgvListado_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvListado.CurrentRow != null)
            {
                Discos seleccionado = (Discos)dgvListado.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagenTapa);
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbAlbum.Load(imagen);
            }
            catch (Exception ex)
            {
                pbAlbum.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAgregarDisco alta = new FrmAgregarDisco();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Discos seleccionado;
            seleccionado = (Discos)dgvListado.CurrentRow.DataBoundItem;

            FrmAgregarDisco modificar = new FrmAgregarDisco(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            Discos seleccionado;
            try
            {
                DialogResult resultado = MessageBox.Show("¿Desea eliminar el disco?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Discos)dgvListado.CurrentRow.DataBoundItem;
                    if(logico)
                    {
                        negocio.deleteLogico(seleccionado.Id);
                        cargar();
                    }
                    else
                    {
                        negocio.delete(seleccionado.Id);
                        cargar();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvListado.DataSource = negocio.Filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Discos> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                listaFiltrada = listaDiscos.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper()) || x.Genero.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDiscos;
            }

            dgvListado.DataSource = null;
            dgvListado.DataSource = listaFiltrada;
            ocultarColumna();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Id")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
