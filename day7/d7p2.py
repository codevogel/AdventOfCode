# by Kamiel de Visser

import time # For measuring efficiency
import math # for infinity

if __name__ == "__main__":
    start = time.time()

    # Convert input
    with (open("./day7/input.txt") as file):
        numberStrings = [line.rstrip("\n") for line in file.readlines()]
        crabPositions = [int(numberString) for numberString in numberStrings[0].split(",")]
    
    # Take min and maxpos, these represent the bounds of crab movement
    minPos, maxPos = min(crabPositions), max(crabPositions)

    # Set minFuel to inf 
    minFuel = math.inf
    # Brute force every possible position
    for desiredPos in range(minPos, maxPos):
        fuel = 0
        for pos in crabPositions:
            # find delta movement
            absPos = abs(desiredPos - pos)
            # find nth triangular number of delta movement
            # this is like a factorial with + instead of *
            fuel += (absPos * (absPos + 1)) / 2
        
        if (fuel < minFuel):
            minFuel = fuel

    end = time.time()
    print("Minimum fuel cost:", minFuel, f"({end-start:.3f} seconds)")