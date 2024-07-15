import socket
import json

def enviar_trama(trama):
    # Crear un socket TCP/IP
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # Conectar el socket al servidor
    server_address = ('localhost', 12345)  # Cambia esto por la dirección y el puerto del servidor
    client_socket.connect(server_address)

    try:
        # Enviar la trama al servidor
        client_socket.sendall(trama.encode('utf-8'))

        # Recibir la respuesta del servidor
        respuesta = client_socket.recv(1024)
        print("Respuesta del servidor: ", respuesta.decode('utf-8'))

    finally:
        # Cerrar la conexión
        client_socket.close()

# Ruta del archivo JSON
ruta_archivo = 'C:\\Users\\50687\\Documents\\CUC\\PROGRAMACION 4\\PROYECTO\\recibo2.json'
#print (ruta_archivo)
# Cargar la trama desde un archivo JSON
#with open('C:\\Users\\50687\\Documents\\CUC\\PROGRAMACION 4\\CLIENTE_SERVIDOR_2DOINTENTO\\recibo.json') as f:
with open(ruta_archivo, 'r') as f:
    trama_texto = f.read()
    #trama_json = json.load(f)
    #trama_texto = f.read().strip()
#print(trama_texto)
identificacion = trama_texto[:12]
servicio = trama_texto[12:22]
Numerorecibo = trama_texto[22:]
# Convertir la trama JSON a texto plano según el formato requerido
#trama_texto = f"{trama_json['identificacion']:12}{trama_json['servicio']:10}{trama_json['Numerorecibo']:10}"
trama_texto = f"{trama_texto[:12]:12}{trama_texto[12:22]:10}{trama_texto[22:]:10}"

# Enviar la trama al servidor
enviar_trama(trama_texto)