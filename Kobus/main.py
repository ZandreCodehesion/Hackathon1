import requests
import time
import math
import random
import json
import multiprocessing

class Benchmark:

    @staticmethod
    def calculate_benchmark(time_in_seconds, results):
        random_generator = random.Random()
        algorithm_result = Benchmark.do_work(random_generator)
        is_successful_algorithm = Benchmark.verify_algorithm_output(algorithm_result)
        if not is_successful_algorithm:
            raise Exception("The value should be positive")

        counter = 0
        start_time = time.time()

        while time.time() - start_time < time_in_seconds:
            Benchmark.do_work(random_generator)
            counter += 1

        total_seconds = time.time() - start_time
        results.put((counter, total_seconds))

    @staticmethod
    def do_work(random_generator):
        x = random_generator.randint(0, 1000)
        y = random_generator.randint(0, 1000)
        z = random_generator.randint(0, 1000)

        length = math.sqrt((x * x) + (y * y) + (z * z))

        x_unit = x / length
        y_unit = y / length
        z_unit = z / length

        length_unit = math.sqrt((x_unit * x_unit) + (y_unit * y_unit) + (z_unit * z_unit))

        if (length_unit - 1) ** 2 > 0.0003:
            raise Exception(f"Unit Variable Calculation is wrong {length_unit} from {x},{y},{z} to {x_unit},{y_unit},{z_unit}")

        return [x, y, z]

    @staticmethod
    def verify_algorithm_output(algorithm_output):
        response = requests.post(
            "https://hackathon-validator.vercel.app/api/verify",
            json=algorithm_output
        )
        response.raise_for_status()
        result = response.json()
        return result["success"]

def main():
    # Shared queue for storing results
    results = multiprocessing.Manager().Queue()

    # Create worker processes
    processes = [multiprocessing.Process(target=Benchmark.calculate_benchmark, args=(5, results)) for _ in range(4)]

    # Start all processes
    for p in processes:
        p.start()

    # Wait for all processes to finish
    for p in processes:
        p.join()

    # Summarize results
    total_counts = 0
    total_seconds = 0
    for _ in range(4):
        count, seconds = results.get()
        total_counts += count
        total_seconds += seconds

    print(f"{total_counts};{total_seconds / 4};")

if __name__ == "__main__":
    main()

