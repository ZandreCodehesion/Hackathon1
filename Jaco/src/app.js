const program = new (require('./program'))(); 


program.run();

setTimeout(()=>{
    program.stop();
    console.log(program.getItterations())
} , 5000);