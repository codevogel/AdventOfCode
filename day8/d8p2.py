# by Kamiel de Visser
# Process input

import time

def overlaps(input, num):
    return all([c in input for c in numDict[num]])

if __name__ == "__main__":
    start = time.time()

    with (open("./day8/input.txt") as file):
        patterns = [line.rstrip("\n") for line in file.readlines()]
    
    sum = 0
    for pattern in patterns:
        inputs = ["".join(sorted(input)) for input in pattern.split(" | ")[0].split(' ')]
        outputs = ["".join(sorted(output)) for output in pattern.split(" | ")[1].split(' ')]
        
        numDict = {}
        for input in inputs:
            match(len(input)):
                case 2: #1
                    numDict[1] = input
                case 4: #4
                    numDict[4] = input
                case 3: #7
                    numDict[7] = input
                case 7: #8
                    numDict[8] = input

        for input in inputs:
            if (len(input) == 6): # 0, 6 or 9
                # Check if 1 does NOT overlap
                if not overlaps(input, 1):
                    # Must be 6
                    numDict[6] = input
                # Check if 4 overlaps
                elif overlaps(input, 4):
                    # Must be 9
                    numDict[9] = input
                else:
                    # Must be 0
                    numDict[0] = input

        for input in inputs:
            if (len(input) == 5): # 2, 3, 5
                # if 1 overlaps input, it is 3
                if overlaps(input, 1):
                    numDict[3] = input
                # if this input is a subsegment of 9
                elif all([c in numDict[9] for c in input]):
                    # must be 9
                    numDict[5] = input
                else:
                    # must be 2
                    numDict[2] = input

        cypherDict = {}
        for key in numDict.keys():
            cypherDict[numDict[key]] = key

        outString = ""
        for output in outputs:
            outString += str(cypherDict[output])

        sum += int(outString)

    end = time.time()
    print("Answer:", sum, f"({end-start:.3f} seconds)")

