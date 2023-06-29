use std::sync::atomic::{AtomicBool, AtomicUsize, Ordering};
use std::sync::Arc;
use std::time::Instant;
use tokio::task;
use tokio::time::{self, Duration};

fn is_prime(number: i32) -> bool {
    if number == 2 {
        return true;
    }

    if number < 2 || number % 2 == 0 {
        return false;
    }

    for i in (3..).step_by(2) {
        if i * i > number {
            break;
        }

        if number % i == 0 {
            return false;
        }
    }

    return true;
}

pub fn sieve_of_atkin(limit: usize) -> Vec<bool> {
    let mut is_prime = vec![false; limit + 1];
    let sqrt_limit = (limit as f64).sqrt() as usize;

    let mut total = Duration::new(0, 0);
    let target_duration = Duration::from_millis(5000);
    let mut parses = 0;

    let mut x = 1;
    while x <= sqrt_limit {
        let mut y = 1;
        while y <= sqrt_limit {
            let mut n = (4 * x * x) + (y * y);
            if n <= limit && (n % 12 == 1 || n % 12 == 5) {
                is_prime[n] ^= true;
            }

            n = (3 * x * x) + (y * y);
            if n <= limit && n % 12 == 7 {
                is_prime[n] ^= true;
            }

            n = (3 * x * x) - (y * y);
            if x > y && n <= limit && n % 12 == 11 {
                is_prime[n] ^= true;
            }
            y += 1;
        }
        x += 1;
    }

    for n in 5..=sqrt_limit {
        if is_prime[n] {
            let mut k = n * n;
            while k <= limit {
                is_prime[k] = false;
                k += n * n;
            }
        }
    }

    is_prime[2] = true;
    is_prime[3] = true;
    is_prime
}

fn looping() {
    let mut total = Duration::new(0, 0);
    let target_duration = Duration::from_millis(5000);
    let mut parses = 0;

    while total < target_duration {
        let start = Instant::now();

        let mut numbers: Vec<i32> = Vec::new();
        for p in 1..1_000_000 {
            if is_prime(p) {
                numbers.push(p);
            }
        }

        let duration = start.elapsed();

        total += duration;
        parses += 1;

        if total >= target_duration {
            break;
        }
    }

    println!(
        "Total elapsed time: {} seconds, parsers: {}",
        total.as_millis(),
        parses,
    );
}

// #[tokio::main]
fn main() {
    let counter = Arc::new(AtomicUsize::new(0));
    let counter_clone = Arc::clone(&counter);

    let should_continue = Arc::new(AtomicBool::new(true));
    let should_continue_clone = Arc::clone(&should_continue);

    // let computation = task::spawn_blocking(move || {
    //     // Your computation here
    //     let mut i = 0;
    //     while i < 100_00_000_000 && should_continue_clone.load(Ordering::Relaxed) {
    //         for _ in 0..100 {
    //             // replace 100 with your preferred batch size
    //             let _x = i * 2; // Some arbitrary computation
    //             counter_clone.store(i, Ordering::Relaxed); // Update the counter
    //             i += 1;
    //         }
    //     }
    //     "Result of the computation" // Replace this with the actual result of your computation
    // });

    // let computation = task::spawn_blocking(move || {
    //     // Your computation here
    //     let mut i = 0;
    //     while should_continue_clone.load(Ordering::Relaxed) {
    //         let _x = i * 2; // Some arbitrary computation
    //         counter_clone.store(i, Ordering::Relaxed); // Update the counter
    //         i += 1;
    //     }
    //     "Result of the computation" // Replace this with the actual result of your computation
    // });

    // let computation = task::spawn_blocking(move || {
    //     // let start = Instant::now();

    //     let mut i = 0;
    //     loop {
    //         let _x = i * 2; // Some arbitrary computation
    //         counter_clone.store(i, Ordering::Relaxed); // Update the counter

    //         if i % 100_000_000 == 0 && !should_continue_clone.load(Ordering::Relaxed) {
    //             break;
    //         }

    //         i += 1;
    //     }

    //     // let duration: Duration = start.elapsed();
    //     // println!("Time elapsed in expensive_function() is: {:?}", duration);

    //     // "Result of the computation" // Replace this with the actual result of your computation
    // });

    // let computation = async {
    // let start = Instant::now();

    // // Your computation here
    // for _i in 0..1_000_000_000 {
    //     let _x = _i * 2; // Some arbitrary computation
    // }

    // let duration = start.elapsed();
    // println!("Time elapsed in expensive_function() is: {:?}", duration);

    // let mut total = Duration::new(0, 0);
    // let mut parses = 0;
    // let start = Instant::now();
    // let mut i = 0;

    let mut total = Duration::new(0, 0);
    let target_duration = Duration::from_millis(5000);
    let mut parses = 0;

    while total < target_duration {
        let start = Instant::now();

        sieve_of_atkin(1_000_000);
        // looping();

        let duration = start.elapsed();

        total += duration;
        parses += 1;

        if total >= target_duration {
            break;
        }
    }

    println!("{};{};", parses, total.as_secs_f32());

    // let start = Instant::now();
    // let duration = start.elapsed();
    // print!("{:?}", duration)

    // 999997

    // 318,028,839
    // 317,995,229
    // 63,484,612
    // 5,995,292
    // 317,701,471
    // 314,421,643

    // "6969;5.00023;"
    // "Result of the computation" // Replace this with the actual result of your computation
    // };

    // match time::timeout(Duration::from_secs(5), computation).await {
    //     Ok(Ok(result)) => {
    //         println!("Computation finished with result: {:?}", result);
    //         println!("Iterations: {}", counter.load(Ordering::Relaxed));
    //     }
    //     Ok(Err(_)) => {
    //         println!("The computation task panicked.");
    //         println!(
    //             "Iterations before panic: {}",
    //             counter.load(Ordering::Relaxed)
    //         );
    //     }
    //     Err(_) => {
    //         println!("Computation didn't finish within 5 seconds");
    //         println!(
    //             "Iterations before timeout: {}",
    //             counter.load(Ordering::Relaxed)
    //         );
    //         should_continue.store(false, Ordering::Relaxed);
    //         panic!("Done!")
    //     }
    // }
}

// 1,135,024,839
//  609877775
//  566931693
//  876,092,909
// 7,907,811,644

// nothing more than half will be factor
