'use strict'

import fetch from 'node-fetch';
import Program from './program.js'

const program = new Program();

const APIURL = "https://hackathon-validator.vercel.app/api/verify";

async function start (){

    const primeNumbers  = getPrimeNumbers();
    console.log(primeNumbers)
    const string = JSON.stringify(primeNumbers)

    // await fetch(APIURL , {
    //     method:'POST',
    //     body: string,
    //     headers: {
    //         'ContentType':'application/json',
    //         "Content-length": string.length
    //     }
    // } )
    // .then((result)=>{
    //     console.log(result.status);
    //     program.start();
    // }) 
}

function getPrimeNumbers (){
    var primeNumbers = [];
    for (var n = 1 ; n < 1000000 ; n ++ ){
        var m=0,flag=0;      
        m=n/2;      
        if(!(n==0||n==1)){  
            for(var i=2;i<=m;i++){      
                if(n%i==0){      
                    flag=1;      
                    break;      
                }      
            }      
            if(flag==0)  
                primeNumbers.push(n); 
        }
    }
    return primeNumbers;
}


start ();