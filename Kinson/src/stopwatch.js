class Stopwatch {
  constructor() {
    this.startTime = null;
    this.endTime = null;
  }

  start() {
    this.startTime = new Date();
  }

  stop() {
    this.endTime = new Date();
  }

  getElapsedMilliseconds() {
    const currentTime = new Date();
    return currentTime - this.startTime;
  }

  getElapsedTicks() {
    const elapsedMilliseconds = this.endTime - this.startTime;
    const ticks = elapsedMilliseconds * 10000;
    return Math.floor(ticks);
  }


  reset() {
    this.startTime = null;
    this.endTime = null;
  }
}

module.exports = Stopwatch;
