class Stopwatch {
  constructor() {
    this.startTime = null;
    this.endTime = null;
  }

  start() {
    this.startTime = Date.now();
  }

  stop() {
    this.endTime = Date.now();
  }

  getElapsedMilliseconds() {
    return Date.now() - this.startTime;
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
