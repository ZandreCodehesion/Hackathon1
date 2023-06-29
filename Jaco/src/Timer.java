import java.util.Date;
import java.lang.*;

public class Timer extends Thread {
    private  Singleton data ;
    public void run() {
        data = Singleton.getInstance();
        StillHasTime();
    }

    public  void StillHasTime(){
        Date now = new Date();
        while(((now.getTime() - data.getStartDate().getTime())/1000) < 5 ){
            now = new Date();
        }        
        data.setEndDate(now);
        data.setIsDone();
    }
}