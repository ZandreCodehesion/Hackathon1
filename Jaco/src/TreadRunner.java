import java.util.ArrayList;
import java.util.stream.IntStream;
import java.lang.*;

public class TreadRunner extends Thread{
    private Singleton data;
    public void run(){
        data = Singleton.getInstance();
        doProcess();
    }

    private  void doProcess(){
        while(!data.getIsDone()){
            ArrayList<Integer> primeNumbers = getPrimeNumbers();
            data.itterate();
        }
    }

    private  ArrayList<Integer> getPrimeNumbers (){
        ArrayList<Integer> primeNumbers = new ArrayList<Integer>();
        for (var n = 2 ; n < 1000000 ; n ++ ){
            if(n%2 ==0) continue;
            if(isPrime(n))
                primeNumbers.add(n);
        }
        return primeNumbers;
    }

    private boolean isPrime(int number) 
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
}