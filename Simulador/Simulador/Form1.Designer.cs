namespace Simulador
{
    partial class frmSimulador
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtServicio = new System.Windows.Forms.TextBox();
            this.txtLlaveServicio = new System.Windows.Forms.TextBox();
            this.btnConsultarRecibo = new System.Windows.Forms.Button();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblServicio = new System.Windows.Forms.Label();
            this.lblLlave = new System.Windows.Forms.Label();
            this.lblTituloConsultas = new System.Windows.Forms.Label();
            this.lblTituloPagos = new System.Windows.Forms.Label();
            this.lblIdentificacionpagos = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPagar = new System.Windows.Forms.Button();
            this.txtIdentificacionPagos = new System.Windows.Forms.TextBox();
            this.txtServicioPagos = new System.Windows.Forms.TextBox();
            this.txtLlavePagos = new System.Windows.Forms.TextBox();
            this.txtNumServicioPago = new System.Windows.Forms.TextBox();
            this.txtCodigoSeguridadPago = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(158, 134);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 22);
            this.txtUser.TabIndex = 0;
            // 
            // txtServicio
            // 
            this.txtServicio.Location = new System.Drawing.Point(158, 189);
            this.txtServicio.Name = "txtServicio";
            this.txtServicio.Size = new System.Drawing.Size(100, 22);
            this.txtServicio.TabIndex = 1;
            // 
            // txtLlaveServicio
            // 
            this.txtLlaveServicio.Location = new System.Drawing.Point(158, 244);
            this.txtLlaveServicio.Name = "txtLlaveServicio";
            this.txtLlaveServicio.Size = new System.Drawing.Size(100, 22);
            this.txtLlaveServicio.TabIndex = 2;
            // 
            // btnConsultarRecibo
            // 
            this.btnConsultarRecibo.Location = new System.Drawing.Point(158, 299);
            this.btnConsultarRecibo.Name = "btnConsultarRecibo";
            this.btnConsultarRecibo.Size = new System.Drawing.Size(75, 23);
            this.btnConsultarRecibo.TabIndex = 3;
            this.btnConsultarRecibo.Text = "Consultar";
            this.btnConsultarRecibo.UseVisualStyleBackColor = true;
            this.btnConsultarRecibo.Click += new System.EventHandler(this.btnConsultarRecibo_Click);
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(67, 143);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(48, 16);
            this.lblCliente.TabIndex = 4;
            this.lblCliente.Text = "Cliente";
            this.lblCliente.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblServicio
            // 
            this.lblServicio.AutoSize = true;
            this.lblServicio.Location = new System.Drawing.Point(67, 189);
            this.lblServicio.Name = "lblServicio";
            this.lblServicio.Size = new System.Drawing.Size(56, 16);
            this.lblServicio.TabIndex = 5;
            this.lblServicio.Text = "Servicio";
            // 
            // lblLlave
            // 
            this.lblLlave.AutoSize = true;
            this.lblLlave.Location = new System.Drawing.Point(67, 244);
            this.lblLlave.Name = "lblLlave";
            this.lblLlave.Size = new System.Drawing.Size(40, 16);
            this.lblLlave.TabIndex = 6;
            this.lblLlave.Text = "Llave";
            // 
            // lblTituloConsultas
            // 
            this.lblTituloConsultas.AutoSize = true;
            this.lblTituloConsultas.Location = new System.Drawing.Point(155, 85);
            this.lblTituloConsultas.Name = "lblTituloConsultas";
            this.lblTituloConsultas.Size = new System.Drawing.Size(66, 16);
            this.lblTituloConsultas.TabIndex = 7;
            this.lblTituloConsultas.Text = "Consultas";
            // 
            // lblTituloPagos
            // 
            this.lblTituloPagos.AutoSize = true;
            this.lblTituloPagos.Location = new System.Drawing.Point(523, 85);
            this.lblTituloPagos.Name = "lblTituloPagos";
            this.lblTituloPagos.Size = new System.Drawing.Size(47, 16);
            this.lblTituloPagos.TabIndex = 8;
            this.lblTituloPagos.Text = "Pagos";
            // 
            // lblIdentificacionpagos
            // 
            this.lblIdentificacionpagos.AutoSize = true;
            this.lblIdentificacionpagos.Location = new System.Drawing.Point(414, 140);
            this.lblIdentificacionpagos.Name = "lblIdentificacionpagos";
            this.lblIdentificacionpagos.Size = new System.Drawing.Size(85, 16);
            this.lblIdentificacionpagos.TabIndex = 9;
            this.lblIdentificacionpagos.Text = "Identificacion";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(414, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Servicio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(414, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Llave";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(414, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Numero Servicio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(385, 280);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Codigo de Seguridad";
            // 
            // btnPagar
            // 
            this.btnPagar.Location = new System.Drawing.Point(526, 322);
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(75, 23);
            this.btnPagar.TabIndex = 14;
            this.btnPagar.Text = "Pagar";
            this.btnPagar.UseVisualStyleBackColor = true;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            // 
            // txtIdentificacionPagos
            // 
            this.txtIdentificacionPagos.Location = new System.Drawing.Point(526, 134);
            this.txtIdentificacionPagos.Name = "txtIdentificacionPagos";
            this.txtIdentificacionPagos.Size = new System.Drawing.Size(100, 22);
            this.txtIdentificacionPagos.TabIndex = 15;
            // 
            // txtServicioPagos
            // 
            this.txtServicioPagos.Location = new System.Drawing.Point(526, 178);
            this.txtServicioPagos.Name = "txtServicioPagos";
            this.txtServicioPagos.Size = new System.Drawing.Size(100, 22);
            this.txtServicioPagos.TabIndex = 16;
            // 
            // txtLlavePagos
            // 
            this.txtLlavePagos.Location = new System.Drawing.Point(526, 214);
            this.txtLlavePagos.Name = "txtLlavePagos";
            this.txtLlavePagos.Size = new System.Drawing.Size(100, 22);
            this.txtLlavePagos.TabIndex = 17;
            // 
            // txtNumServicioPago
            // 
            this.txtNumServicioPago.Location = new System.Drawing.Point(526, 247);
            this.txtNumServicioPago.Name = "txtNumServicioPago";
            this.txtNumServicioPago.Size = new System.Drawing.Size(100, 22);
            this.txtNumServicioPago.TabIndex = 18;
            // 
            // txtCodigoSeguridadPago
            // 
            this.txtCodigoSeguridadPago.Location = new System.Drawing.Point(526, 280);
            this.txtCodigoSeguridadPago.Name = "txtCodigoSeguridadPago";
            this.txtCodigoSeguridadPago.Size = new System.Drawing.Size(100, 22);
            this.txtCodigoSeguridadPago.TabIndex = 19;
            // 
            // frmSimulador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 450);
            this.Controls.Add(this.txtCodigoSeguridadPago);
            this.Controls.Add(this.txtNumServicioPago);
            this.Controls.Add(this.txtLlavePagos);
            this.Controls.Add(this.txtServicioPagos);
            this.Controls.Add(this.txtIdentificacionPagos);
            this.Controls.Add(this.btnPagar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIdentificacionpagos);
            this.Controls.Add(this.lblTituloPagos);
            this.Controls.Add(this.lblTituloConsultas);
            this.Controls.Add(this.lblLlave);
            this.Controls.Add(this.lblServicio);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.btnConsultarRecibo);
            this.Controls.Add(this.txtLlaveServicio);
            this.Controls.Add(this.txtServicio);
            this.Controls.Add(this.txtUser);
            this.Name = "frmSimulador";
            this.Text = "SimuladorConsultaPagos";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtServicio;
        private System.Windows.Forms.TextBox txtLlaveServicio;
        private System.Windows.Forms.Button btnConsultarRecibo;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label lblServicio;
        private System.Windows.Forms.Label lblLlave;
        private System.Windows.Forms.Label lblTituloConsultas;
        private System.Windows.Forms.Label lblTituloPagos;
        private System.Windows.Forms.Label lblIdentificacionpagos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPagar;
        private System.Windows.Forms.TextBox txtIdentificacionPagos;
        private System.Windows.Forms.TextBox txtServicioPagos;
        private System.Windows.Forms.TextBox txtLlavePagos;
        private System.Windows.Forms.TextBox txtNumServicioPago;
        private System.Windows.Forms.TextBox txtCodigoSeguridadPago;
    }
}

