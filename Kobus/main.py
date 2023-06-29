import time
import math
import random
import asyncio
import httpx
from concurrent.futures import ThreadPoolExecutor

class Benchmark:

    @staticmethod
    def do_work(random_generator):
        x, y, z = [random_generator.randint(0, 1000) for _ in range(3)]
        length = math.sqrt(x * x + y * y + z * z)
        x_unit, y_unit, z_unit = x / length, y / length, z / length
        length_unit = math.sqrt(x_unit**2 + y_unit**2 + z_unit**2)

        if abs(length_unit - 1) > 0.0003:
            raise Exception(f"Unit Variable Calculation is wrong {length_unit} from {x},{y},{z} to {x_unit},{y_unit},{z_unit}")

        return [x, y, z]

    @staticmethod
    async def verify_algorithm_output(algorithm_output):
        async with httpx.AsyncClient() as client:
            response = await client.post(
                "https://hackathon-validator.vercel.app/api/verify",
                json=algorithm_output
            )
            response.raise_for_status()
            result = response.json()
            return result["success"]

    @staticmethod
    async def calculate_benchmark(time_in_seconds, results):
        random_generator = random.Random()
        algorithm_result = Benchmark.do_work(random_generator)
        is_successful_algorithm = await Benchmark.verify_algorithm_output(algorithm_result)
        if not is_successful_algorithm:
            raise Exception("The value should be positive")

        counter = 0
        start_time = time.time()

        while time.time() - start_time < time_in_seconds:
            Benchmark.do_work(random_generator)
            counter += 1

        total_seconds = time.time() - start_time
        results.append((counter, total_seconds))

async def main():
    # Shared list for storing results
    results = []

    # Create tasks for asyncio.gather
    tasks = [Benchmark.calculate_benchmark(5, results) for _ in range(4)]

    # Run all tasks and wait for them to complete
    await asyncio.gather(*tasks)

    # Summarize results
    total_counts = 0
    total_seconds = 0
    for count, seconds in results:
        total_counts += count
        total_seconds += seconds

    print(f"{total_counts};{total_seconds / 4};")

if __name__ == "__main__":
    asyncio.run(main())

