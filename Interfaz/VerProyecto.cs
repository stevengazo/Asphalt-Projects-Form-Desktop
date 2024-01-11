﻿using Modelos;
using Negocio;
using Negocios;
using System.Data;
using System.Globalization;

namespace Interfaz
{
    public partial class VerProyecto : Form
    {
        public int idProyecto { get; set; } = 0;
        public VerProyecto()
        {
            InitializeComponent();
        }

        private void VerProyecto_Load(object sender, EventArgs e)
        {
            CargarProyectoDetallado();
        }


        private async Task CargarNotas(int id)
        {
            List<Nota> Notas = NotaNegocio.GetNotasByProyecto(id);
            if (Notas.Count > 0)
            {
                //  dataGridViewComentarios.Columns.Clear();

                DataTable _dt = new();

                _dt.Columns.Add("Titulo");
                _dt.Columns.Add("Autor");

                foreach (Nota nota in Notas)
                {
                    _dt.Rows.Add(
                            nota.Titulo,
                            nota.Autor
                        );
                }
                dataGridViewComentarios.DataSource = _dt;

                DataGridViewButtonColumn botonVer = new();
                botonVer.HeaderText = "Ver";
                botonVer.Text = "Ver";
                botonVer.Name = "btnVerNota";
                botonVer.UseColumnTextForButtonValue = true;
                dataGridViewComentarios.Columns.Add(botonVer);

                DataGridViewButtonColumn botonEditar = new();
                botonEditar.HeaderText = "Editar";
                botonEditar.Text = "Editar";
                botonEditar.Name = "btnEditarNota";
                botonEditar.UseColumnTextForButtonValue = true;
                dataGridViewComentarios.Columns.Add(botonEditar);
            }
        }

        private void CargarProyectoDetallado()
        {
            try
            {
                if (idProyecto == 0)
                {
                    MessageBox.Show("Error interno, no hay un proyecto especificado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ProyectoNegocios proyectoNegocios = new();
                    Proyecto proyectoTemporal = proyectoNegocios.ObtenerProyecto(idProyecto);
                    this.idProyecto = proyectoTemporal.ProyectoId;
                    CargarNotas(proyectoTemporal.ProyectoId);
                    txtNumeroProyecto.Text = $"P-{proyectoTemporal.ProyectoId}";
                    txtEstado.Text = proyectoTemporal.Estado;
                    txtVendedor.Text = proyectoTemporal.Vendedor.Nombre;
                    txtRazonSocial.Text = proyectoTemporal.Cliente;
                    txtOC.Text = proyectoTemporal.FechaOC.ToLongDateString();
                    txtContacto.Text = proyectoTemporal.Contacto;
                    txtOferta.Text = proyectoTemporal.OfertaId;
                    txtMontoProyecto.Text = proyectoTemporal.Monto.ToString("C", CultureInfo.CurrentCulture);
                    txtPorcentaje.Text = $"{proyectoTemporal.PorcentajeAnticipo}%";
                    txtNumeroFacturaAnticipo.Text = proyectoTemporal.FacturaAnticipoId;
                    txtNumeroTarea.Text = proyectoTemporal.TareaId.ToString();
                    txtUbicacion.Text = proyectoTemporal.Ubicacion;
                    txtNotas.Text = proyectoTemporal.Tipo;
                    txtFechaInicio.Text = proyectoTemporal.FechaInicio.ToLongDateString();
                    txtFechaFinal.Text = proyectoTemporal.FechaFinal.ToLongDateString();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show($"Error interno: {f.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTituloNota.Text) || string.IsNullOrEmpty(txtDescripcionNota.Text))
            {
                MessageBox.Show("Verifique los campos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Nota nota = new Nota()
                {
                    Titulo = txtTituloNota.Text,
                    Descripcion = txtDescripcionNota.Text,
                    Autor = Temporal.UsuarioActivo.Nombre,
                    ProyectoId = this.idProyecto
                };
                NotaNegocio.Add(nota);
                MessageBox.Show("Nota Agregada", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarNotas(this.idProyecto);
            }
        }
    }
}
