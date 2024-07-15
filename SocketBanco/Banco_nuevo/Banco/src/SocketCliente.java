import java.io.BufferedReader;
import java.io.File;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

public class SocketCliente {
    public static void main(String[] args) {
        String url = "jdbc:mysql://localhost:3306/bancopagos";
        String user = "root";
        String password = "Mena200129+";
        String serverAddress = "localhost"; // Dirección IP del servidor
        int port = 5000; // Puerto en el que escucha el servidor

        try {
            // Conexión a la base de datos
            Connection conn = DriverManager.getConnection(url, user, password);
            String query = "SELECT * FROM Datos WHERE identificacionCliente = ?";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, "202220222");

            ResultSet rs = stmt.executeQuery();

            if (rs.next()) {
                // Creación de XML
                DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
                DocumentBuilder builder = factory.newDocumentBuilder();
                Document doc = builder.newDocument();

                Element rootElement = doc.createElement("pago");
                doc.appendChild(rootElement);

                Element identificacion = doc.createElement("identificacion");
                identificacion.appendChild(doc.createTextNode(rs.getString("identificacionCliente")));
                rootElement.appendChild(identificacion);

                Element tipo = doc.createElement("tipo");
                tipo.appendChild(doc.createTextNode(rs.getString("tipoPago")));
                rootElement.appendChild(tipo);

                Element monto = doc.createElement("monto");
                monto.appendChild(doc.createTextNode(rs.getString("montoPago")));
                rootElement.appendChild(monto);

                Element codigo = doc.createElement("codigo");
                codigo.appendChild(doc.createTextNode(rs.getString("codigoAutorizacion")));
                rootElement.appendChild(codigo);

                // Guarda el XML en la ruta especificada
                TransformerFactory transformerFactory = TransformerFactory.newInstance();
                Transformer transformer = transformerFactory.newTransformer();
                DOMSource source = new DOMSource(doc);
                StreamResult result = new StreamResult(new File("C:\\Dahiana Solano\\pago.xml"));
                transformer.transform(source, result);

                System.out.println("XML generado y guardado con éxito en C:\\Dahiana Solano\\pago.xml");

                // Enviar el XML al servidor
                try (Socket socket = new Socket(serverAddress, port);
                     PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
                     BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()))) {

                    String xmlContent = new String(Files.readAllBytes(Paths.get("C:\\Dahiana Solano\\pago.xml")));
                    System.out.println("Contenido del XML enviado: " + xmlContent);
                    out.println(xmlContent); // Enviar contenido XML

                    String serverResponse = in.readLine();
                    System.out.println("Respuesta del servidor: " + serverResponse);
                }

                System.out.println("XML enviado con éxito.");
            } else {
                System.out.println("No se encontraron datos.");
            }

            rs.close();
            stmt.close();
            conn.close();

        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
