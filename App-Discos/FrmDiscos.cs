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
        }

        private void cargar()
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                listaDiscos = negocio.Listado();
                dgvListado.DataSource = listaDiscos;
                dgvListado.Columns["UrlImagenTapa"].Visible = false;
                cargarImagen(listaDiscos[0].UrlImagenTapa);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void dgvListado_SelectionChanged(object sender, EventArgs e)
        {
            Discos seleccionado = (Discos)dgvListado.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagenTapa);
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
            DiscosNegocio negocio = new DiscosNegocio();
            Discos seleccionado;
            try
            {
                DialogResult resultado = MessageBox.Show("¿Desea eliminar el disco?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes) 
                {
                    seleccionado = (Discos)dgvListado.CurrentRow.DataBoundItem;
                    negocio.delete(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
