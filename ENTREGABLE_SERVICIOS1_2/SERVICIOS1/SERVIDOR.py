import socket
import pymongo
import json
client = pymongo.MongoClient("mongodb://localhost:27017/")
database = client["SocketServiciosBD"]
clientes_collection = database["Sertvicios"]

def procesar_trama(trama):
    
    if len(trama) != 42:
        return "ERROR: La trama no tiene el formato correcto"

    identificacion = trama[:12].strip()
    servicio = trama[12:22].strip()
    llave = (trama[22:].strip())

    
    if not all((identificacion, servicio, llave)):
        return "ERROR: No hay datos"

    
    monto = consultar_monto(identificacion, servicio, llave)
    return monto


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
        

server_address = ('localhost', 8888)  
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(server_address)
server_socket.listen(10)

print("Esperando conexiones...")

while True:
    client_socket, client_address = server_socket.accept()
    print("Conexión establecida desde:", client_address)

    try:
       
        trama = client_socket.recv(1024).decode('utf-8')

        TramaCorregida = trama
        print("Trama recibida:", TramaCorregida)

        respuesta = procesar_trama(trama)

        client_socket.sendall(respuesta.encode('utf-8'))

    finally:
        
        client_socket.close()