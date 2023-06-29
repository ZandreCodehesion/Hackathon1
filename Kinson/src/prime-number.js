const getPrimeArray = (max) => {
    const data = [2,3];
    let sieve = Array(max).fill(false);

    for (let x = 1; x * x <= max; x++)
    {
        for (let y = 1; y * y <= max; y++)
        {
            let n = (4 * x * x) + (y * y);
            if (n <= max && (n % 12 == 1 || n % 12 == 5)) sieve[n] ^= true;

            n = (3 * x * x) + (y * y);
            if (n <= max && n % 12 == 7) sieve[n] ^= true;

            n = (3 * x * x) - (y * y);
            if (x > y && n <= max && n % 12 == 11) sieve[n] ^= true;
        }
    }

    for (let r = 5; r * r <= max; r++) {
        if (sieve[r]) {
            for (i = r * r; i <= max; i += r * r) sieve[i] = false;
        }
    }

    for (let a = 5; a <= max; a++){
      if (sieve[a]) data.push(a);
    }

    return data;
}

exports.getPrimeArray = getPrimeArray;
