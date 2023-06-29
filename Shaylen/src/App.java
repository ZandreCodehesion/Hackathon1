
import java.io.IOException;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Arrays;

public class App {
  public static int noOfTimes = 0;

  public static void main(String[] args) {
    String calculateBenchmark = calculateBenchmark(5000);
    System.out.println(calculateBenchmark);
  }

  public static String calculateBenchmark(int timeInMilliseconds) {
    String primeResult = doWork();
    int con = Connection.testConnection(primeResult);
    if (con == 200) {
      int counter = 0;
      long startTime = System.currentTimeMillis();
      while (System.currentTimeMillis() - startTime < timeInMilliseconds) {
        doWork();
        counter++;
      }
      long endTime = System.currentTimeMillis();
      double totalSeconds = (endTime - startTime) / 1000;

      return "" + counter + ";" + totalSeconds + ";";
    } else {
      return "";
    }
  }

  public static String doWork() {
    boolean[] primes = new boolean[1000000];
    Arrays.fill(primes, true);
    primes[0] = primes[1] = false;

    int sqrt = (int) Math.sqrt(primes.length - 1);
    for (int i = 2; i <= sqrt; i++) {
      if (primes[i]) {
        for (int j = i * i; j < primes.length; j += i) {
          primes[j] = false;
        }
      }
    }

    StringBuilder sb = new StringBuilder("[");
    for (int i = 2; i < primes.length; i++) {
      if (noOfTimes == i) {
        break;
      }
      if (primes[i]) {
        noOfTimes++;
        sb.append(i).append(",");
      }
    }
    sb.deleteCharAt(sb.length() - 1);
    sb.append("]");
    return sb.toString();

  }

  public static boolean isPrime(int n) {
    if (n <= 1) {
      return false;
    }
    for (int i = 2; i <= Math.sqrt(n); i++) {
      if (n % i == 0) {
        return false;
      }
    }
    return true;
  }

}

class Connection {
  public static int testConnection(String prime) {
    try {
      // Create the URL object
      URL url = new URL("https://hackathon-validator.vercel.app/api/verify");

      // Open a connection
      HttpURLConnection connection = (HttpURLConnection) url.openConnection();
      connection.setRequestMethod("POST");
      connection.setRequestProperty("Content-Type", "application/json");
      connection.setDoOutput(true);

      // Set the request body
      String requestBody = prime;
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
      return responseCode;
    } catch (IOException e) {
      e.printStackTrace();
    }
    return 500;

  }
}
