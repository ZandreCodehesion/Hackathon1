
import java.io.IOException;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class App {
  public static void main(String[] args) {
    test();
    String calculateBenchmark = calculateBenchmark(5000);
    System.out.println(calculateBenchmark);
  }

  public static String calculateBenchmark(int timeInMilliseconds) {
    int counter = 0;
    long startTime = System.currentTimeMillis();
    while (System.currentTimeMillis() - startTime < timeInMilliseconds) {

      counter++;
    }
    long endTime = System.currentTimeMillis();
    double totalSeconds = (endTime - startTime) / 1000;

    return "" + counter + ";" + totalSeconds + ";";
  }

  public static boolean test() {
    try {
      // Create the URL object
      URL url = new URL("https://hackathon-validator.vercel.app/api/verify");

      // Open a connection
      HttpURLConnection connection = (HttpURLConnection) url.openConnection();
      connection.setRequestMethod("POST");
      connection.setRequestProperty("Content-Type", "application/json");
      connection.setDoOutput(true);

      // Set the request body
      String requestBody = "[1,2]";
      byte[] requestBodyBytes = requestBody.getBytes("UTF-8");
      connection.setRequestProperty("Content-Length", String.valueOf(requestBodyBytes.length));
      OutputStream outputStream = connection.getOutputStream();
      outputStream.write(requestBodyBytes);
      outputStream.flush();
      outputStream.close();

      // Get the response code
      int responseCode = connection.getResponseCode();
      System.out.println("Response Code: " + responseCode);

      // Close the connection
      connection.disconnect();
    } catch (IOException e) {
      e.printStackTrace();
    }
    return false;

  }

}
