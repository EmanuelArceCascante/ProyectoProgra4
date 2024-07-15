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
        print("Respuesta del servidor:", respuesta.decode('utf-8'))

    finally:
        # Cerrar la conexión
        client_socket.close()

# Ruta del archivo JSON
ruta_archivo = 'C:\\Users\\50687\\Documents\\CUC\\PROGRAMACION 4\\PROYECTO\\trama2.txt'
print (ruta_archivo)
# Cargar la trama desde un archivo JSON
#with open('C:\\Users\\50687\\Documents\\CUC\\PROGRAMACION 4\\CLIENTE_SERVIDOR_2DOINTENTO\\trama.json') as f:
with open(ruta_archivo, 'r') as f:
    trama_texto = f.read()
print(trama_texto)

# Convertir la trama JSON a texto plano según el formato requerido
#trama_texto = f"{trama_json['identificacion']:12}{trama_json['servicio']:10}{trama_json['llave']:20}"

# Enviar la trama al servidor
enviar_trama(trama_texto)