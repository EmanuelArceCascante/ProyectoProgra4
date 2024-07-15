import java.io.BufferedReader;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.io.StringReader;
import java.io.StringWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.xml.sax.InputSource;

public class SocketServidor {
    public static void main(String[] args) {
        
        int port = 5000;
        String url = "jdbc:mysql://localhost:3306/bancopagos";
        String user = "root";
        String password = "123456789";
        String filePath = "D:\\.Proyecto1PrograIV\\Entregable1ProyectoProgramacion4\\SocketBanco\\Banco_nuevo\\Banco";
        
                
                
        /*    int port = 1433; // Puerto predeterminado para SQL Server
            String url = "jdbc:sqlserver://localhost:" + port + ";databaseName=bancopagos";
            String user = "DESKTOP-8VDFIGA";
            String password = "123456";
            String filePath = "C:\\Users\\50687\\Documents\\CUC\\PROGRAMACION 4\\AVANCES_PROYECTOFINAL\\Entregable1ProyectoProgramacion4\\Banco_nuevo";
          */      
               
        try (ServerSocket serverSocket = new ServerSocket(port);
             Connection dbConnection = DriverManager.getConnection(url, user, password)) {

            while (true) {
                System.out.println("Esperando conexión del cliente...");
                System.out.println("Hola");

                try (Socket clientSocket = serverSocket.accept();
                     PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true);
                     BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()))) {
                    
                    StringBuilder xmlData = new StringBuilder();
                    String inputLine;
                    while ((inputLine = in.readLine()) != null) {
                        xmlData.append(inputLine);
                        if (!inputLine.contains("</pago>")) { // Verificar el delimitador para el final del XML
                            break;
                        }
                    }
                    
                    String xmlContent = xmlData.toString();
                    System.out.println("Contenido del XML recibido: " + xmlContent);

                    try {
                        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
                        DocumentBuilder builder = factory.newDocumentBuilder();
                        Document doc = builder.parse(new InputSource(new StringReader(xmlContent)));

                        // Extraer elementos del XML
                        String clientId = doc.getElementsByTagName("identificacion").item(0).getTextContent();
                        String tipoPago = doc.getElementsByTagName("tipo").item(0).getTextContent();
                        String codigoAutorizacion = doc.getElementsByTagName("codigo").item(0).getTextContent();
                        String montoStr = doc.getElementsByTagName("monto").item(0).getTextContent();
                        double amount = Double.parseDouble(montoStr);

                        // Validación del monto
                        if (amount <= 0) {
                            String errorXml = createErrorXml("Monto incorrecto");
                            saveXmlToFile(errorXml, filePath);
                            out.println(errorXml);
                        } else {
                            // Validar existencia del cliente
                            String clienteQuery = "SELECT idCliente FROM Clientes WHERE identificacion = ?";
                            try (PreparedStatement clienteStmt = dbConnection.prepareStatement(clienteQuery)) {
                                clienteStmt.setString(1, clientId);
                                ResultSet clienteRs = clienteStmt.executeQuery();
                                if (clienteRs.next()) {
                                    int idCliente = clienteRs.getInt("idCliente");

                                    // Verificar existencia de la cuenta y rebajar saldo
                                    String cuentaQuery = "SELECT saldo FROM Cuentas WHERE numeroCuenta = ? AND idCliente = ?";
                                    try (PreparedStatement cuentaStmt = dbConnection.prepareStatement(cuentaQuery)) {
                                        cuentaStmt.setString(1, codigoAutorizacion);
                                        cuentaStmt.setInt(2, idCliente);
                                        ResultSet cuentaRs = cuentaStmt.executeQuery();
                                        if (cuentaRs.next()) {
                                            double saldo = cuentaRs.getDouble("saldo");
                                            if (saldo >= amount) {
                                                String updateSaldo = "UPDATE Cuentas SET saldo = saldo - ? WHERE numeroCuenta = ? AND idCliente = ?";
                                                try (PreparedStatement updateStmt = dbConnection.prepareStatement(updateSaldo)) {
                                                    updateStmt.setDouble(1, amount);
                                                    updateStmt.setString(2, codigoAutorizacion);
                                                    updateStmt.setInt(3, idCliente);
                                                    updateStmt.executeUpdate();
                                                    String successXml = createSuccessXml("<OK/>");
                                                    saveXmlToFile(successXml, filePath);
                                                    out.println(successXml);
                                                }
                                            } else {
                                                String errorXml = createErrorXml("Fondos insuficientes");
                                                saveXmlToFile(errorXml, filePath);
                                                out.println(errorXml);
                                            }
                                        } else {
                                            String errorXml = createErrorXml("Cuenta no existe");
                                            saveXmlToFile(errorXml, filePath);
                                            out.println(errorXml);
                                        }
                                    }
                                } else {
                                    String errorXml = createErrorXml("Cliente no existe");
                                    saveXmlToFile(errorXml, filePath);
                                    out.println(errorXml);
                                }
                            }
                        }
                    } catch (Exception e) {
                        String errorXml = createErrorXml("Ha ocurrido un error intente de nuevo");
                        saveXmlToFile(errorXml, filePath);
                        out.println(errorXml);
                        //System.out.println("Error al procesar el XML o durante la conexión a la base de datos: " + e.getMessage());
                    }
                } catch (IOException e) {
                    //System.out.println("Error al comunicarse con el cliente: " + e.getMessage());
                }
            }
        } catch (IOException e) {
            System.out.println("No se pudo iniciar el servidor: " + e.getMessage());
        } catch (SQLException e) {
            System.out.println("Error de conexión a la base de datos: " + e.getMessage());
        }
    }

    // Método para crear un mensaje de error en formato XML
    private static String createErrorXml(String errorMessage) {
        try {
            DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
            DocumentBuilder builder = factory.newDocumentBuilder();
            Document doc = builder.newDocument();

            Element rootElement = doc.createElement("respuesta");
            doc.appendChild(rootElement);

            Element status = doc.createElement("status");
            status.appendChild(doc.createTextNode("error"));
            rootElement.appendChild(status);

            Element message = doc.createElement("mensaje");
            message.appendChild(doc.createTextNode(errorMessage));
            rootElement.appendChild(message);

            return transformDocumentToString(doc);
        } catch (ParserConfigurationException | TransformerException e) {
            e.printStackTrace();
            return "<respuesta><status>error</status><mensaje>Error al generar el mensaje XML</mensaje></respuesta>";
        }
    }

    // Método para crear un mensaje de éxito en formato XML
    private static String createSuccessXml(String successMessage) {
        try {
            DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
            DocumentBuilder builder = factory.newDocumentBuilder();
            Document doc = builder.newDocument();

            Element rootElement = doc.createElement("respuesta");
            doc.appendChild(rootElement);

            Element status = doc.createElement("status");
            status.appendChild(doc.createTextNode("exito"));
            rootElement.appendChild(status);

            Element message = doc.createElement("mensaje");
            message.appendChild(doc.createTextNode(successMessage));
            rootElement.appendChild(message);

            return transformDocumentToString(doc);
        } catch (ParserConfigurationException | TransformerException e) {
            e.printStackTrace();
            return "<respuesta><status>error</status><mensaje>Error al generar el mensaje XML</mensaje></respuesta>";
        }
    }

    // Método para transformar un documento XML a String
    private static String transformDocumentToString(Document doc) throws TransformerException {
        TransformerFactory transformerFactory = TransformerFactory.newInstance();
        Transformer transformer = transformerFactory.newTransformer();
        transformer.setOutputProperty(OutputKeys.OMIT_XML_DECLARATION, "no");
        transformer.setOutputProperty(OutputKeys.METHOD, "xml");
        transformer.setOutputProperty(OutputKeys.INDENT, "yes");
        transformer.setOutputProperty(OutputKeys.ENCODING, "UTF-8");

        DOMSource source = new DOMSource(doc);
        StringWriter writer = new StringWriter();
        StreamResult result = new StreamResult(writer);
        transformer.transform(source, result);
        return writer.toString();
    }

    // Método para guardar el XML en un archivo
    private static void saveXmlToFile(String xmlContent, String filePath) {
        try (FileWriter writer = new FileWriter(new File(filePath))) {
            writer.write(xmlContent);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}

