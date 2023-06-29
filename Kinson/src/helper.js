const axios = require('axios');

const Stopwatch = require('./stopwatch');
const { getPrimeArray } = require('./prime-number');

const verifyService = async (data) => {
  return await axios({
    method: 'post',
    url: 'https://hackathon-validator.vercel.app/api/verify',
    headers: {
      'Content-Type': 'application/json',
    },
    data,
  }).then((result) => result?.data)
  .catch((error) => {
    console.error(error);
  })
}

const benchmark = async (timeInMilliseconds) => {
  const data = getPrimeArray(1_000_000);

  console.log(data);

  const isVerify = await verifyService(data);

  console.log(JSON.stringify({ isVerify }, null, 2));

  if (isVerify?.success){
    let counter = 0;
    const stopwatch = new Stopwatch();
    stopwatch.start();
    while (stopwatch.getElapsedMilliseconds() < timeInMilliseconds)
    {
        getPrimeArray();
        counter++;
    }
    stopwatch.stop();

    const totalSeconds = stopwatch.getElapsedTicks() / 10_000_000;
    console.log(`${counter};${totalSeconds};`)
  } else {
    console.error("Verification failed");
  }
}

exports.benchmark = benchmark;
