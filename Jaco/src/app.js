'use strict'

import fetch from 'node-fetch';
import Program from './program.js'

const program = new Program();

const APIURL = "https://hackathon-validator.vercel.app/api/verify";

async function start (){
    let intArray = [];
    for(var i = 0 ; i < 5 ; i ++ ){
        intArray.push(Math.random(new Date().getTime() ));
    }
    const string = JSON.stringify(intArray)
    await fetch(APIURL , {
        method:'POST',
        body: string,
        headers: {
            'ContentType':'application/json',
            "Content-length": string.length
        }
    } )
    .then((result)=>{
        console.log(result.status);
        program.start();
    }) 
}


start ();