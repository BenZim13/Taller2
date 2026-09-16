import os
import re

forms_dir = r"C:\Proyecto\StockOS\src\StockOS.UI.WinForms\Forms"

skip_files = ["FormInicio.Designer.cs", "UcInventario.Designer.cs", "UcListarUsuarios.Designer.cs", "UcUsuarios.Designer.cs"]

for filename in os.listdir(forms_dir):
    if not filename.endswith(".Designer.cs") or filename in skip_files:
        continue
        
    filepath = os.path.join(forms_dir, filename)
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    controls = []
    for line in content.splitlines():
        if "new System.Windows.Forms.TextBox()" in line or "new TextBox()" in line or \
           "new System.Windows.Forms.ComboBox()" in line or "new ComboBox()" in line or \
           "new System.Windows.Forms.NumericUpDown()" in line or "new NumericUpDown()" in line:
            match = re.search(r'(this\.[a-zA-Z0-9_]+| [a-zA-Z0-9_]+)\s*=\s*new', line)
            if match:
                var_name = match.group(1).strip()
                controls.append(var_name)
    
    # Remove duplicates
    controls = list(set(controls))

    for c in controls:
        anchor_pattern = r'(' + re.escape(c) + r'\.Anchor\s*=\s*[^;]+;)'
        anchor_prop = c + ".Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));"
        # some use short syntax
        anchor_prop_short = c + ".Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;"
        
        is_short = "AnchorStyles" in content and "System.Windows.Forms.AnchorStyles" not in content

        prop_to_use = anchor_prop_short if is_short else anchor_prop

        if re.search(anchor_pattern, content):
            content = re.sub(anchor_pattern, prop_to_use, content)
        else:
            location_pattern = r'(' + re.escape(c) + r'\.Location\s*=)'
            content = re.sub(location_pattern, prop_to_use + "\n            \\1", content)
            
    # For buttons, anchor to Bottom | Right, except in FormRegistroUsuario where we already added a TableLayoutPanel for bottom buttons
    if "FormRegistroUsuario" not in filename:
        buttons = ["this.btnGuardar", "this.btnCancelar", "this.btnIngresar", "this.btnSalir", "btnGuardar", "btnCancelar", "btnIngresar", "btnSalir"]
        for b in buttons:
            if b in content:
                anchor_pattern = r'(' + re.escape(b) + r'\.Anchor\s*=\s*[^;]+;)'
                anchor_prop = b + ".Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));"
                anchor_prop_short = b + ".Anchor = AnchorStyles.Bottom | AnchorStyles.Right;"
                
                is_short = "AnchorStyles" in content and "System.Windows.Forms.AnchorStyles" not in content
                prop_to_use = anchor_prop_short if is_short else anchor_prop
                
                if re.search(anchor_pattern, content):
                    content = re.sub(anchor_pattern, prop_to_use, content)
                else:
                    location_pattern = r'(' + re.escape(b) + r'\.Location\s*=)'
                    content = re.sub(location_pattern, prop_to_use + "\n            \\1", content)

    with open(filepath, "w", encoding="utf-8") as f:
        f.write(content)

print("Done")

