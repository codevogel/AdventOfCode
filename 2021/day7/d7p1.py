# by Kamiel de Visser


import time # For measuring efficiency
import math # for infinity

if __name__ == "__main__":
    start = time.time()
    # Process input
    with (open("./day7/input.txt") as file):
        numberStrings = [line.rstrip("\n") for line in file.readlines()]
        crabPositions = [int(numberString) for numberString in numberStrings[0].split(",")]

    # Take min and maxpos, these represent the bounds of crab movement
    minPos, maxPos = min(crabPositions), max(crabPositions)

    min = math.inf
    # Brute force all positions
    for desiredPos in range(minPos, maxPos):
        fuel = 0
        for pos in crabPositions:
            # find delta movement
            fuel += abs(desiredPos - pos)
        
        # keep minimum fuel cost
        if (fuel < min):
            min = fuel

    end = time.time()
    print("Minimum fuel cost:", min, f"({end-start:.3f} seconds)")