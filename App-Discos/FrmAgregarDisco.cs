using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;

namespace App_Discos
{
    public partial class FrmAgregarDisco : Form
    {
        private Discos disco = null;
        public FrmAgregarDisco()
        {
            InitializeComponent();
        }

        public FrmAgregarDisco(Discos disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar Disco";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                if (disco == null)
                    disco = new Discos();

                disco.Titulo = txtTitulo.Text;
                disco.Fecha_Lanzamiento = dtpFechaLanzamiento.Value;
                disco.Cant_Canciones = int.Parse(txtCantCanciones.Text);
                disco.UrlImagenTapa = txtUrl.Text;
                disco.Genero = (Estilos)cboEstilo.SelectedItem;
                disco.Edicion = (TipoEdicion)cboEdicion.SelectedItem;

                if(disco.Id == 0)
                {
                    negocio.Add(disco);
                    MessageBox.Show("Disco agregado de manera exitosa...");
                }
                else
                {
                    negocio.modify(disco);
                    MessageBox.Show("Disco modificado de manera exitosa...");
                }
                    Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmAgregarDisco_Load(object sender, EventArgs e)
        {
            EstiloNegocio elementoNegocio = new EstiloNegocio();
            TipoEdicionNegocio elementoEdicion = new TipoEdicionNegocio();
            try
            {
                cboEstilo.DataSource = elementoNegocio.Listado();
                cboEstilo.ValueMember = "Id";
                cboEstilo.DisplayMember = "Descripcion";
                cboEdicion.DataSource = elementoEdicion.Listado();
                cboEdicion.ValueMember = "Id";
                cboEdicion.DisplayMember = "Descripcion";

                if(disco != null)
                {
                    txtTitulo.Text = disco.Titulo;
                    dtpFechaLanzamiento.Value = disco.Fecha_Lanzamiento;
                    txtCantCanciones.Text = disco.Cant_Canciones.ToString();
                    txtUrl.Text = disco.UrlImagenTapa;
                    cargarImagen(disco.UrlImagenTapa);
                    cboEstilo.SelectedValue = disco.Genero.Id;
                    cboEdicion.SelectedValue = disco.Edicion.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrl.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxUrlimagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxUrlimagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
    }
}
