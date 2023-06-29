import java.util.Date;
import java.util.Random;
import java.util.ArrayList;
import java.util.stream.IntStream;
import java.io.*;    
import java.net.*;    

public class main  {

    private static Date startDate;
    private static Singleton data ;
    private static final int treadcount = 10;
    private static TreadRunner[] threads = new TreadRunner[treadcount];

    public static void main (String [] args) {
        data = Singleton.getInstance();
        start();
    }



    private static void start () {
        ArrayList<Integer> primeNumbers = getPrimeNumbers();
        String primeStringArray = listToString(primeNumbers);
        boolean isValid = HttpVerification(primeStringArray);
        if(!isValid) return;

        startTreads();
    }

    public static String listToString (ArrayList<Integer> numbers){
        String list ="[";
        for(int i = 0 ; i < numbers.size() ; i ++){
            list += Integer.toString(numbers.get(i));
            if( i < numbers.size()-1)list+=',';
            else list+=']';
        }
        return list;
    }

    private static ArrayList<Integer> getPrimeNumbers (){
        ArrayList<Integer> primeNumbers = new ArrayList<Integer>();
        for (var n = 2 ; n < 1000000 ; n ++ ){
            if(isPrime(n))
                primeNumbers.add(n);
        }
        return primeNumbers;
    }

     private static boolean isPrime(int number) 
    {
        if(number <= 2)
            return number == 2;
        else
            return  (number % 2) != 0
                &&
                IntStream.rangeClosed(3, (int) Math.sqrt(number))
                .filter(n -> n % 2 != 0)
                    .noneMatch(n -> (number % n == 0));
    }
    
    private static boolean HttpVerification (String requestBody){
        try {
            // Create the URL object
            URL url = new URL("https://hackathon-validator.vercel.app/api/verify");

            // Open a connection
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setDoOutput(true);
            Random random = new Random();
            byte[] requestBodyBytes = requestBody.getBytes("UTF-8");
            connection.setRequestProperty("Content-Length", String.valueOf(requestBodyBytes.length));
            OutputStream outputStream = connection.getOutputStream();
            outputStream.write(requestBodyBytes);
            outputStream.flush();
            outputStream.close();
            int responseCode = connection.getResponseCode();
            System.out.println("Response Code: " + responseCode);
            connection.disconnect();
            return true;
        } catch (IOException e) {
            e.printStackTrace();
        }
        return false;
    }

    private static  void startTreads(){
        data.setStartDate(new Date());
        Timer timer = new Timer();
        timer.start();
        for(int i = 0 ; i < treadcount ; i ++){
            threads[i] = new TreadRunner();
            threads[i].start();
        }
        boolean isDone = data.getIsDone();
        while(!isDone){
            System.out.print("");
            isDone = data.getIsDone();
        }
        timer.stop();
        for(int i = 0 ; i < treadcount ; i ++){
            threads[i] = new TreadRunner();
            threads[i].stop();
        }
        Date now = new Date();
        long time = (now.getTime() - data.getStartDate().getTime())/1000;
        System.out.println(data.getItterations()+","+ time);
        System.exit(0);
    }
}


class ThreadRunner extends Thread {

}