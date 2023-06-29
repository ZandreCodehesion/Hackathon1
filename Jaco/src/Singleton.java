import java.util.Date;
public class Singleton {

    private static Singleton single_instance = null;
    private static  Date startDate;
    private static Date endDate;
    private static int itterations =0;
    private  boolean isDone=false;

    public Singleton (){
        itterations = 0;
    }

    public void setStartDate(Date start){
        startDate = start;
    }
    public void setEndDate(Date end){
        endDate = end;
    }

    public static void itterate () {
        itterations++;
    }

    public Date getStartDate(){
        return startDate;
    }
    public Date getEndDate(){
        return endDate;
    }
    public static int getItterations(){
        return itterations;
    }

    public static synchronized Singleton getInstance()
    {
        if (single_instance == null)
            single_instance = new Singleton();
  
        return single_instance;
    }

    public  void setIsDone(){
        isDone = true;
    }

    public  boolean getIsDone(){
        return isDone;
    }

}