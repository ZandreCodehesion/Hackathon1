const axios = require('axios');

const Stopwatch = require('./stopwatch');

const doWork = () => {
  const x = Math.floor(Math.random() * 1000) + 1;
  const y = Math.floor(Math.random() * 1000) + 1;
  const z = Math.floor(Math.random() * 1000) + 1;

  const length = Math.sqrt((x * x) + (y * y) + (z * z));

  const xUnit = x / length;
  const yUnit = y / length;
  const zUnit = z / length;

  var lengthUnit = Math.sqrt((xUnit * xUnit) + (yUnit * yUnit) + (zUnit * zUnit));

  if (((lengthUnit - 1) * (lengthUnit - 1)) > 0.0003) {
    console.error(`Unit Variable Calculation is wrong ${lengthUnit} from ${x},${y},${z} to ${xUnit},${yUnit},${zUnit}`);
    return [];
  }

  return [x,y,z];
}

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
  const data = doWork();
  const isVerify = await verifyService(data);

  if (isVerify?.success){
    let counter = 0;
    const stopwatch = new Stopwatch();
    stopwatch.start();
    while (stopwatch.getElapsedMilliseconds() < timeInMilliseconds)
    {
        doWork();
        counter++;
    }
    stopwatch.stop();

    const totalSeconds = stopwatch.getElapsedTicks() / 1_000_000_000;
    console.log(`;${counter};${totalSeconds}`)
  } else {
    console.error("Verification failed");
  }
}

exports.benchmark = benchmark;
