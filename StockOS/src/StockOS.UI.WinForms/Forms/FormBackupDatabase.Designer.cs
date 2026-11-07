namespace StockOS.UI.WinForms.Forms
{
    partial class FormBackupDatabase
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblSubtitulo = new System.Windows.Forms.Label();
            btnCerrar = new System.Windows.Forms.Button();
            pnlBackupCard = new RoundedPanel();
            lblBackupTitulo = new System.Windows.Forms.Label();
            lblBackupDesc = new System.Windows.Forms.Label();
            lblPeriodoLabel = new System.Windows.Forms.Label();
            btnPeriodoDia = new System.Windows.Forms.Button();
            btnPeriodoSemana = new System.Windows.Forms.Button();
            btnPeriodoMes = new System.Windows.Forms.Button();
            btnPeriodoAnio = new System.Windows.Forms.Button();
            lblInfoPeriodo = new System.Windows.Forms.Label();
            lblRutaDestino = new System.Windows.Forms.Label();
            txtRutaDestino = new System.Windows.Forms.TextBox();
            btnExaminarDestino = new System.Windows.Forms.Button();
            chkProgramarAuto = new System.Windows.Forms.CheckBox();
            dtpHoraProgramada = new System.Windows.Forms.DateTimePicker();
            btnGuardarProgramacion = new System.Windows.Forms.Button();
            btnGenerarBackup = new System.Windows.Forms.Button();
            lblBackupEstado = new System.Windows.Forms.Label();
            pnlRestoreCard = new RoundedPanel();
            lblRestoreTitulo = new System.Windows.Forms.Label();
            lblRestoreDesc = new System.Windows.Forms.Label();
            lblAdvertencia = new System.Windows.Forms.Label();
            lblRutaOrigen = new System.Windows.Forms.Label();
            txtRutaOrigen = new System.Windows.Forms.TextBox();
            btnExaminarOrigen = new System.Windows.Forms.Button();
            btnRestaurarBackup = new System.Windows.Forms.Button();
            lblRestoreEstado = new System.Windows.Forms.Label();
            pnlHeader.SuspendLayout();
            pnlBackupCard.SuspendLayout();
            pnlRestoreCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Controls.Add(btnCerrar);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(780, 75);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(20, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(434, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🛠️ Copias de Seguridad y Respaldo (SQL Server)";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblSubtitulo.Location = new System.Drawing.Point(23, 43);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(510, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Gestión de respaldos por períodos (Día, Semana, Mes, Año) y restauración de StockOS";
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCerrar.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrar.ForeColor = System.Drawing.Color.White;
            btnCerrar.Location = new System.Drawing.Point(675, 18);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new System.Drawing.Size(85, 38);
            btnCerrar.TabIndex = 2;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // pnlBackupCard
            // 
            pnlBackupCard.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlBackupCard.BorderRadius = 14;
            pnlBackupCard.Controls.Add(lblBackupTitulo);
            pnlBackupCard.Controls.Add(lblBackupDesc);
            pnlBackupCard.Controls.Add(lblPeriodoLabel);
            pnlBackupCard.Controls.Add(btnPeriodoDia);
            pnlBackupCard.Controls.Add(btnPeriodoSemana);
            pnlBackupCard.Controls.Add(btnPeriodoMes);
            pnlBackupCard.Controls.Add(btnPeriodoAnio);
            pnlBackupCard.Controls.Add(lblInfoPeriodo);
            pnlBackupCard.Controls.Add(lblRutaDestino);
            pnlBackupCard.Controls.Add(txtRutaDestino);
            pnlBackupCard.Controls.Add(btnExaminarDestino);
            pnlBackupCard.Controls.Add(chkProgramarAuto);
            pnlBackupCard.Controls.Add(dtpHoraProgramada);
            pnlBackupCard.Controls.Add(btnGuardarProgramacion);
            pnlBackupCard.Controls.Add(btnGenerarBackup);
            pnlBackupCard.Controls.Add(lblBackupEstado);
            pnlBackupCard.Location = new System.Drawing.Point(20, 90);
            pnlBackupCard.Name = "pnlBackupCard";
            pnlBackupCard.Size = new System.Drawing.Size(740, 325);
            pnlBackupCard.TabIndex = 1;
            // 
            // lblBackupTitulo
            // 
            lblBackupTitulo.AutoSize = true;
            lblBackupTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblBackupTitulo.ForeColor = System.Drawing.Color.White;
            lblBackupTitulo.Location = new System.Drawing.Point(20, 14);
            lblBackupTitulo.Name = "lblBackupTitulo";
            lblBackupTitulo.Size = new System.Drawing.Size(375, 21);
            lblBackupTitulo.TabIndex = 0;
            lblBackupTitulo.Text = "💾 Generar Copia de Seguridad por Período";
            // 
            // lblBackupDesc
            // 
            lblBackupDesc.AutoSize = true;
            lblBackupDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblBackupDesc.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            lblBackupDesc.Location = new System.Drawing.Point(22, 39);
            lblBackupDesc.Name = "lblBackupDesc";
            lblBackupDesc.Size = new System.Drawing.Size(610, 15);
            lblBackupDesc.TabIndex = 1;
            lblBackupDesc.Text = "Seleccione el período del respaldo para organizar las copias y aplicar la política de retención correspondiente:";
            // 
            // lblPeriodoLabel
            // 
            lblPeriodoLabel.AutoSize = true;
            lblPeriodoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblPeriodoLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblPeriodoLabel.Location = new System.Drawing.Point(22, 64);
            lblPeriodoLabel.Name = "lblPeriodoLabel";
            lblPeriodoLabel.Size = new System.Drawing.Size(134, 15);
            lblPeriodoLabel.TabIndex = 2;
            lblPeriodoLabel.Text = "Período de Respaldo:";
            // 
            // btnPeriodoDia
            // 
            btnPeriodoDia.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnPeriodoDia.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPeriodoDia.FlatAppearance.BorderSize = 1;
            btnPeriodoDia.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(96, 165, 250);
            btnPeriodoDia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPeriodoDia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnPeriodoDia.ForeColor = System.Drawing.Color.White;
            btnPeriodoDia.Location = new System.Drawing.Point(24, 85);
            btnPeriodoDia.Name = "btnPeriodoDia";
            btnPeriodoDia.Size = new System.Drawing.Size(160, 34);
            btnPeriodoDia.TabIndex = 3;
            btnPeriodoDia.Text = "📅 Por Día";
            btnPeriodoDia.UseVisualStyleBackColor = false;
            btnPeriodoDia.Click += btnPeriodoDia_Click;
            // 
            // btnPeriodoSemana
            // 
            btnPeriodoSemana.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            btnPeriodoSemana.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPeriodoSemana.FlatAppearance.BorderSize = 1;
            btnPeriodoSemana.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnPeriodoSemana.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPeriodoSemana.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnPeriodoSemana.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            btnPeriodoSemana.Location = new System.Drawing.Point(198, 85);
            btnPeriodoSemana.Name = "btnPeriodoSemana";
            btnPeriodoSemana.Size = new System.Drawing.Size(160, 34);
            btnPeriodoSemana.TabIndex = 4;
            btnPeriodoSemana.Text = "📆 Por Semana";
            btnPeriodoSemana.UseVisualStyleBackColor = false;
            btnPeriodoSemana.Click += btnPeriodoSemana_Click;
            // 
            // btnPeriodoMes
            // 
            btnPeriodoMes.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            btnPeriodoMes.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPeriodoMes.FlatAppearance.BorderSize = 1;
            btnPeriodoMes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnPeriodoMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPeriodoMes.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnPeriodoMes.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            btnPeriodoMes.Location = new System.Drawing.Point(372, 85);
            btnPeriodoMes.Name = "btnPeriodoMes";
            btnPeriodoMes.Size = new System.Drawing.Size(160, 34);
            btnPeriodoMes.TabIndex = 5;
            btnPeriodoMes.Text = "🗓️ Por Mes";
            btnPeriodoMes.UseVisualStyleBackColor = false;
            btnPeriodoMes.Click += btnPeriodoMes_Click;
            // 
            // btnPeriodoAnio
            // 
            btnPeriodoAnio.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            btnPeriodoAnio.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPeriodoAnio.FlatAppearance.BorderSize = 1;
            btnPeriodoAnio.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnPeriodoAnio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPeriodoAnio.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnPeriodoAnio.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            btnPeriodoAnio.Location = new System.Drawing.Point(546, 85);
            btnPeriodoAnio.Name = "btnPeriodoAnio";
            btnPeriodoAnio.Size = new System.Drawing.Size(168, 34);
            btnPeriodoAnio.TabIndex = 6;
            btnPeriodoAnio.Text = "📚 Por Año";
            btnPeriodoAnio.UseVisualStyleBackColor = false;
            btnPeriodoAnio.Click += btnPeriodoAnio_Click;
            // 
            // lblInfoPeriodo
            // 
            lblInfoPeriodo.AutoSize = true;
            lblInfoPeriodo.Font = new System.Drawing.Font("Segoe UI", 8.75F, System.Drawing.FontStyle.Italic);
            lblInfoPeriodo.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            lblInfoPeriodo.Location = new System.Drawing.Point(24, 126);
            lblInfoPeriodo.Name = "lblInfoPeriodo";
            lblInfoPeriodo.Size = new System.Drawing.Size(520, 15);
            lblInfoPeriodo.TabIndex = 7;
            lblInfoPeriodo.Text = "📌 Período DIARIO: Almacena en subcarpeta '\\Diario' y conserva los últimos 7 días de copias.";
            // 
            // lblRutaDestino
            // 
            lblRutaDestino.AutoSize = true;
            lblRutaDestino.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblRutaDestino.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblRutaDestino.Location = new System.Drawing.Point(22, 152);
            lblRutaDestino.Name = "lblRutaDestino";
            lblRutaDestino.Size = new System.Drawing.Size(145, 15);
            lblRutaDestino.TabIndex = 8;
            lblRutaDestino.Text = "Carpeta raíz de destino:";
            // 
            // txtRutaDestino
            // 
            txtRutaDestino.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtRutaDestino.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtRutaDestino.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtRutaDestino.ForeColor = System.Drawing.Color.White;
            txtRutaDestino.Location = new System.Drawing.Point(24, 173);
            txtRutaDestino.Name = "txtRutaDestino";
            txtRutaDestino.Size = new System.Drawing.Size(568, 24);
            txtRutaDestino.TabIndex = 9;
            // 
            // btnExaminarDestino
            // 
            btnExaminarDestino.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnExaminarDestino.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExaminarDestino.FlatAppearance.BorderSize = 0;
            btnExaminarDestino.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExaminarDestino.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnExaminarDestino.ForeColor = System.Drawing.Color.White;
            btnExaminarDestino.Location = new System.Drawing.Point(602, 172);
            btnExaminarDestino.Name = "btnExaminarDestino";
            btnExaminarDestino.Size = new System.Drawing.Size(112, 26);
            btnExaminarDestino.TabIndex = 10;
            btnExaminarDestino.Text = "Examinar...";
            btnExaminarDestino.UseVisualStyleBackColor = false;
            btnExaminarDestino.Click += btnExaminarDestino_Click;
            // 
            // chkProgramarAuto
            // 
            chkProgramarAuto.AutoSize = true;
            chkProgramarAuto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            chkProgramarAuto.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            chkProgramarAuto.Location = new System.Drawing.Point(24, 214);
            chkProgramarAuto.Name = "chkProgramarAuto";
            chkProgramarAuto.Size = new System.Drawing.Size(375, 19);
            chkProgramarAuto.TabIndex = 11;
            chkProgramarAuto.Text = "⏰ Activar backup automático para el período elegido a las:";
            chkProgramarAuto.UseVisualStyleBackColor = true;
            // 
            // dtpHoraProgramada
            // 
            dtpHoraProgramada.CustomFormat = "HH:mm";
            dtpHoraProgramada.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dtpHoraProgramada.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpHoraProgramada.Location = new System.Drawing.Point(408, 210);
            dtpHoraProgramada.Name = "dtpHoraProgramada";
            dtpHoraProgramada.ShowUpDown = true;
            dtpHoraProgramada.Size = new System.Drawing.Size(78, 24);
            dtpHoraProgramada.TabIndex = 12;
            // 
            // btnGuardarProgramacion
            // 
            btnGuardarProgramacion.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnGuardarProgramacion.Cursor = System.Windows.Forms.Cursors.Hand;
            btnGuardarProgramacion.FlatAppearance.BorderSize = 0;
            btnGuardarProgramacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarProgramacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnGuardarProgramacion.ForeColor = System.Drawing.Color.White;
            btnGuardarProgramacion.Location = new System.Drawing.Point(500, 209);
            btnGuardarProgramacion.Name = "btnGuardarProgramacion";
            btnGuardarProgramacion.Size = new System.Drawing.Size(214, 28);
            btnGuardarProgramacion.TabIndex = 13;
            btnGuardarProgramacion.Text = "Guardar Programación";
            btnGuardarProgramacion.UseVisualStyleBackColor = false;
            btnGuardarProgramacion.Click += btnGuardarProgramacion_Click;
            // 
            // btnGenerarBackup
            // 
            btnGenerarBackup.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnGenerarBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            btnGenerarBackup.FlatAppearance.BorderSize = 0;
            btnGenerarBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGenerarBackup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnGenerarBackup.ForeColor = System.Drawing.Color.White;
            btnGenerarBackup.Location = new System.Drawing.Point(24, 260);
            btnGenerarBackup.Name = "btnGenerarBackup";
            btnGenerarBackup.Size = new System.Drawing.Size(260, 40);
            btnGenerarBackup.TabIndex = 14;
            btnGenerarBackup.Text = "Crear Backup (Por Día)";
            btnGenerarBackup.UseVisualStyleBackColor = false;
            btnGenerarBackup.Click += btnGenerarBackup_Click;
            // 
            // lblBackupEstado
            // 
            lblBackupEstado.AutoSize = true;
            lblBackupEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            lblBackupEstado.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            lblBackupEstado.Location = new System.Drawing.Point(298, 272);
            lblBackupEstado.Name = "lblBackupEstado";
            lblBackupEstado.Size = new System.Drawing.Size(131, 15);
            lblBackupEstado.TabIndex = 15;
            lblBackupEstado.Text = "Listo para crear respaldo.";
            // 
            // pnlRestoreCard
            // 
            pnlRestoreCard.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlRestoreCard.BorderRadius = 14;
            pnlRestoreCard.Controls.Add(lblRestoreTitulo);
            pnlRestoreCard.Controls.Add(lblRestoreDesc);
            pnlRestoreCard.Controls.Add(lblAdvertencia);
            pnlRestoreCard.Controls.Add(lblRutaOrigen);
            pnlRestoreCard.Controls.Add(txtRutaOrigen);
            pnlRestoreCard.Controls.Add(btnExaminarOrigen);
            pnlRestoreCard.Controls.Add(btnRestaurarBackup);
            pnlRestoreCard.Controls.Add(lblRestoreEstado);
            pnlRestoreCard.Location = new System.Drawing.Point(20, 430);
            pnlRestoreCard.Name = "pnlRestoreCard";
            pnlRestoreCard.Size = new System.Drawing.Size(740, 225);
            pnlRestoreCard.TabIndex = 2;
            // 
            // lblRestoreTitulo
            // 
            lblRestoreTitulo.AutoSize = true;
            lblRestoreTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblRestoreTitulo.ForeColor = System.Drawing.Color.White;
            lblRestoreTitulo.Location = new System.Drawing.Point(20, 14);
            lblRestoreTitulo.Name = "lblRestoreTitulo";
            lblRestoreTitulo.Size = new System.Drawing.Size(268, 21);
            lblRestoreTitulo.TabIndex = 0;
            lblRestoreTitulo.Text = "🔄 Restaurar Base de Datos";
            // 
            // lblRestoreDesc
            // 
            lblRestoreDesc.AutoSize = true;
            lblRestoreDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblRestoreDesc.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            lblRestoreDesc.Location = new System.Drawing.Point(22, 40);
            lblRestoreDesc.Name = "lblRestoreDesc";
            lblRestoreDesc.Size = new System.Drawing.Size(564, 15);
            lblRestoreDesc.TabIndex = 1;
            lblRestoreDesc.Text = "Restaura el estado completo del sistema a partir de un archivo .bak previamente generado en SQL Server.";
            // 
            // lblAdvertencia
            // 
            lblAdvertencia.AutoSize = true;
            lblAdvertencia.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblAdvertencia.ForeColor = System.Drawing.Color.FromArgb(248, 113, 113);
            lblAdvertencia.Location = new System.Drawing.Point(22, 63);
            lblAdvertencia.Name = "lblAdvertencia";
            lblAdvertencia.Size = new System.Drawing.Size(657, 15);
            lblAdvertencia.TabIndex = 2;
            lblAdvertencia.Text = "⚠️ ATENCIÓN: La restauración sobrescribirá todos los datos actuales. Asegúrese de realizar un backup previo.";
            // 
            // lblRutaOrigen
            // 
            lblRutaOrigen.AutoSize = true;
            lblRutaOrigen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblRutaOrigen.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblRutaOrigen.Location = new System.Drawing.Point(22, 90);
            lblRutaOrigen.Name = "lblRutaOrigen";
            lblRutaOrigen.Size = new System.Drawing.Size(161, 15);
            lblRutaOrigen.TabIndex = 3;
            lblRutaOrigen.Text = "Archivo de respaldo (.bak):";
            // 
            // txtRutaOrigen
            // 
            txtRutaOrigen.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtRutaOrigen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtRutaOrigen.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtRutaOrigen.ForeColor = System.Drawing.Color.White;
            txtRutaOrigen.Location = new System.Drawing.Point(24, 111);
            txtRutaOrigen.Name = "txtRutaOrigen";
            txtRutaOrigen.Size = new System.Drawing.Size(568, 24);
            txtRutaOrigen.TabIndex = 4;
            // 
            // btnExaminarOrigen
            // 
            btnExaminarOrigen.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            btnExaminarOrigen.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExaminarOrigen.FlatAppearance.BorderSize = 0;
            btnExaminarOrigen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExaminarOrigen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnExaminarOrigen.ForeColor = System.Drawing.Color.White;
            btnExaminarOrigen.Location = new System.Drawing.Point(602, 110);
            btnExaminarOrigen.Name = "btnExaminarOrigen";
            btnExaminarOrigen.Size = new System.Drawing.Size(112, 26);
            btnExaminarOrigen.TabIndex = 5;
            btnExaminarOrigen.Text = "Examinar...";
            btnExaminarOrigen.UseVisualStyleBackColor = false;
            btnExaminarOrigen.Click += btnExaminarOrigen_Click;
            // 
            // btnRestaurarBackup
            // 
            btnRestaurarBackup.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            btnRestaurarBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRestaurarBackup.FlatAppearance.BorderSize = 0;
            btnRestaurarBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRestaurarBackup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRestaurarBackup.ForeColor = System.Drawing.Color.White;
            btnRestaurarBackup.Location = new System.Drawing.Point(24, 155);
            btnRestaurarBackup.Name = "btnRestaurarBackup";
            btnRestaurarBackup.Size = new System.Drawing.Size(220, 38);
            btnRestaurarBackup.TabIndex = 6;
            btnRestaurarBackup.Text = "Restaurar Base de Datos";
            btnRestaurarBackup.UseVisualStyleBackColor = false;
            btnRestaurarBackup.Click += btnRestaurarBackup_Click;
            // 
            // lblRestoreEstado
            // 
            lblRestoreEstado.AutoSize = true;
            lblRestoreEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            lblRestoreEstado.ForeColor = System.Drawing.Color.FromArgb(248, 113, 113);
            lblRestoreEstado.Location = new System.Drawing.Point(260, 166);
            lblRestoreEstado.Name = "lblRestoreEstado";
            lblRestoreEstado.Name = "lblRestoreEstado";
            lblRestoreEstado.Size = new System.Drawing.Size(171, 15);
            lblRestoreEstado.TabIndex = 7;
            lblRestoreEstado.Text = "Seleccione un archivo de copia.";
            // 
            // FormBackupDatabase
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            ClientSize = new System.Drawing.Size(780, 675);
            Controls.Add(pnlRestoreCard);
            Controls.Add(pnlBackupCard);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBackupDatabase";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Administración de Copias de Seguridad";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBackupCard.ResumeLayout(false);
            pnlBackupCard.PerformLayout();
            pnlRestoreCard.ResumeLayout(false);
            pnlRestoreCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Button btnCerrar;
        private RoundedPanel pnlBackupCard;
        private System.Windows.Forms.Label lblBackupTitulo;
        private System.Windows.Forms.Label lblBackupDesc;
        private System.Windows.Forms.Label lblPeriodoLabel;
        private System.Windows.Forms.Button btnPeriodoDia;
        private System.Windows.Forms.Button btnPeriodoSemana;
        private System.Windows.Forms.Button btnPeriodoMes;
        private System.Windows.Forms.Button btnPeriodoAnio;
        private System.Windows.Forms.Label lblInfoPeriodo;
        private System.Windows.Forms.Label lblRutaDestino;
        private System.Windows.Forms.TextBox txtRutaDestino;
        private System.Windows.Forms.Button btnExaminarDestino;
        private System.Windows.Forms.CheckBox chkProgramarAuto;
        private System.Windows.Forms.DateTimePicker dtpHoraProgramada;
        private System.Windows.Forms.Button btnGuardarProgramacion;
        private System.Windows.Forms.Button btnGenerarBackup;
        private System.Windows.Forms.Label lblBackupEstado;
        private RoundedPanel pnlRestoreCard;
        private System.Windows.Forms.Label lblRestoreTitulo;
        private System.Windows.Forms.Label lblRestoreDesc;
        private System.Windows.Forms.Label lblAdvertencia;
        private System.Windows.Forms.Label lblRutaOrigen;
        private System.Windows.Forms.TextBox txtRutaOrigen;
        private System.Windows.Forms.Button btnExaminarOrigen;
        private System.Windows.Forms.Button btnRestaurarBackup;
        private System.Windows.Forms.Label lblRestoreEstado;
    }
}
