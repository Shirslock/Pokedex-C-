using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon;
        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {   
            if(dgvPokemons.CurrentRow != null) //EVITAMOS EL CRASH AL TOCAR EL TXT VACIO//
            {
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
            

        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumnas();
                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            //cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e) //MODIFICAR// //M.1//
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; 

            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();

        }

        private void btnEliminacionFisica_Click(object sender, EventArgs e) //E.2//
        {
            eliminar();

        }

        private void btnEliminacionLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false) //EL 2//
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {

                DialogResult respuesta = MessageBox.Show("¿Desea eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);//E.3//
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;// "POKEMON DE LA LISTA"

                    if(logico)
                        negocio.eliminarLogica(seleccionado.Id);
                    else
                        negocio.eliminarFisica(seleccionado.Id);
                        MessageBox.Show("Eliminado :(");

                    cargar();
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e) //FR.1//
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltrar.Text;

            if(filtro.Length > 3)
            {
                listaFiltrada = listaPokemon.FindAll(X => X.Nombre.ToUpper().Contains(filtro.ToUpper()) || X.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }
            
            dgvPokemons.DataSource = null; //FR2// HAGO LIMPIEZA DE GRILLA PRIMERO//
            dgvPokemons.DataSource = listaFiltrada;//FR.3 BUSCA//
            ocultarColumnas();
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltrar.Text;

            if (filtro.Length > 3)
            {
                listaFiltrada = listaPokemon.FindAll(X => X.Nombre.ToUpper().Contains(filtro.ToUpper()) || X.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null; //FR2// HAGO LIMPIEZA DE GRILLA PRIMERO//
            dgvPokemons.DataSource = listaFiltrada;//FR.3 BUSCA//
            ocultarColumnas();
        }
    }
}
