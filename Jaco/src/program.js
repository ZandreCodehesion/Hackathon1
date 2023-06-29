

class Program {
    constructor() {
        this.itterations = 0;
        this.startTime = null;
        this.endTime = null;
        this.timeUp = false;
        this.timeOut = null;
    }

    start() {
        this.startTime = new Date().getTime();
        this.run();      
    }

    run () {
        var etime =null;
        do{
            this.itterations++;
            etime = new Date().getTime();
        }while((etime - this.startTime ) < 5000);
        this.endTime = etime;
        this.stop();
    }

    stop(){
        console.log(`${this.itterations},${(this.endTime - this.startTime)/1000}`)
    }
}

export default  Program;