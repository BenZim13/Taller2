import re

file_path = r"C:\Proyecto\StockOS\src\StockOS.UI.WinForms\Forms\FormRegistroUsuario.Designer.cs"

with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

# 1. Add tlpBotones declaration
decl_str = "private System.Windows.Forms.TableLayoutPanel tlpBotones;"
if decl_str not in content:
    content = content.replace("private System.Windows.Forms.ComboBox cmbSucursal;", "private System.Windows.Forms.ComboBox cmbSucursal;\n        " + decl_str)
    content = content.replace("private ComboBox cmbSucursal;", "private ComboBox cmbSucursal;\n        " + decl_str)

# 2. Add instantiation in InitializeComponent
init_str = "tlpBotones = new TableLayoutPanel();"
if init_str not in content:
    content = content.replace("cmbSucursal = new ComboBox();", "cmbSucursal = new ComboBox();\n            tlpBotones = new TableLayoutPanel();\n            tlpBotones.SuspendLayout();")

# 4. Modify buttons and setup tlpBotones
button_setup = """
            // 
            // tlpBotones
            // 
            tlpBotones.ColumnCount = 2;
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBotones.Controls.Add(btnGuardar, 0, 0);
            tlpBotones.Controls.Add(btnCancelar, 1, 0);
            tlpBotones.Dock = DockStyle.Bottom;
            tlpBotones.Location = new Point(0, 592);
            tlpBotones.Name = "tlpBotones";
            tlpBotones.RowCount = 1;
            tlpBotones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBotones.Size = new Size(434, 60);
            tlpBotones.TabIndex = 21;
            tlpBotones.Padding = new Padding(30, 0, 30, 15);
"""

# replace btnGuardar properties
content = re.sub(r"btnGuardar\.Anchor = .*?;", "btnGuardar.Anchor = AnchorStyles.Left | AnchorStyles.Right;", content)
content = re.sub(r"btnGuardar\.Location = .*?;", "btnGuardar.Location = new Point(3, 3);", content)
content = re.sub(r"btnGuardar\.Size = .*?;", "btnGuardar.Size = new Size(181, 42);", content)
content = re.sub(r"btnGuardar\.Margin = .*?;", "", content) # remove margin if exists
content = re.sub(r"btnGuardar\.UseVisualStyleBackColor = false;", "btnGuardar.UseVisualStyleBackColor = false;\n            btnGuardar.Margin = new Padding(10, 0, 10, 0);", content)

# replace btnCancelar properties
content = re.sub(r"btnCancelar\.Anchor = .*?;", "btnCancelar.Anchor = AnchorStyles.Left | AnchorStyles.Right;", content)
content = re.sub(r"btnCancelar\.Location = .*?;", "btnCancelar.Location = new Point(190, 3);", content)
content = re.sub(r"btnCancelar\.Size = .*?;", "btnCancelar.Size = new Size(181, 42);", content)
content = re.sub(r"btnCancelar\.Margin = .*?;", "", content) # remove margin if exists
content = re.sub(r"btnCancelar\.UseVisualStyleBackColor = false;", "btnCancelar.UseVisualStyleBackColor = false;\n            btnCancelar.Margin = new Padding(10, 0, 10, 0);", content)

# insert tlpBotones setup before form setup
if "tlpBotones.ColumnCount" not in content:
    content = content.replace("            // FormRegistroUsuario", button_setup + "\n            // FormRegistroUsuario")

# Update Controls.Add
content = content.replace("Controls.Add(btnCancelar);", "")
content = content.replace("Controls.Add(btnGuardar);", "Controls.Add(tlpBotones);")

# Update form height and autoscroll margin to prevent overlap
content = content.replace("AutoScroll = true;", "AutoScroll = true;\n            AutoScrollMargin = new Size(0, 80);")

if "tlpBotones.ResumeLayout" not in content:
    content = content.replace("ResumeLayout(false);", "tlpBotones.ResumeLayout(false);\n            ResumeLayout(false);", 1)

with open(file_path, "w", encoding="utf-8") as f:
    f.write(content)

print("Done")

