CREATE DATABASE IF NOT EXISTS bydesigners_db;
USE bydesigners_db;

-- 1. Tabla Padre: Cliente
CREATE TABLE IF NOT EXISTS Cliente (
    id_cli INT AUTO_INCREMENT PRIMARY KEY,
    nom_cli VARCHAR(60) NOT NULL,
    dir_cli VARCHAR(150),
    tel_cli VARCHAR(12),
    email_cli VARCHAR(60),
    fecha_reg DATETIME DEFAULT CURRENT_TIMESTAMP,
    tipo_cliente VARCHAR(60) NOT NULL
);

-- 1.1 Tabla Hija: Cliente_Particular (Herencia)
CREATE TABLE IF NOT EXISTS Cliente_Particular (
    id_cliente INT PRIMARY KEY,
    nombre_completo VARCHAR(60) NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cli) ON DELETE CASCADE
);

-- 1.2 Tabla Hija: Cliente_Empresa (Herencia)
CREATE TABLE IF NOT EXISTS Cliente_Empresa (
    id_cliente INT PRIMARY KEY,
    rfc VARCHAR(13) NOT NULL,
    razon_social VARCHAR(100) NOT NULL,
    contacto_puesto VARCHAR(50),
    FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cli) ON DELETE CASCADE
);

-- 2. Tabla Trabajador
CREATE TABLE IF NOT EXISTS Trabajador (
    id_trab INT AUTO_INCREMENT PRIMARY KEY,
    nom_trab VARCHAR(60) NOT NULL,
    esp_trab VARCHAR(40),
    tel_trab VARCHAR(12),
    email_trab VARCHAR(60),
    fecha_contr DATETIME DEFAULT CURRENT_TIMESTAMP,
    sueldo_base DECIMAL(10,2) NOT NULL
);

-- 3. Tabla Proveedor
CREATE TABLE IF NOT EXISTS Proveedor (
    id_prov INT AUTO_INCREMENT PRIMARY KEY,
    nom_prov VARCHAR(60) NOT NULL,
    tel_prov VARCHAR(12),
    dir_prov VARCHAR(150),
    email_prov VARCHAR(60),
    estatus_prov VARCHAR(10) DEFAULT 'Activo'
);

-- 4. Tabla Padre: Item_Cotizable
CREATE TABLE IF NOT EXISTS Item_Cotizable (
    id_item INT AUTO_INCREMENT PRIMARY KEY,
    nombre_item VARCHAR(60) NOT NULL,
    descripcion TEXT,
    precio_base DECIMAL(10,2) NOT NULL,
    tipo_item VARCHAR(20) NOT NULL,
    stock INT DEFAULT 0,
    fecha_alta DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 4.1 Tabla Hija: Mueble_Medida (Herencia)
CREATE TABLE IF NOT EXISTS Mueble_Medida (
    id_mueble INT AUTO_INCREMENT PRIMARY KEY,
    id_item INT NOT NULL,
    material_principal VARCHAR(40),
    alto DOUBLE,
    ancho DOUBLE,
    profundidad DOUBLE,
    color_acabado VARCHAR(30),
    id_prov INT,
    FOREIGN KEY (id_item) REFERENCES Item_Cotizable(id_item) ON DELETE CASCADE,
    FOREIGN KEY (id_prov) REFERENCES Proveedor(id_prov)
);

-- 4.2 Tabla Hija: Servicio_Mantenimiento (Herencia)
CREATE TABLE IF NOT EXISTS Servicio_Mantenimiento (
    id_item INT PRIMARY KEY,
    categoria VARCHAR(40),
    unidad_cobro VARCHAR(20),
    FOREIGN KEY (id_item) REFERENCES Item_Cotizable(id_item) ON DELETE CASCADE
);

-- 5. Tabla Cotizacion
CREATE TABLE IF NOT EXISTS Cotizacion (
    id_cot INT AUTO_INCREMENT PRIMARY KEY,
    fecha_cot DATETIME DEFAULT CURRENT_TIMESTAMP,
    estado_cot VARCHAR(20) DEFAULT 'Pendiente',
    total_cot DECIMAL(10,2) DEFAULT 0.00,
    id_cli INT NOT NULL,
    id_trab INT NOT NULL,
    obs_cot TEXT,
    FOREIGN KEY (id_cli) REFERENCES Cliente(id_cli),
    FOREIGN KEY (id_trab) REFERENCES Trabajador(id_trab)
);

-- 6. Tabla Detalle_Cotizacion
CREATE TABLE IF NOT EXISTS Detalle_Cotizacion (
    id_detalle INT AUTO_INCREMENT PRIMARY KEY,
    id_cot INT NOT NULL,
    id_item INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unit DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_cot) REFERENCES Cotizacion(id_cot) ON DELETE CASCADE,
    FOREIGN KEY (id_item) REFERENCES Item_Cotizable(id_item)
);

-- 7. Tabla Padre: Pago
CREATE TABLE IF NOT EXISTS Pago (
    id_pago INT AUTO_INCREMENT PRIMARY KEY,
    monto_pago DECIMAL(10,2) NOT NULL,
    fecha_pago DATETIME DEFAULT CURRENT_TIMESTAMP,
    tipo_pago VARCHAR(20) NOT NULL,
    id_cot INT NOT NULL,
    estatus_pago VARCHAR(20) DEFAULT 'Registrado',
    FOREIGN KEY (id_cot) REFERENCES Cotizacion(id_cot)
);

-- 7.1 Tabla Hija: Pago_Transferencia (Herencia)
CREATE TABLE IF NOT EXISTS Pago_Transferencia (
    id_pago INT PRIMARY KEY,
    banco_origen VARCHAR(40),
    clave_rastreo_spei VARCHAR(30),
    FOREIGN KEY (id_pago) REFERENCES Pago(id_pago) ON DELETE CASCADE
);

-- 7.2 Tabla Hija: Pago_Efectivo (Herencia)
CREATE TABLE IF NOT EXISTS Pago_Efectivo (
    id_pago INT PRIMARY KEY,
    recibo_por VARCHAR(60),
    FOREIGN KEY (id_pago) REFERENCES Pago(id_pago) ON DELETE CASCADE
);