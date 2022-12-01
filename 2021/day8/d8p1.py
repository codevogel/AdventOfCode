# by Kamiel de Visser
# Process input

numSegments1 = 2
numSegments4 = 4
numSegments7 = 3
numSegments8 = 7

import time

if __name__ == "__main__":
    start = time.time()

    with (open("./day8/input.txt") as file):
        lines = [line.rstrip("\n") for line in file.readlines()]
        outputs = [line.split(" | ")[1] for line in lines]
        onSegments = [output.split(" ") for output in outputs]


    i = 0
    for segments in onSegments:
        for segment in segments:
            x = len(segment)
            if (x == numSegments1 or x == numSegments4 or x == numSegments7 or x == numSegments8):
                i += 1

    print(i)