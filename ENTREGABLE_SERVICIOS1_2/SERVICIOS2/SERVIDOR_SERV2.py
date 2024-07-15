import socket
import pymongo
import json

# Conexión a la base de datos de MongoDB
client = pymongo.MongoClient("mongodb://localhost:27017/")
database = client["SocketServiciosBD"]
clientes_collection = database["Sertvicios"]

def consultar_monto(identificacion, servicio, llave):
    cliente = clientes_collection.find_one({"identificacion": identificacion, "servicio": servicio, "llave": llave})
    
    print(f"Cliente : {cliente}")  # Depuración

    if cliente:
        # Extracción de los valores directamente del documento
        numero_recibo = str(cliente.get("Numerorecibo", "")).zfill(10)
        Fechavencimiento = str(cliente.get("Fechavencimiento", "")).ljust(10)[:10]
        monto = cliente.get("monto", 0.00)
        monto_str = "{:.2f}".format(monto).replace('.', '').zfill(19)
        
        # Construcción del resultado
        resultado = f"OK[{numero_recibo}{Fechavencimiento}{monto_str}]"
        
        print(f"Resultado final: {resultado}")  # Depuración
        return resultado
    else:
        #print("No se encontró el cliente con la identificación, servicio y llave proporcionados.")  # Depuración
        return "ERROR: No hay datos"
def verificar_existencia_datos(identificacion, servicio, Numerorecibo):
    # Conectar a la base de datos
    client = pymongo.MongoClient("mongodb://localhost:27017/")
    database = client["SocketServiciosBD"]
    clientes_collection = database["Sertvicios"]
    
    # Crear un filtro para buscar documentos que coincidan con los criterios de búsqueda
    filtro = {
        "identificacion": identificacion,
        "servicio": servicio,
        "Numerorecibo": Numerorecibo
    }
    
    # Buscar un documento que coincida con el filtro
    documento = clientes_collection.find_one(filtro)
    
    # Si no se encuentra el documento, devolver un mensaje de error
    if documento is None:
        return "ERROR: No hay datos"
    
    # Devolver True si se encuentra un documento
    return True


# Método para procesar la trama recibida y realizar la consulta del monto a pagar
def procesar_trama(trama_texto):
    # Conexión a la base de datos MongoDB
    client = pymongo.MongoClient("mongodb://localhost:27017/")
    db = client["SocketServiciosBD"]
    clientes_collection = db["Sertvicios"]    
    
    # Verificar que la trama tenga la longitud adecuada
    trama_texto = trama_texto.strip('"')
    if len(trama_texto) != 32 | len(trama_texto)!= 42:  
                
        return "ERROR: La trama no tiene el formato correcto"
    if len(trama_texto) == 42:
       identificacion = trama_texto[:12].strip()
       servicio = trama_texto[12:22].strip()
       llave = (trama_texto[22:].strip())
       monto = consultar_monto(identificacion, servicio, llave)
       return monto
    # Extraer la identificación del cliente, el servicio y la llave de la trama
    identificacion = trama_texto[:12].strip()
    servicio = trama_texto[12:22].strip()
    Numerorecibo = (trama_texto[22:].strip())

    # Verificar que ninguno de los campos esté vacío
    if not all((identificacion, servicio, Numerorecibo)):
        return "ERROR: La trama no tiene el formato correcto"
    
    existe_datos = verificar_existencia_datos(identificacion, servicio, Numerorecibo)
    if existe_datos == "ERROR: No hay datos":
        return existe_datos
    ##Vamos a probar hacer conexion a MONGODB
    filtro = {"identificacion": identificacion, "servicio": servicio, "Numerorecibo": Numerorecibo}
    documento = clientes_collection.find_one(filtro)

    # Verificar si se encontró el documento
    if documento:
        # Actualizar el campo "Estado" a "pagado"
        actualizacion = {"$set": {"Estado": "pagado"}}
        clientes_collection.update_one(filtro, actualizacion)
        
    # Imprimir el estado después de la actualización
        documento_actualizado = clientes_collection.find_one(filtro)
        print("Estado después de la actualización:", documento_actualizado["Estado"])
        return "OK"
    else:
        return "ERROR: Ha ocurrido un error intente de nuevo"

# Crear un socket para recibir la trama
server_address = ('localhost', 14000)  # Cambia esto por la dirección y el puerto adecuado
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(server_address)
server_socket.listen(10)

print("Esperando conexiones...")

while True:
    client_socket, client_address = server_socket.accept()
    
    print("Conexión establecida desde:", client_address)

    try:
        # Recibir la trama del cliente
        trama_texto1 = client_socket.recv(1024).decode('utf-8')
        print("Trama recibida:", trama_texto1)

        # Procesar la trama y obtener la respuesta
        respuesta1 = procesar_trama(trama_texto1)
        if respuesta1 is not None:
            client_socket.sendall(respuesta1.encode('utf-8'))
        else:
            print("Error: La respuesta es None, no se puede enviar.")
        
    finally:   
        # Cerrar la conexión con el cliente
        client_socket.close()