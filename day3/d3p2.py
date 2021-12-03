# by Kamiel de Visser

with (open("./day3/input.txt") as file):
    numberStrings = [ line.rstrip("\n") for line in file.readlines() ]

numberLength = len(numberStrings[0])

def findCommonBit(index, numbers, leastCommon = False):
    zeroes = 0
    ones = 0
    for numberString in numbers:
        if numberString[index] == '1':
            ones += 1
        else :
            zeroes += 1
    if (leastCommon):
        return int(ones < zeroes)
    return int(ones >= zeroes)

def keepNumbers(remainingNumbers, index, bit):
    newRemainingNumbers = []
    for number in remainingNumbers:
        if int(number[index]) == bit:
               newRemainingNumbers.append(number)
    return newRemainingNumbers

def getRemainingNumber(leastCommon):
    remainingNumbers = numberStrings
    while (len(remainingNumbers) > 1):
        for i in range (0, numberLength):
            remainingNumbers = keepNumbers(remainingNumbers, i, findCommonBit(i, remainingNumbers, leastCommon))
            if (len(remainingNumbers) == 1):
                break
    return remainingNumbers[0]

if __name__ == "__main__":
    o2Rating = int(getRemainingNumber(False), 2)
    co2Rating = int(getRemainingNumber(True), 2)

    print("o2 rating:", o2Rating)
    print("co2 rating:", co2Rating)
    print("Life support rating:", o2Rating * co2Rating)
