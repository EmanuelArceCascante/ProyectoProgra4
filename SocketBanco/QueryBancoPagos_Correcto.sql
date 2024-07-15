use bancopagos;

CREATE TABLE Clientes (
    idCliente INT AUTO_INCREMENT PRIMARY KEY,
    identificacion VARCHAR(20) UNIQUE NOT NULL,
    nombreCompleto VARCHAR(100) NOT NULL,
    correoElectronico VARCHAR(100),
    estado BOOLEAN NOT NULL
);


CREATE TABLE Cuentas (
    idCuenta INT AUTO_INCREMENT PRIMARY KEY,
    idCliente INT,
    numeroCuenta VARCHAR(20) UNIQUE NOT NULL,
    saldo DECIMAL(10, 2) NOT NULL,
    tipoCuenta VARCHAR(50),
    FOREIGN KEY (idCliente) REFERENCES Clientes(idCliente)
);

CREATE TABLE Tarjetas (
    idTarjeta INT AUTO_INCREMENT PRIMARY KEY,
    idCliente INT,
    numeroTarjeta VARCHAR(20) UNIQUE NOT NULL,
    fechaExpiracion DATE NOT NULL,
    codigoSeguridad VARCHAR(4),
    tipo VARCHAR(10),
    FOREIGN KEY (idCliente) REFERENCES Clientes(idCliente)
);

select * from clientes;
select * from cuentas;
select * from tarjetas;

CREATE TABLE Datos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    identificacionCliente VARCHAR(20) NOT NULL,
    tipoPago ENUM('CTA', 'TJTA') NOT NULL,
    montoPago DECIMAL(10, 2) NOT NULL,
    codigoAutorizacion VARCHAR(20) NOT NULL
);

INSERT INTO Datos (identificacionCliente, tipoPago, montoPago, codigoAutorizacion)
SELECT 
    c.identificacion,
    CASE 
        WHEN t.numeroTarjeta IS NOT NULL THEN 'TJTA'
        ELSE 'CTA'
    END AS tipoPago,
    0.00,  -- Asigna el monto del pago a 0.00 inicialmente
    COALESCE(t.numeroTarjeta, cu.numeroCuenta) AS codigoAutorizacion
FROM Clientes c
LEFT JOIN Cuentas cu ON c.idCliente = cu.idCliente
LEFT JOIN Tarjetas t ON c.idCliente = t.idCliente
WHERE cu.numeroCuenta IS NOT NULL OR t.numeroTarjeta IS NOT NULL;

DELIMITER //

CREATE TRIGGER AfterCuentasInsert
AFTER INSERT ON Cuentas
FOR EACH ROW
BEGIN
    INSERT INTO Datos (identificacionCliente, tipoPago, montoPago, codigoAutorizacion)
    SELECT 
        c.identificacion,
        'CTA',
        NEW.saldo,  -- Inicialmente asignamos el saldo como monto del pago
        NEW.numeroCuenta
    FROM Clientes c
    WHERE c.idCliente = NEW.idCliente;
END //

DELIMITER ;
DELIMITER //

CREATE TRIGGER AfterTarjetasInsert
AFTER INSERT ON Tarjetas
FOR EACH ROW
BEGIN
    INSERT INTO Datos (identificacionCliente, tipoPago, montoPago, codigoAutorizacion)
    SELECT 
        c.identificacion,
        'TJTA',
        0.00,  -- Inicialmente asignamos el monto del pago a 0.00
        NEW.numeroTarjeta
    FROM Clientes c
    WHERE c.idCliente = NEW.idCliente;
END //

DELIMITER ;
DELIMITER //

CREATE EVENT UpdateCuentasSaldo
ON SCHEDULE EVERY 1 MINUTE
DO
BEGIN
    UPDATE Cuentas c
    JOIN Datos d ON c.numeroCuenta = d.codigoAutorizacion
    SET c.saldo = c.saldo - d.montoPago
    WHERE d.tipoPago = 'CTA';
END //

DELIMITER ;
DELIMITER //

CREATE EVENT UpdateCuentasSaldo1
ON SCHEDULE EVERY 1 MINUTE
DO
BEGIN
    -- Actualizar saldo para pagos con cuenta
    UPDATE Cuentas c
    JOIN Datos d ON c.numeroCuenta = d.codigoAutorizacion
    SET c.saldo = c.saldo - d.montoPago
    WHERE d.tipoPago = 'CTA';

    -- Actualizar saldo para pagos con tarjeta
    UPDATE Cuentas c
    JOIN Clientes cl ON c.idCliente = cl.idCliente
    JOIN Tarjetas t ON cl.idCliente = t.idCliente
    JOIN Datos d ON t.numeroTarjeta = d.codigoAutorizacion
    SET c.saldo = c.saldo - d.montoPago
    WHERE d.tipoPago = 'TJTA';
END //

DELIMITER ;
