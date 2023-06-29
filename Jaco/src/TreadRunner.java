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

    public  boolean isPrime(int n){
    
        if (n <= 1)
            return false;
            
        if (n == 2 || n == 3)
            return true;
            
        if (n % 3 == 0)
            return false;
        
        for (int i = 5; i <= Math.sqrt(n); i = i + 6)
            if (n % i == 0 || n % (i + 2) == 0)
                return false;

        return true;
    }
  
}